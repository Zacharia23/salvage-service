using System.Globalization;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.Customer.Request;
using SalvageCore.DTOs.Customer.Response;
using SalvageCore.DTOs.Notification;
using SalvageCore.DTOs.User;
using SalvageCore.Enums;
using SalvageCore.Helpers;
using SalvageCore.Interface;
using SalvageCore.Models;
using SalvageCore.Service;
using Serilog;

namespace SalvageCore.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<TempCustomer> _otpHasher;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerRepository(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IPasswordHasher<TempCustomer> otpHasher,
        ITokenService tokenService)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _otpHasher = otpHasher;
        _tokenService = tokenService;
    }

    public async Task<ServiceResponse<RegistrationResponse>> TemporaryRegisterCustomer(CustomerRequest customer)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var email = customer.Email.Trim().ToLowerInvariant();
            var phone = customer.Phone.Trim();

            var identityExists = await _userManager.FindByEmailAsync(email) is not null;
            var phoneExists = await _userManager.Users.AnyAsync(u => u.PhoneNumber == phone);

            if (identityExists || phoneExists)
            {
                return ServiceResponse.Failure<RegistrationResponse>(
                    "Unable to create an account with the supplied details.",
                    new List<string> { "ACCOUNT_ALREADY_EXISTS" });
            }

            var systemUser = new SystemUser
            {
                Username = customer.Username.Trim(),
                Number = NumberGenerator.GenerateCustomerNumber(),
                Email = email,
                Phone = phone,
                Role = "Customer",
                AccountVerified = false,
                Status = StatusEnums.Pending
            };

            await _context.SystemUsers.AddAsync(systemUser);

            var appUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = phone,
                SystemUserId = systemUser.SystemUserId,
                Domain = "Customer",
                EmailConfirmed = false,
                PhoneNumberConfirmed = false
            };

            var createdUser = await _userManager.CreateAsync(appUser, customer.Password);
            if (!createdUser.Succeeded)
            {
                await transaction.RollbackAsync();
                return ServiceResponse.Failure<RegistrationResponse>(
                    "Unable to create customer account.",
                    createdUser.Errors.Select(error => error.Code).ToList());
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "Customer");
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return ServiceResponse.Failure<RegistrationResponse>(
                    "Unable to assign customer access.",
                    roleResult.Errors.Select(error => error.Code).ToList());
            }

            var newCustomer = new Customer
            {
                ApplicationUserId = appUser.Id,
                FirstName = customer.FirstName.Trim(),
                LastName = customer.LastName.Trim(),
                Email = email,
                Phone = phone,
                AccountVerified = false
            };

            await _context.Customers.AddAsync(newCustomer);

            var pin = NumberGenerator.GenerateCode(6);
            var now = DateTime.UtcNow;
            var verification = new TempCustomer
            {
                ApplicationUserId = appUser.Id,
                Phone = phone,
                Email = email,
                OtpHash = string.Empty,
                Purpose = AuthChallengePurpose.Registration,
                OtpAttempts = 0,
                OtpExpiry = now.AddMinutes(10),
                LastSentAt = now,
                CreatedDate = now
            };
            verification.OtpHash = _otpHasher.HashPassword(verification, pin);

            await _context.TempCustomers.AddAsync(verification);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TryQueueVerificationMessages(email, phone, pin);

            return ServiceResponse.Success(new RegistrationResponse
            {
                VerificationId = verification.Id,
                ExpiresAt = verification.OtpExpiry
            }, "Registration started. Enter the verification code to activate the account.");
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            Log.Error("Failed to register customer temporarily => {@exception}", exception);
            return ServiceResponse.Failure<RegistrationResponse>("Unable to create customer account.");
        }
    }

    public async Task<ServiceResponse<LoginResponse>> VerifyCustomerAccount(VerifyCodeRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var transactionCompleted = false;
        try
        {
            var verificationQuery = _context.TempCustomers.AsQueryable();
            TempCustomer? verification;

            if (request.VerificationId.HasValue)
            {
                verification = await verificationQuery
                    .FirstOrDefaultAsync(x =>
                        x.Id == request.VerificationId.Value &&
                        x.Purpose == AuthChallengePurpose.Registration);
            }
            else
            {
                verification = await verificationQuery
                    .Where(x =>
                        x.Phone == request.Phone.Trim() &&
                        x.Purpose == AuthChallengePurpose.Registration)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync();
            }

            if (verification is null || verification.ConsumedAt.HasValue)
                return ServiceResponse.Failure<LoginResponse>(
                    "Verification request is invalid.",
                    new List<string> { "INVALID_VERIFICATION_REQUEST" });

            if (verification.OtpExpiry <= DateTime.UtcNow)
                return ServiceResponse.Failure<LoginResponse>(
                    "Verification code expired. Request a new code.",
                    new List<string> { "VERIFICATION_CODE_EXPIRED" });

            if (verification.OtpAttempts >= 5)
                return ServiceResponse.Failure<LoginResponse>(
                    "Too many verification attempts. Request a new code.",
                    new List<string> { "VERIFICATION_ATTEMPTS_EXCEEDED" });

            var verificationResult = _otpHasher.VerifyHashedPassword(
                verification,
                verification.OtpHash,
                request.Code.Trim());

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                verification.OtpAttempts++;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                transactionCompleted = true;

                return ServiceResponse.Failure<LoginResponse>(
                    "Verification code is incorrect.",
                    new List<string> { "INCORRECT_VERIFICATION_CODE" });
            }

            var appUser = await _userManager.Users
                .Include(user => user.SystemUser)
                .FirstOrDefaultAsync(user => user.Id == verification.ApplicationUserId);
            var customer = await _context.Customers
                .FirstOrDefaultAsync(item => item.ApplicationUserId == verification.ApplicationUserId);

            if (appUser?.SystemUser is null ||
                customer is null ||
                !IsCustomerRole(appUser.SystemUser.Role))
                return ServiceResponse.Failure<LoginResponse>(
                    "Customer account could not be activated.",
                    new List<string> { "CUSTOMER_ACCOUNT_INCOMPLETE" });

            appUser.EmailConfirmed = true;
            appUser.PhoneNumberConfirmed = true;
            appUser.SystemUser.AccountVerified = true;
            appUser.SystemUser.Status = StatusEnums.Active;
            customer.AccountVerified = true;
            verification.ConsumedAt = DateTime.UtcNow;

            var updateUser = await _userManager.UpdateAsync(appUser);
            if (!updateUser.Succeeded)
            {
                return ServiceResponse.Failure<LoginResponse>(
                    "Customer account could not be activated.",
                    updateUser.Errors.Select(error => error.Code).ToList());
            }

            await _context.SaveChangesAsync();
            var loginResponse = CreateLoginResponse(appUser, customer);

            await transaction.CommitAsync();
            transactionCompleted = true;

            try
            {
                BackgroundJob.Enqueue<INotificationService>(
                    service => service.SendWelcomeMessage(customer.Email, $"{customer.FirstName} {customer.LastName}"));
            }
            catch (Exception notificationException)
            {
                Log.Warning(notificationException, "Customer verified, but welcome notification could not be queued.");
            }

            return ServiceResponse.Success(loginResponse, "Account verified successfully.");
        }
        catch (Exception exception)
        {
            Log.Error("Failed to verify customer account => {@exception}", exception);

            if (!transactionCompleted)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackException)
                {
                    Log.Error(rollbackException, "Failed to roll back customer verification transaction.");
                }
            }

            return ServiceResponse.Failure<LoginResponse>("Unable to verify customer account.");
        }
    }

    public async Task<ServiceResponse<RegistrationResponse>> StartCustomerLogin(CustomerLoginRequest request)
    {
        try
        {
            var phone = request.Phone.Trim();
            var user = await _userManager.Users
                .Include(item => item.SystemUser)
                .FirstOrDefaultAsync(item => item.PhoneNumber == phone);

            if (user?.SystemUser is null ||
                !IsCustomerRole(user.SystemUser.Role))
            {
                return ServiceResponse.Failure<RegistrationResponse>(
                    "Invalid phone number or PIN.",
                    new List<string> { "INVALID_CREDENTIALS" });
            }

            var pinResult = await _signInManager.CheckPasswordSignInAsync(user, request.Pin, true);
            if (pinResult.IsLockedOut)
            {
                return ServiceResponse.Failure<RegistrationResponse>(
                    "Too many failed login attempts. Try again later.",
                    new List<string> { "ACCOUNT_LOCKED" });
            }

            if (!pinResult.Succeeded)
            {
                return ServiceResponse.Failure<RegistrationResponse>(
                    "Invalid phone number or PIN.",
                    new List<string> { "INVALID_CREDENTIALS" });
            }

            if (!user.PhoneNumberConfirmed ||
                !user.SystemUser.AccountVerified ||
                user.SystemUser.Status != StatusEnums.Active)
            {
                return ServiceResponse.Failure<RegistrationResponse>(
                    "Customer account is not active.",
                    new List<string> { "ACCOUNT_NOT_ACTIVE" });
            }

            var now = DateTime.UtcNow;
            var activeChallenges = await _context.TempCustomers
                .Where(item =>
                    item.ApplicationUserId == user.Id &&
                    item.Purpose == AuthChallengePurpose.Login &&
                    !item.ConsumedAt.HasValue)
                .ToListAsync();

            foreach (var activeChallenge in activeChallenges)
                activeChallenge.ConsumedAt = now;

            var code = NumberGenerator.GenerateCode(6);
            var challenge = new TempCustomer
            {
                ApplicationUserId = user.Id,
                Phone = phone,
                Email = user.Email ?? string.Empty,
                OtpHash = string.Empty,
                Purpose = AuthChallengePurpose.Login,
                OtpAttempts = 0,
                OtpExpiry = now.AddMinutes(5),
                LastSentAt = now,
                CreatedDate = now
            };
            challenge.OtpHash = _otpHasher.HashPassword(challenge, code);

            await _context.TempCustomers.AddAsync(challenge);
            await _context.SaveChangesAsync();

            TryQueueVerificationMessages(challenge.Email, phone, code, sendEmail: false);

            return ServiceResponse.Success(new RegistrationResponse
            {
                VerificationId = challenge.Id,
                ExpiresAt = challenge.OtpExpiry
            }, "A login code has been sent to the registered phone number.");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to start customer OTP login.");
            return ServiceResponse.Failure<RegistrationResponse>("Unable to start customer login.");
        }
    }

    public async Task<ServiceResponse<LoginResponse>> VerifyCustomerLogin(VerifyCodeRequest request)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var transactionCompleted = false;

        try
        {
            var challengeQuery = _context.TempCustomers
                .Where(item => item.Purpose == AuthChallengePurpose.Login);

            TempCustomer? challenge;
            if (request.VerificationId.HasValue)
            {
                challenge = await challengeQuery
                    .FirstOrDefaultAsync(item => item.Id == request.VerificationId.Value);
            }
            else
            {
                challenge = await challengeQuery
                    .Where(item => item.Phone == request.Phone.Trim())
                    .OrderByDescending(item => item.CreatedDate)
                    .FirstOrDefaultAsync();
            }

            if (challenge is null || challenge.ConsumedAt.HasValue)
                return ServiceResponse.Failure<LoginResponse>(
                    "Login verification request is invalid.",
                    new List<string> { "INVALID_LOGIN_CHALLENGE" });

            if (challenge.OtpExpiry <= DateTime.UtcNow)
                return ServiceResponse.Failure<LoginResponse>(
                    "Login code expired. Start login again.",
                    new List<string> { "LOGIN_CODE_EXPIRED" });

            if (challenge.OtpAttempts >= 5)
                return ServiceResponse.Failure<LoginResponse>(
                    "Too many verification attempts. Start login again.",
                    new List<string> { "LOGIN_ATTEMPTS_EXCEEDED" });

            var codeResult = _otpHasher.VerifyHashedPassword(
                challenge,
                challenge.OtpHash,
                request.Code.Trim());

            if (codeResult == PasswordVerificationResult.Failed)
            {
                challenge.OtpAttempts++;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                transactionCompleted = true;

                return ServiceResponse.Failure<LoginResponse>(
                    "Login code is incorrect.",
                    new List<string> { "INCORRECT_LOGIN_CODE" });
            }

            var user = await _userManager.Users
                .Include(item => item.SystemUser)
                .FirstOrDefaultAsync(item => item.Id == challenge.ApplicationUserId);
            var customer = await _context.Customers
                .FirstOrDefaultAsync(item => item.ApplicationUserId == challenge.ApplicationUserId);

            if (user?.SystemUser is null ||
                customer is null ||
                !IsCustomerRole(user.SystemUser.Role) ||
                !user.PhoneNumberConfirmed ||
                !user.SystemUser.AccountVerified ||
                user.SystemUser.Status != StatusEnums.Active)
            {
                return ServiceResponse.Failure<LoginResponse>(
                    "Customer account is not active.",
                    new List<string> { "ACCOUNT_NOT_ACTIVE" });
            }

            challenge.ConsumedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var loginResponse = CreateLoginResponse(user, customer);
            await transaction.CommitAsync();
            transactionCompleted = true;

            return ServiceResponse.Success(loginResponse, "Customer logged in successfully.");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to verify customer OTP login.");

            if (!transactionCompleted)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackException)
                {
                    Log.Error(rollbackException, "Failed to roll back customer login verification.");
                }
            }

            return ServiceResponse.Failure<LoginResponse>("Unable to verify customer login.");
        }
    }

    public async Task<ServiceResponse<RegistrationResponse>> ResendVerificationCode(ResendVerificationRequest request)
    {
        var verification = await _context.TempCustomers
            .FirstOrDefaultAsync(item => item.Id == request.VerificationId);

        if (verification is null || verification.ConsumedAt.HasValue)
            return ServiceResponse.Failure<RegistrationResponse>(
                "Verification request is invalid.",
                new List<string> { "INVALID_VERIFICATION_REQUEST" });

        var now = DateTime.UtcNow;
        if (verification.LastSentAt.AddMinutes(1) > now)
            return ServiceResponse.Failure<RegistrationResponse>(
                "Please wait before requesting another verification code.",
                new List<string> { "VERIFICATION_RESEND_COOLDOWN" });

        var pin = NumberGenerator.GenerateCode(6);
        verification.OtpHash = _otpHasher.HashPassword(verification, pin);
        verification.OtpAttempts = 0;
        verification.OtpExpiry = verification.Purpose == AuthChallengePurpose.Login
            ? now.AddMinutes(5)
            : now.AddMinutes(10);
        verification.LastSentAt = now;

        await _context.SaveChangesAsync();
        TryQueueVerificationMessages(
            verification.Email,
            verification.Phone,
            pin,
            sendEmail: verification.Purpose == AuthChallengePurpose.Registration);

        return ServiceResponse.Success(new RegistrationResponse
        {
            VerificationId = verification.Id,
            ExpiresAt = verification.OtpExpiry
        }, "A new verification code has been sent.");
    }

    public async Task<ICollection<CustomerList>> FetchCustomers()
    {
        try
        {
            var customers = await _context.Customers
                .OrderBy(s => s.CreatedDate)
                .Select(x => new CustomerList
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email,
                    Phone = x.Phone,
                    IdentityType = x.IdentityType.Name,
                    CardNumber = x.CardNumber,
                    Gender = x.Gender,
                    BirthDate = x.BirthDate,
                    AcceptedTerms = x.AcceptedTerms,
                    AccountVerified = x.AccountVerified,
                    Region = $"{x.Region.RegionName}, {x.Region.RegionIso}",
                    Bids = x.Bids.Count,
                    Awards = x.Bids.Select(b => b.Awarded.Equals(true)).Count(),
                    //LastLogin = x.ActivityLogs.Select(l => l.Timestamp).LastOrDefault(),
                    CreatedDate = x.CreatedDate,
                    Status = "Active"
                })
                .ToListAsync();

            return customers;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch customer list => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<CustomerProfile?> FetchCustomerProfile(Guid customerId)
    {
        try
        {
            var result = await _context.Customers
                .Where(c => c.Id.Equals(customerId))
                .Select(x => new CustomerProfile
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email,
                    Phone = x.Phone,
                    IdentityType = x.IdentityType.Name,
                    CardNumber = x.CardNumber,
                    Gender = x.Gender.ToString(),
                    BirthDate = x.BirthDate,
                    AcceptedTerms = x.AcceptedTerms,
                    AccountVerified = x.AccountVerified,
                    Region = $"{x.Region.RegionName}, {x.Region.RegionIso}",
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();

            return result ?? null;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch customer details => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<CustomerInfo?> FetchCustomerInfo(string email)
    {
        try
        {
            var customer = await _context.Customers.Where(c => c.Email.Equals(email))
                .Select(x => new CustomerInfo
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email,
                    Phone = x.Phone,
                    IdentityType = x.IdentityType.Name,
                    CardNumber = x.CardNumber,
                    Gender = x.Gender,
                    BirthDate = x.BirthDate,
                    AcceptedTerms = x.AcceptedTerms,
                    AccountVerified = x.AccountVerified,
                    Region = $"{x.Region.RegionName}, {x.Region.RegionIso}",
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();

            return customer ?? null;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch customer information => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<bool> CustomerExists(Guid customerId)
    {
        try
        {
            return await _context.Customers.AnyAsync(c => c.Id.Equals(customerId));
        }
        catch (Exception exception)
        {
            Log.Error("Failed to check if customer exists => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ICollection<IdTypeList>> FetchIdentityTypes()
    {
        try
        {
            return await _context.IdentityTypes.Select(x => new IdTypeList
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedDate = x.CreatedDate
            }).ToListAsync();
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch identity types => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ServiceResponse<bool>> AccountComplete(Guid customerId)
    {
        try
        {
            var customer = await _context.Customers.Where(c => c.Id.Equals(customerId)).FirstOrDefaultAsync();

            if (customer == null) return ServiceResponse.Failure<bool>("Customer not found");

            return ServiceResponse.Success(IsProfileComplete(customer));
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return ServiceResponse.Failure<bool>(exception.Message);
        }
    }

    public async Task<bool> CustomerPhoneExists(string phone)
    {
        try
        {
            return await _context.Customers.AnyAsync(x => x.Phone.Equals(phone));
        }
        catch (Exception exception)
        {
            Log.Error("Failed to check if customer phone exists => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<bool> CustomerEmailExists(string email)
    {
        try
        {
            return await _context.Customers.AnyAsync(x => x.Email.Equals(email));
        }
        catch (Exception exception)
        {
            Log.Error("Failed to check if customer email exists => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<bool> CustomerIdentityExists(string cardNumber)
    {
        try
        {
            return await _context.Customers.AnyAsync(x => x.CardNumber.Equals(cardNumber));
        }
        catch (Exception exception)
        {
            Log.Error("Failed to check if customer identity exists => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    public async Task<ServiceResponse<Customer>> CompleteRegistration(string applicationUserId, CompleteRegistrationReq request)
    {
        try
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(item => item.ApplicationUserId == applicationUserId);

            if (customer == null) return ServiceResponse.Failure<Customer>("Customer account not found.");

            if (!customer.AccountVerified)
                return ServiceResponse.Failure<Customer>("Customer account must be verified first.");

            if (!request.AcceptedTerms)
                return ServiceResponse.Failure<Customer>("Terms and conditions must be accepted.");

            if (request.IdentityTypeId == Guid.Empty ||
                !await _context.IdentityTypes.AnyAsync(item => item.Id == request.IdentityTypeId))
                return ServiceResponse.Failure<Customer>("Identity type is invalid.");

            if (request.RegionId == Guid.Empty ||
                !await _context.Regions.AnyAsync(item => item.Id == request.RegionId))
                return ServiceResponse.Failure<Customer>("Region is invalid.");

            var identityInUse = await _context.Customers.AnyAsync(item =>
                item.Id != customer.Id &&
                item.CardNumber == request.CardNumber.Trim());
            if (identityInUse)
                return ServiceResponse.Failure<Customer>("Identity number is already registered.");

            if (!DateTime.TryParse(request.BirthDate, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var birthDate))
                return ServiceResponse.Failure<Customer>("Birth date is invalid.");

            if (birthDate.Date > DateTime.UtcNow.Date)
                return ServiceResponse.Failure<Customer>("Birth date cannot be in the future.");

            customer.AccountVerified = true;
            customer.IdentityTypeId = request.IdentityTypeId;
            customer.CardNumber = request.CardNumber.Trim();
            customer.Gender = request.Gender;
            customer.BirthDate = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc);
            customer.AcceptedTerms = request.AcceptedTerms;
            customer.RegionId = request.RegionId;
            customer.TaxNumber = request.TaxNumber;
            customer.VNumber = request.VNumber;

            await _context.SaveChangesAsync();

            return ServiceResponse.Success(customer);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to complete customer registration => {@exception}", exception);
            return ServiceResponse.Failure<Customer>(exception.Message);
        }
    }

    private LoginResponse CreateLoginResponse(ApplicationUser appUser, Customer customer)
    {
        const string customerRole = "Customer";

        return new LoginResponse
        {
            Id = appUser.SystemUserId,
            CustomerId = customer.Id,
            Username = appUser.SystemUser?.Username ?? appUser.UserName,
            Email = appUser.Email,
            Phone = appUser.PhoneNumber,
            Role = customerRole,
            Details = null,
            AccessToken = _tokenService.CreateToken(appUser, customer.Id),
            ExpiresIn = _tokenService.AccessTokenLifetimeSeconds,
            ProfileComplete = IsProfileComplete(customer)
        };
    }

    private static bool IsProfileComplete(Customer customer)
    {
        return customer.IdentityTypeId.HasValue
               && !string.IsNullOrWhiteSpace(customer.CardNumber)
               && customer.Gender.HasValue
               && customer.BirthDate.HasValue
               && customer.AcceptedTerms
               && customer.RegionId.HasValue;
    }

    private static bool IsCustomerRole(string? role)
    {
        return role?.Equals("Customer", StringComparison.OrdinalIgnoreCase) == true;
    }

    private static void TryQueueVerificationMessages(
        string email,
        string phone,
        string pin,
        bool sendEmail = true)
    {
        try
        {
            var payload = new VerificationReq
            {
                Phone = phone,
                Message = sendEmail
                    ? $"Your verification code is {pin}. It expires in 10 minutes."
                    : $"Your login code is {pin}. It expires in 5 minutes."
            };

            var smsJob = BackgroundJob.Enqueue<INotificationService>(
                service => service.SendVerificationCode(payload));
            if (sendEmail)
            {
                BackgroundJob.Enqueue<INotificationService>(
                    service => service.SendVerificationCodeEmail(email, pin));
            }

            Log.Information("Verification jobs initiated. SMS job: {JobId}, Phone: {Phone}", smsJob, phone);
        }
        catch (Exception exception)
        {
            Log.Warning(exception, "Verification created, but notifications could not be queued for {Phone}.", phone);
        }
    }
}
