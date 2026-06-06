using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalvageCore.Data;
using SalvageCore.DTOs.User;
using SalvageCore.DTOs.User.Request;
using SalvageCore.DTOs.User.Response;
using SalvageCore.Helpers;
using SalvageCore.Interface;
using SalvageCore.Models;
using SalvageCore.Service;
using Serilog;

namespace SalvageCore.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ServiceResponse<ICollection<UserListResponse>>> FetchUsers()
    {
        try
        {
            var results = await _context.SystemUsers
                .Where(r => r.Role != "Customer")
                .Select(x => new UserListResponse
                {
                    Id = x.SystemUserId,
                    Username = x.Username,
                    Email = x.Email,
                    Phone = x.Phone,
                    Role = x.Role,
                    Number = x.Number,
                    Address = x.Address,
                    Status = x.Status.ToString(),
                    CreatedDate = x.CreatedDate
                })
                .ToListAsync();

            return ServiceResponse.Success<ICollection<UserListResponse>>(results, "Users Fetched Successfully");
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch users => {@exception}", exception);
            return ServiceResponse.Failure<ICollection<UserListResponse>>("Failed to fetch users", new List<string> { exception.Message });
            ;
        }
    }

    public async Task<ServiceResponse<UserProfileResponse>> FetchUserDetails(Guid userId)
    {
        try
        {
            var results = await _context.SystemUsers
                .Where(u => u.SystemUserId.Equals(userId))
                .Select(x => new UserProfileResponse
                {
                    Id = x.SystemUserId,
                    Username = x.Username,
                    Email = x.Email,
                    Phone = x.Phone,
                    Role = x.Role,
                    Number = x.Number,
                    Address = x.Address,
                    Status = x.Status.ToString(),
                    CreatedDate = x.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (results is null)
            {
                return ServiceResponse.Failure<UserProfileResponse>("Failed to fetch user details", new List<string> { "User Not Found" });
                ;
            }

            return ServiceResponse.Success(results, "Users Fetched Successfully");
        }
        catch (Exception exception)
        {
            Log.Error("Failed to fetch users details => {@exception}", exception);
            return ServiceResponse.Failure<UserProfileResponse>("Failed to fetch user details", new List<string> { exception.Message });
            ;
        }
    }

    public async Task<ServiceResponse<SystemUser>> RegisterUser(UserRequest user)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var transactionCompleted = false;

        try
        {
            var email = user.Email.Trim().ToLowerInvariant();
            var phone = user.Phone.Trim();
            var role = user.Role.Trim();

            if (!await _roleManager.RoleExistsAsync(role))
                return ServiceResponse.Failure<SystemUser>(
                    "The selected role does not exist.",
                    new List<string> { "INVALID_ROLE" });

            if (role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                return ServiceResponse.Failure<SystemUser>(
                    "Customer accounts must use the customer registration flow.",
                    new List<string> { "INVALID_USER_ROLE" });

            var emailExists = await _userManager.FindByEmailAsync(email) is not null;
            var phoneExists = await _userManager.Users.AnyAsync(item => item.PhoneNumber == phone);
            if (emailExists || phoneExists)
                return ServiceResponse.Failure<SystemUser>(
                    "A user with the supplied email or phone already exists.",
                    new List<string> { "USER_ALREADY_EXISTS" });

            var newUser = new SystemUser
            {
                Username = user.Username.Trim(),
                Number = NumberGenerator.GenerateUserNumber(),
                Email = email,
                Phone = phone,
                Address = user.Address.Trim(),
                Role = role,
                AccountVerified = true
            };

            await _context.SystemUsers.AddAsync(newUser);

            var appUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = phone,
                SystemUserId = newUser.SystemUserId,
                Domain = "SystemUser",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var createdUser = await _userManager.CreateAsync(appUser, user.Password);
            if (!createdUser.Succeeded)
            {
                return ServiceResponse.Failure<SystemUser>(
                    "Failed to create user account.",
                    createdUser.Errors.Select(error => $"{error.Code}: {error.Description}").ToList());
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, role);
            if (!roleResult.Succeeded)
            {
                return ServiceResponse.Failure<SystemUser>(
                    "Failed to assign the selected role.",
                    roleResult.Errors.Select(error => $"{error.Code}: {error.Description}").ToList());
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            transactionCompleted = true;

            try
            {
                var smsJob = BackgroundJob.Enqueue<INotificationService>(
                    service => service.SendCredentialsMessage(user.Password, phone, email));
                BackgroundJob.Enqueue<INotificationService>(
                    service => service.SendCredentialsEmail(email, user.Password));

                Log.Information("User registration notification queued. JobId: {JobId}", smsJob);
            }
            catch (Exception notificationException)
            {
                Log.Warning(notificationException, "User created, but registration notification could not be queued.");
            }

            return ServiceResponse.Success(newUser, "User created successfully.");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to register user.");

            if (!transactionCompleted)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch (Exception rollbackException)
                {
                    Log.Error(rollbackException, "Failed to roll back user registration.");
                }
            }

            return ServiceResponse.Failure<SystemUser>(
                "Failed to register user.",
                new List<string> { exception.Message });
        }
    }

    public async Task<bool> UserExists(Guid userId)
    {
        try
        {
            var results = await _context.SystemUsers.AnyAsync(x => x.SystemUserId.Equals(userId));
            return results;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to get user => {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }
}
