using System.Text;
using System.Text.Json;
using SalvageCore.DTOs.Customer.Request;
using SalvageCore.Interface;
using Serilog;

namespace SalvageCore.Service;

public class VerifyCodeService : IVerifyCodeService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public VerifyCodeService(IConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<bool> VerifyCode(string pinId, string code)
    {
        try
        {
            Log.Information("Code Verification: {@pinId} and {code}", pinId, code);

            var endpoint = _configuration.GetValue<string>("BeemProvider:OtpEndpoint");
            var key = _configuration.GetValue<string>("BeemProvider:Key");
            var secret = _configuration.GetValue<string>("BeemProvider:Secret");

            var payload = new VerifyPayload
            {
                PinId = pinId,
                Code = code
            };

            var jsonContent = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{key}:{secret}"));
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

            var response = await _httpClient.PostAsync($"{endpoint}/verify", content);

            if (!response.IsSuccessStatusCode)
            {
                Log.Error($"Code Verification Error: {response.StatusCode} - {response.ReasonPhrase}");
                return false;
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            Log.Information("Code Verified Successfully: {@body}", responseBody);

            return true;
        }
        catch (Exception exception)
        {
            Log.Error("Failed to verify auth Code: {@exception}", exception);
            return false;
        }
    }
}