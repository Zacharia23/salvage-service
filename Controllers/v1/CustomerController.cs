using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalvageCore.DTOs.Customer.Request;
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
public class CustomerController : ControllerBase
{
    private readonly IVerifyCodeService _codeService;
    private readonly ICustomerRepository _customerRepository;
    private readonly INotificationService _notificationService;
    private readonly IOfferRepository _offerRepository;
    private readonly IRedisCacheService _redisCacheService;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;

    public CustomerController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService,
        ICustomerRepository customerRepository, IOfferRepository offerRepository, IRedisCacheService redisCacheService, IVerifyCodeService codeService, INotificationService notificationService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _customerRepository = customerRepository;
        _offerRepository = offerRepository;
        _redisCacheService = redisCacheService;
        _codeService = codeService;
        _notificationService = notificationService;
    }

    [HttpPost]
    [Route("[Controller]/RegisterCustomer")]
    public async Task<IActionResult> RegisterCustomerAction([FromBody] CustomerRequest customer)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _customerRepository.TemporaryRegisterCustomer(customer);

        if (!result.Success) return this.RespondError(StatusCodes.Status400BadRequest, result.Message);

        return this.Respond(result.Message, result.Data);
    }

    [HttpPost]
    [Route("[Controller]/CustomerLogin")]
    public async Task<IActionResult> CustomerLoginAction([FromBody] UserLogin customer)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        try
        {
            var user = await _userManager.Users.Include(x => x.SystemUser)
                .FirstOrDefaultAsync(i => i.Email.Equals(customer.Username));

            if (user is null) return this.RespondNotFound("Requested Customer Not Found");


            if (!user.SystemUser.Role.Equals("Customer")) return this.RespondNotFound("User Found But Not a Customer Not Found");

            var result = await _signInManager.CheckPasswordSignInAsync(user, customer.Password, false);

            if (!result.Succeeded) return this.RespondError(StatusCodes.Status401Unauthorized, "Username not Found or incorrect password");

            var customerInfo = await _customerRepository.FetchCustomerInfo(user.Email);

            var response = new LoginResponse
            {
                Id = user.SystemUserId,
                Username = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Details = customerInfo,
                AccessToken = _tokenService.CreateToken(user)
            };

            return this.Respond("Customer logged in successfully", response);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to Login Customer {@exception}", exception);
            throw new InvalidOperationException(exception.Message);
        }
    }

    [HttpGet]
    [Route("[Controller]/FetchCustomers")]
    public async Task<IActionResult> FetchCustomersAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var customers = await _customerRepository.FetchCustomers();

        return this.Respond("Customers Fetched Successfully", customers);
    }

    [HttpGet]
    [Route("[Controller]/FetchCustomerProfile/{customerId:guid}")]
    public async Task<IActionResult> FetchCustomerProfileAction([FromRoute] Guid customerId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _customerRepository.CustomerExists(customerId)) return this.RespondNotFound($"Customer with id {customerId} not found");

        var customer = await _customerRepository.FetchCustomerProfile(customerId);

        return this.Respond("Customer Profile Fetched Successfully", customer);
    }

    [HttpGet]
    [Route("[Controller]/FetchActiveOffers")]
    public async Task<IActionResult> FetchActiveOffersAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var offers = await _offerRepository.FetchActiveOffers();

        return this.Respond("Offers Fetched Successfully", offers);
    }

    [HttpGet]
    [Route("[Controller]/FetchEndSoonOffers")]
    public async Task<IActionResult> FetchEndSoonOffersAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var offers = await _offerRepository.FetchEndSoonOffers();

        if (!offers.Success) return this.RespondError(StatusCodes.Status400BadRequest, offers.Message);

        return this.Respond(offers.Message, offers.Data);
    }

    [HttpGet]
    [Route("[Controller]/FetchCompleteOffers")]
    public async Task<IActionResult> FetchCompleteOffersAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var offers = await _offerRepository.FetchCompletedOffers();

        if (!offers.Success) return this.RespondError(StatusCodes.Status400BadRequest, offers.Message);

        return this.Respond(offers.Message, offers.Data);
    }

    [HttpGet]
    [Route("[Controller]/FetchOfferProfile/{offerId:guid}")]
    public async Task<IActionResult> FetchOfferProfileAction([FromRoute] Guid offerId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!await _offerRepository.OfferExists(offerId)) return this.RespondNotFound($"Offer With Id {offerId} Not Found");

        var result = await _offerRepository.FetchOfferProfile(offerId);

        return this.Respond("Offer Profile Fetched Successfully", result);
    }

    [HttpPost]
    [Route("[Controller]/CheckSubscriptionStatus")]
    public async Task<IActionResult> CheckCustomerSubAction([FromBody] CheckSubscription request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _offerRepository.CheckSubscriptionStatus(request);

        if (!result.Success) return this.RespondError(StatusCodes.Status400BadRequest, result.Message!);

        return this.Respond(result.Message!, result.Data);
    }

    [HttpPost]
    [Route("[Controller]/VerifyAuthCode")]
    public async Task<IActionResult> VerifyAuthCodeTask([FromBody] VerifyCodeRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _customerRepository.VerifyCustomerAccount(request);

        if (!result.Success) return this.RespondError(StatusCodes.Status400BadRequest, result.Errors.FirstOrDefault()!);

        return this.Respond(result.Message, result.Data);
    }

    [HttpPost]
    [Route("[Controller]/CheckAccountCompletion/{customerId:guid}")]
    public async Task<IActionResult> AccountCompleteTask([FromRoute] Guid customerId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var request = await _customerRepository.AccountComplete(customerId);

        if (!request.Success) return this.RespondError(StatusCodes.Status400BadRequest, "Account not complete");

        return this.Respond("Request Success", request.Data);
    }

    [HttpPost]
    [Route("[Controller]/CompleteRegistration")]
    public async Task<IActionResult> CompleteRegistrationTask([FromBody] CompleteRegistrationReq request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var response = await _customerRepository.CompleteRegistration(request);

        if (!response.Success) return this.RespondError(StatusCodes.Status400BadRequest, "Customer Registration Failed");

        return this.Respond("Registration Complete Successfully", response.Data.Id);
    }
}