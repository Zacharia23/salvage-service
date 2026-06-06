using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalvageCore.DTOs.User;
using SalvageCore.DTOs.User.Request;
using SalvageCore.Enums;
using SalvageCore.Extensions;
using SalvageCore.Interface;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class UserController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("[Controller]/RegisterUser")]
    public async Task<IActionResult> RegisterUserAction([FromBody] UserRequest user)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var response = await _userRepository.RegisterUser(user);

        if (!response.Success)
            return this.RespondError(StatusCodes.Status400BadRequest, GetErrorMessage(response.Message, response.Errors));

        return this.Respond(response.Message ?? "User created successfully.", response.Data!);
    }

    [HttpPost]
    [Route("[Controller]/Login")]
    public async Task<IActionResult> UserLoginAction([FromBody] UserLogin userLogin)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        try
        {
            var email = userLogin.Username.Trim().ToLowerInvariant();
            var user = await _userManager.Users
                .Include(x => x.SystemUser)
                .FirstOrDefaultAsync(i => i.NormalizedEmail == _userManager.NormalizeEmail(email));

            if (user?.SystemUser is null ||
                user.SystemUser.Role.Equals("Customer", StringComparison.OrdinalIgnoreCase))
                return this.RespondError(StatusCodes.Status401Unauthorized, "Invalid credentials.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, true);

            if (result.IsLockedOut)
                return this.RespondError(StatusCodes.Status429TooManyRequests, "Too many failed login attempts. Try again later.");

            if (!result.Succeeded)
                return this.RespondError(StatusCodes.Status401Unauthorized, "Invalid credentials.");

            if (user.SystemUser.Status != StatusEnums.Active)
                return this.RespondError(StatusCodes.Status403Forbidden, "User account is not active.");

            var response = new LoginResponse
            {
                Id = user.SystemUserId,
                Email = user.Email,
                Username = user.SystemUser.Username,
                Phone = user.PhoneNumber,
                Role = user.SystemUser.Role,
                Details = null,
                AccessToken = _tokenService.CreateToken(user),
                ExpiresIn = _tokenService.AccessTokenLifetimeSeconds
            };

            return this.Respond("Login Success", response);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Login User {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    [HttpPost]
    [Route("[Controller]/ResetPassword")]
    public async Task<IActionResult> ResetPasswordAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost]
    [Route("[Controller]/ChangePassword")]
    public async Task<IActionResult> ChangePasswordAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet]
    [Route("[Controller]/FetchCustomers")]
    public async Task<IActionResult> FetchCustomersAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet]
    [Route("[Controller]/FetchUsers")]
    public async Task<IActionResult> FetchUsersAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _userRepository.FetchUsers();

        if (!result.Success)
            return this.RespondError(StatusCodes.Status400BadRequest, GetErrorMessage(result.Message, result.Errors));

        return this.Respond(result.Message ?? "Users fetched successfully.", result.Data!);
    }

    [HttpGet]
    [Route("[Controller]/FetchUserProfile/{userId:guid}")]
    public async Task<IActionResult> FetchUserProfileAction([FromRoute] Guid userId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _userRepository.FetchUserDetails(userId);

        if (!result.Success)
            return this.RespondError(StatusCodes.Status400BadRequest, GetErrorMessage(result.Message, result.Errors));

        return this.Respond(result.Message ?? "User profile fetched successfully.", result.Data!);
    }

    private static string GetErrorMessage(string? message, IEnumerable<string> errors)
    {
        var errorMessage = string.Join(", ", errors.Where(error => !string.IsNullOrWhiteSpace(error)));
        return !string.IsNullOrWhiteSpace(errorMessage)
            ? errorMessage
            : message ?? "Request failed.";
    }
}
