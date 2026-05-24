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
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var number = NumberGenerator.GenerateUserNumber();

                var newUser = new SystemUser
                {
                    Username = user.Username,
                    Number = number,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    Role = user.Role
                };

                await _context.SystemUsers.AddAsync(newUser);

                var appuser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    PhoneNumber = user.Phone,
                    SystemUserId = newUser.SystemUserId,
                    Domain = "SystemUser"
                };

                // Generated Password
                var generator = new PasswordGenerator();
                var password = await generator.Generate();

                // Create user using SignIn Manager
                var createdUser = await _userManager.CreateAsync(appuser, user.Password);
                Log.Information("Generated Password => {@password} : {@email}", user.Password, appuser.Email);

                if (!createdUser.Succeeded)
                {
                    Log.Error("Failed to register user: {@email}", appuser.Email);
                    return ServiceResponse.Failure<SystemUser>($"Failed to register user {user.Email}");
                }

                // Add user to role
                await _userManager.AddToRoleAsync(appuser, user.Role);

                // Send SMS to registered user 
                var smsJob = BackgroundJob.Enqueue<INotificationService>(x => x.SendCredentialsMessage(user.Password, user.Phone, user.Email));
                var emailJob = BackgroundJob.Enqueue<INotificationService>(x => x.SendCredentialsEmail(user.Email, user.Password));

                Log.Information("Send Registration Message JobId => {@jobId}", smsJob);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return ServiceResponse.Success(newUser, "User Created Successfully");
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync();
                Log.Error("Failed to register user => {@exception}", exception.Message);
                return ServiceResponse.Failure<SystemUser>($"Failed to register user: {exception.Message}");
            }
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