using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SalvageCore.DTOs.Customer.Request;
using SalvageCore.Extensions;
using SalvageCore.Interface;

namespace SalvageCore.Controllers.v1;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOfferRepository _offerRepository;

    public CustomerController(
        ICustomerRepository customerRepository,
        IOfferRepository offerRepository)
    {
        _customerRepository = customerRepository;
        _offerRepository = offerRepository;
    }

    [HttpPost]
    [Route("[Controller]/RegisterCustomer")]
    [AllowAnonymous]
    [EnableRateLimiting("customer-auth")]
    public async Task<IActionResult> RegisterCustomerAction([FromBody] CustomerRequest customer)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _customerRepository.TemporaryRegisterCustomer(customer);

        if (!result.Success) return this.RespondError(
            StatusCodes.Status400BadRequest,
            result.Message ?? "Customer registration failed.");

        return this.Respond(result.Message ?? "Registration started.", result.Data!);
    }

    [HttpPost]
    [Route("[Controller]/CustomerLogin")]
    [AllowAnonymous]
    [EnableRateLimiting("customer-auth")]
    public async Task<IActionResult> CustomerLoginAction([FromBody] CustomerLoginRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _customerRepository.StartCustomerLogin(request);
        if (!result.Success)
        {
            var statusCode = result.Errors.Contains("ACCOUNT_LOCKED")
                ? StatusCodes.Status429TooManyRequests
                : StatusCodes.Status401Unauthorized;

            return this.RespondError(statusCode, result.Message ?? "Unable to start customer login.");
        }

        return this.Respond(result.Message ?? "Login code sent.", result.Data!);
    }

    [HttpPost]
    [Route("[Controller]/VerifyLoginCode")]
    [AllowAnonymous]
    [EnableRateLimiting("customer-auth")]
    public async Task<IActionResult> VerifyLoginCodeAction([FromBody] VerifyCodeRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _customerRepository.VerifyCustomerLogin(request);
        if (!result.Success)
            return this.RespondError(
                StatusCodes.Status401Unauthorized,
                result.Message ?? result.Errors.FirstOrDefault() ?? "Login verification failed.");

        return this.Respond(result.Message ?? "Customer logged in successfully.", result.Data!);
    }

    [HttpGet]
    [Route("[Controller]/FetchCustomers")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<IActionResult> FetchCustomersAction()
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var customers = await _customerRepository.FetchCustomers();

        return this.Respond("Customers Fetched Successfully", customers);
    }

    [HttpGet]
    [Route("[Controller]/FetchCustomerProfile/{customerId:guid}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> FetchCustomerProfileAction([FromRoute] Guid customerId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!TryGetCustomerId(out var authenticatedCustomerId) || authenticatedCustomerId != customerId)
            return this.RespondError(StatusCodes.Status403Forbidden, "You cannot access another customer profile.");

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
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CheckCustomerSubAction([FromBody] CheckSubscription request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!TryGetCustomerId(out var customerId))
            return this.RespondError(StatusCodes.Status401Unauthorized, "Customer identity is missing.");

        request.CustomerId = customerId;
        var result = await _offerRepository.CheckSubscriptionStatus(request);

        if (!result.Success)
            return this.RespondError(
                StatusCodes.Status400BadRequest,
                result.Message ?? result.Errors.FirstOrDefault() ?? "Unable to check subscription status.");

        return this.Respond(result.Message ?? "Subscription status checked.", result.Data);
    }

    [HttpPost]
    [Route("[Controller]/VerifyAuthCode")]
    [AllowAnonymous]
    [EnableRateLimiting("customer-auth")]
    public async Task<IActionResult> VerifyAuthCodeTask([FromBody] VerifyCodeRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _customerRepository.VerifyCustomerAccount(request);

        if (!result.Success) return this.RespondError(
            StatusCodes.Status400BadRequest,
            result.Message ?? result.Errors.FirstOrDefault() ?? "Verification failed.");

        return this.Respond(result.Message ?? "Account verified successfully.", result.Data!);
    }

    [HttpPost]
    [Route("[Controller]/ResendAuthCode")]
    [AllowAnonymous]
    [EnableRateLimiting("customer-auth")]
    public async Task<IActionResult> ResendAuthCodeTask([FromBody] ResendVerificationRequest request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var result = await _customerRepository.ResendVerificationCode(request);
        if (!result.Success)
            return this.RespondError(StatusCodes.Status400BadRequest, result.Message ?? "Unable to resend verification code.");

        return this.Respond(result.Message!, result.Data!);
    }

    [HttpPost]
    [Route("[Controller]/CheckAccountCompletion/{customerId:guid}")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> AccountCompleteTask([FromRoute] Guid customerId)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        if (!TryGetCustomerId(out var authenticatedCustomerId) || authenticatedCustomerId != customerId)
            return this.RespondError(StatusCodes.Status403Forbidden, "You cannot access another customer account.");

        var request = await _customerRepository.AccountComplete(customerId);

        if (!request.Success) return this.RespondError(StatusCodes.Status400BadRequest, "Account not complete");

        return this.Respond("Request Success", request.Data);
    }

    [HttpPost]
    [Route("[Controller]/CompleteRegistration")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CompleteRegistrationTask([FromBody] CompleteRegistrationReq request)
    {
        if (!ModelState.IsValid) return this.RespondBadRequest(ModelState);

        var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(applicationUserId))
            return this.RespondError(StatusCodes.Status401Unauthorized, "Customer identity is missing.");

        var response = await _customerRepository.CompleteRegistration(applicationUserId, request);

        if (!response.Success) return this.RespondError(StatusCodes.Status400BadRequest, response.Message ?? "Customer registration failed.");

        return this.Respond("Registration Complete Successfully", response.Data!.Id);
    }

    private bool TryGetCustomerId(out Guid customerId)
    {
        return Guid.TryParse(User.FindFirstValue("customer_id"), out customerId);
    }
}
