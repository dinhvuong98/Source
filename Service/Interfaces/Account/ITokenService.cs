using Services.Dtos.Account;


namespace Services.Interfaces.Account
{
    public interface ITokenService
    {
        string GenerateJwtToken(AccountDto account);
    }
}
