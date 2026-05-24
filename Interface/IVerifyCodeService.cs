namespace SalvageCore.Interface;

public interface IVerifyCodeService
{
    public Task<bool> VerifyCode(string pinId, string code);
}