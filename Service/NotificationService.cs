using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Hangfire;
using SalvageCore.DTOs.Customer.Response;
using SalvageCore.DTOs.Default;
using SalvageCore.DTOs.Notification;
using SalvageCore.DTOs.Offer.Response;
using SalvageCore.Helpers;
using SalvageCore.Interface;
using SalvageCore.Models;
using Serilog;

namespace SalvageCore.Service;

public class NotificationService : INotificationService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly IRedisCacheService _redisCacheService;

    public NotificationService(IConfiguration configuration, HttpClient httpClient, IRedisCacheService redisCacheService)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _redisCacheService = redisCacheService;
    }

    public async Task SendWelcomeMessage(string? email, string name)
    {
        try
        {
            var template = new EmailTemplates().GetWelcomeMessage(email, name);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to send registration Email {@e}", exception.Message);
            throw;
        }
    }

    public async Task SendCredentialsEmail(string email, string password)
    {
        try
        {
        }
        catch (Exception exception)
        {
            Log.Error("Failed to send registration Email {@e}", exception.Message);
            throw;
        }
    }

    public async Task SendGetStartedMessage(string email, string name)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task<NotificationResponse> SendVerificationCode(VerificationReq request)
    {
        try
        {
            NotificationResponse notificationResponse;

            var endpoint = _configuration.GetValue<string>("SmsProvider:Endpoint");
            var secret = _configuration.GetValue<string>("SmsProvider:Secret");
            var sender = _configuration.GetValue<string>("SmsProvider:Sender");
            var random = NumberGenerator.RandomString(7);

            var payload = new NotificationReq
            {
                From = sender,
                To = request.Phone,
                Text = request.Message,
                Flash = 0,
                Reference = random
            };

            var jsonContent = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {secret}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.PostAsync($"{endpoint}/api/sms/v2/text/single", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Log.Information("Message Sent Successfully: {@body}", responseBody);

                var messageResponse = JsonSerializer.Deserialize<PinResponse>(responseBody);

                notificationResponse = new NotificationResponse
                {
                    Success = true
                };
            }
            else
            {
                Log.Error($"Message Error: {response.StatusCode} - {response.ReasonPhrase}");

                notificationResponse = new NotificationResponse
                {
                    Success = false
                };
            }

            await Task.CompletedTask;

            return notificationResponse;
        }
        catch (SocketException exception)
        {
            Log.Error("Socket Connection Error  => {@exception}", exception.Message);
            throw new Exception("Socket Connection Error", exception);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to send message {@exception}", exception);
            throw;
        }
    }

    public async Task<NotificationResponse> SendVerificationCodeEmail(string email, string code)
    {
        try
        {
            var template = new EmailTemplates().GetVerificationEmail(email, code);

            var response = new NotificationResponse
            {
                Success = true
            };

            return response;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to send email message {@exception}", exception);
            throw;
        }
    }

    public async Task<NotificationResponse> SendAwardNotification(NotificationPayload payload)
    {
        try
        {
            NotificationResponse notificationResponse;

            var requestUrl = _configuration.GetValue<string>("CustomSettings:SmsEndpoint");

            Log.Information("Payload sent to registered customer ${@payload}", payload);

            var resultJson = JsonSerializer.Serialize(payload);

            var content = new StringContent(resultJson, Encoding.UTF8, "application/json");

            var apikey = _configuration.GetValue<string>("CustomSettings:SmsApiKey");

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            request.Headers.Add("X-API-Key", apikey);

            var response = await _httpClient.SendAsync(request);

            Log.Information("Response Status Code {statusCode} with {message}", response.StatusCode, response.ReasonPhrase);

            if (response.IsSuccessStatusCode)
            {
                notificationResponse = new NotificationResponse();

                Log.Information("Successfully Posted Result At {@date}", DateTime.Now);
            }
            else
            {
                notificationResponse = new NotificationResponse();
                Log.Error("Error Posting Results => {@status} with {@message}", response.StatusCode, response.ReasonPhrase);
            }

            await Task.CompletedTask;

            return notificationResponse;
        }
        catch (SocketException exception)
        {
            Log.Error("Socket Connection Error  => {@exception}", exception.Message);
            throw new Exception("Socket Connection Error", exception);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to send verification code {@exception}", exception);
            throw;
        }
    }

    public async Task<NotificationResponse> SendBidNotifications(BulkOfferNotify payload)
    {
        try
        {
            Log.Information("Received notif sent ${@payload}", payload);

            NotificationResponse notificationResponse;

            var endpoint = _configuration.GetValue<string>("SmsProvider:Endpoint");
            var secret = _configuration.GetValue<string>("SmsProvider:Secret");
            var sender = _configuration.GetValue<string>("SmsProvider:Sender");

            var jsonContent = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            Log.Information("Notification sent to registered customer ${@payload}", content);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {secret}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await _httpClient.PostAsync($"{endpoint}/api/sms/v2/text/multi", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Log.Information("Message Sent Successfully: {@body}", responseBody);

                var messageResponse = JsonSerializer.Deserialize<PinResponse>(responseBody);

                notificationResponse = new NotificationResponse
                {
                    Success = true
                };
            }
            else
            {
                Log.Error($"Message Error: {response.StatusCode} - {response.ReasonPhrase}");

                notificationResponse = new NotificationResponse
                {
                    Success = false
                };
            }

            await Task.CompletedTask;

            return notificationResponse;
        }
        catch (SocketException exception)
        {
            Log.Error("Socket Connection Error  => {@exception}", exception.Message);
            throw new Exception("Socket Connection Error", exception);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to send message {@exception}", exception);
            throw;
        }
    }

    public async Task<ServiceResponse<SmsResponse>> SendCredentialsMessage(string code, string phone, string email)
    {
        try
        {
            var numbers = new List<string> { phone };
            var message = $"Your Smart Salvage TZ Account has been created with username: {email} and password: {code}";

            var sender = _configuration.GetValue<string>("BeemProvider:SenderId");
            var endpoint = _configuration.GetValue<string>("BeemProvider:SmsEndpoint");
            var key = _configuration.GetValue<string>("BeemProvider:SmsKey");
            var secret = _configuration.GetValue<string>("BeemProvider:SmsSecret");

            var payload = new MessagePayload
            {
                SourceAddress = sender,
                ScheduledTime = "",
                Encoding = 0,
                Message = message,
                Recipients = numbers.Select((mobile, index) => new RecipientPayload
                {
                    Id = index + 1,
                    Address = mobile
                }).ToList()
            };

            var jsonContent = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key}:{secret}"));
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

            Log.Information("Content sent to registered user {@payload}", jsonContent);

            var response = await _httpClient.PostAsync($"{endpoint}/send", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            Log.Information("SMS Response Content {@content}", responseContent);

            Log.Information("Response Status Code {statusCode} with {message}", response.StatusCode, response.ReasonPhrase);

            if (!response.IsSuccessStatusCode) Log.Error("Failed to send Registration SMS {@code}", response.StatusCode);
            //return ServiceResponse.Failure<SmsResponse>("",response.ReasonPhrase); 
            var smsResponse = JsonSerializer.Deserialize<SmsResponse>(responseContent);

            return ServiceResponse.Success<SmsResponse>(smsResponse, "SMS Sent Successfully");
        }
        catch (SocketException exception)
        {
            Log.Error("Socket Connection Error  => {@exception}", exception.Message);
            throw new Exception("Socket Connection Error", exception);
        }
        catch (Exception exception)
        {
            Log.Error("Failed to send verification code {@exception}", exception);
            throw;
        }
    }
}