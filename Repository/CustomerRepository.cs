using System.Globalization;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.Customer.Request;
using SalvageCore.DTOs.Customer.Response;
using SalvageCore.DTOs.Notification;
using SalvageCore.Helpers;
using SalvageCore.Interface;
using SalvageCore.Models;
using SalvageCore.Service;
using Serilog;

namespace SalvageCore.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly IRedisCacheService _cacheService;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IRedisCacheService cacheService)
    {
        _context = context;
        _userManager = userManager;
        _cacheService = cacheService;
    }

    public async Task<ServiceResponse<bool>> TemporaryRegisterCustomer(CustomerRequest customer)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var user = await _userManager.FindByNameAsync(customer.Email);

                if (user != null) ServiceResponse.Failure<bool>($"Another user with email {customer.Email} already exists", new List<string?> { "EMAIL_ALREADY_EXISTS" });

                var pin = NumberGenerator.GenerateCode(6);
                _cacheService.SetData($"verification_{customer.Phone}", pin);

                var tempCustomer = new TempCustomer
                {
                    Username = customer.Username,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    Password = customer.Password,
                    OtpCode = pin,
                    OtpAttempts = 1,
                    OtpExpiry = DateTime.UtcNow.AddMinutes(10),
                    IsSmsSent = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _context.TempCustomers.AddAsync(tempCustomer);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var payload = new VerificationReq
                {
                    Phone = customer.Phone,
                    Message = $"Your OTP Code is {pin}. Use it to verify your Account. Code expires in 10 minutes"
                };

                // Send Number Verification SMS
                var smsJob = BackgroundJob.Enqueue<INotificationService>(x => x.SendVerificationCode(payload));
                var emailJob = BackgroundJob.Enqueue<INotificationService>(x => x.SendVerificationCodeEmail(customer.Email, pin));

                Log.Information("SMS Verification Job Initiated Job: {@job} Phone:{@phone}", smsJob, customer.Phone);

                return ServiceResponse.Success(true, "Registration Successful, Enter OTP Code to complete");
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                Log.Error("Failed to register customer temporarily => {@exception}", exception);
                return ServiceResponse.Failure<bool>(exception.Message);
            }
        }
    }

    public async Task<ServiceResponse<SystemUser>> RegisterCustomer(CustomerRequest customer)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var existingUserByEmail = await _context.SystemUsers.FirstOrDefaultAsync(c => c.Email == customer.Email);

                if (existingUserByEmail != null)
                {
                    if (existingUserByEmail.AccountVerified) return ServiceResponse.Failure<SystemUser>($"Account with email {customer.Email} already exists and is verified.");

                    _context.SystemUsers.Remove(existingUserByEmail);
                    await _context.SaveChangesAsync();
                    Log.Information("Removed unverified account for email: {Email}", customer.Email);
                }

                var existingUserByPhone = await _context.SystemUsers.FirstOrDefaultAsync(c => c.Phone == customer.Phone);

                if (existingUserByPhone != null)
                {
                    if (existingUserByPhone.AccountVerified) return ServiceResponse.Failure<SystemUser>($"Account with phone {customer.Phone} already exists and is verified.");

                    _context.SystemUsers.Remove(existingUserByPhone);
                    await _context.SaveChangesAsync();
                    Log.Information("Removed unverified account for phone: {Phone}", customer.Phone);
                }

                var number = NumberGenerator.GenerateCustomerNumber();

                var systemUser = new SystemUser
                {
                    Username = customer.Username,
                    Number = number,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    Role = "Customer"
                };

                await _context.SystemUsers.AddAsync(systemUser);

                var appuser = new ApplicationUser
                {
                    UserName = customer.Username,
                    Email = customer.Email,
                    PhoneNumber = customer.Phone,
                    SystemUserId = systemUser.SystemUserId,
                    Domain = "Customer"
                };

                var createdUser = await _userManager.CreateAsync(appuser, customer.Password);

                if (createdUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appuser, "Customer");

                    var newCustomer = new Customer
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Email = customer.Email,
                        Phone = customer.Phone
                    };

                    await _context.Customers.AddAsync(newCustomer);

                    var pin = NumberGenerator.GenerateCode(6);
                    _cacheService.SetData($"verification_{customer.Phone}", pin);

                    var payload = new VerificationReq
                    {
                        Phone = customer.Phone,
                        Message = $"Your OTP Code is {pin}. Use it to verify your Account. Code expires in 10 minutes"
                    };

                    // Send Number Verification SMS
                    var smsJob = BackgroundJob.Enqueue<INotificationService>(x => x.SendVerificationCode(payload));

                    Log.Information("SMS Verification Job Initiated Job: {@job} Phone:{@phone}", smsJob, customer.Phone);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ServiceResponse.Success(systemUser, "Customer registered successfully");
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                Log.Error("Failed to register customer => {@exception}", exception);
                return ServiceResponse.Failure<SystemUser>(exception.Message);
            }
        }
    }

    public async Task<ServiceResponse<bool>> VerifyCustomerAccount(VerifyCodeRequest request)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var tempCustomer = await _context.TempCustomers
                    .Where(x => x.Phone == request.Phone)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync();

                if (tempCustomer == null) return ServiceResponse.Failure<bool>($"Customer with phone {request.Phone} does not found.");

                if (tempCustomer.OtpCode != null && tempCustomer.OtpCode != request.Code)
                {
                    Log.Information("Code comparison: {code1} :: {code2}", tempCustomer.OtpCode, request.Code);

                    return ServiceResponse.Failure<bool>("Verification code entered is incorrect, Check and submit again", new List<string?> { "INCORRECT_VERIFICATION_CODE" });
                }

                if (tempCustomer.OtpExpiry < DateTime.UtcNow)
                    return ServiceResponse.Failure<bool>("Verification Code Expired. Click Resend to get a new one", new List<string?> { "VERIFICATION_CODE_EXPIRED" });

                // Create as Customer 
                var newCustomer = new Customer
                {
                    FirstName = tempCustomer.FirstName,
                    LastName = tempCustomer.LastName,
                    Email = tempCustomer.Email!,
                    Phone = tempCustomer.Phone,
                    AccountVerified = true
                };

                await _context.Customers.AddAsync(newCustomer);

                var number = NumberGenerator.GenerateCustomerNumber();

                var systemUser = new SystemUser
                {
                    Username = tempCustomer.Username,
                    Number = number,
                    Email = tempCustomer.Email!,
                    Phone = tempCustomer.Phone,
                    Role = "Customer"
                };

                await _context.SystemUsers.AddAsync(systemUser);

                // Create Customer user
                var appuser = new ApplicationUser
                {
                    UserName = tempCustomer.Username,
                    Email = tempCustomer.Email,
                    PhoneNumber = tempCustomer.Phone,
                    SystemUserId = systemUser.SystemUserId,
                    Domain = "Customer"
                };

                var createdUser = await _userManager.CreateAsync(appuser, tempCustomer.Password);

                if (createdUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appuser, "Customer");
                    // Send Welcome Message
                    var emailJob = BackgroundJob.Enqueue<INotificationService>(x => x.SendWelcomeMessage(tempCustomer.Email, $"{tempCustomer.FirstName} {tempCustomer.LastName}"));
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ServiceResponse.Success(true, "Account verified successfully");
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                Log.Error("Failed to verify customer account => {@exception}", exception);
                return ServiceResponse.Failure<bool>(exception.Message);
            }
        }
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

            return ServiceResponse.Success(customer.AccountVerified);
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

    public async Task<ServiceResponse<Customer>> CompleteRegistration(CompleteRegistrationReq request)
    {
        try
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);

            if (customer == null) return ServiceResponse.Failure<Customer>($"Customer with id {request.CustomerId} not found");

            customer.AccountVerified = true;
            customer.IdentityTypeId = request.IdentityTypeId;
            customer.CardNumber = request.CardNumber;
            customer.Gender = request.Gender;
            customer.BirthDate = DateTime.SpecifyKind(DateTime.Parse(request.BirthDate, null, DateTimeStyles.RoundtripKind), DateTimeKind.Utc);
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
}