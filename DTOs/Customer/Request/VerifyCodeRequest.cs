namespace SalvageCore.DTOs.Customer.Request;

public class VerifyCodeRequest
{
    public string Phone { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}