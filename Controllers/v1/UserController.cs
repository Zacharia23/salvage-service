using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalvageCore.DTOs.User;
using SalvageCore.DTOs.User.Request;
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

        if (!response.Success) return this.RespondError(StatusCodes.Status400BadRequest, response.Errors.ToString());

        return this.Respond(response.Message, response.Data);
    }

    [HttpPost]
    [Route("[Controller]/Login")]
    public async Task<IActionResult> UserLoginAction([FromBody] UserLogin userLogin)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        try
        {
            var user = await _userManager.Users
                .Include(x => x.SystemUser)
                .FirstOrDefaultAsync(i => i.Email.Equals(userLogin.Username.ToLower()));

            if (user is null) return this.RespondNotFound($"User {userLogin.Username} not found");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

            if (!result.Succeeded) return this.RespondError(StatusCodes.Status401Unauthorized, "Username not Found or incorrect password");

            var response = new LoginResponse
            {
                Id = user.SystemUserId,
                Email = user.Email,
                Username = user.UserName,
                Phone = user.PhoneNumber,
                Details = null,
                AccessToken = _tokenService.CreateToken(user)
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

        if (!result.Success) return this.RespondError(StatusCodes.Status400BadRequest, result.Errors.ToString());

        return this.Respond(result.Message, result.Data);
    }

    [HttpGet]
    [Route("[Controller]/FetchUserProfile/{userId:guid}")]
    public async Task<IActionResult> FetchUserProfileAction([FromRoute] Guid userId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _userRepository.FetchUserDetails(userId);

        if (!result.Success) return this.RespondError(StatusCodes.Status400BadRequest, result.Errors.ToString());

        return this.Respond(result.Message, result.Data);
    }
}