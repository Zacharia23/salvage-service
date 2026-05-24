using SalvageCore.DTOs.Default;
using SalvageCore.DTOs.Notification;
using SalvageCore.DTOs.Offer.Response;
using SalvageCore.Helpers;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface INotificationService
{
    Task SendWelcomeMessage(string? email, string name);
    Task SendCredentialsEmail(string email, string password);
    Task SendGetStartedMessage(string email, string name);
    Task<NotificationResponse> SendVerificationCode(VerificationReq request);
    Task<NotificationResponse> SendVerificationCodeEmail(string email, string code);
    Task<NotificationResponse> SendAwardNotification(NotificationPayload payload);
    Task<NotificationResponse> SendBidNotifications(BulkOfferNotify payload);
    Task<ServiceResponse<SmsResponse>> SendCredentialsMessage(string passcode, string phone, string email);
}