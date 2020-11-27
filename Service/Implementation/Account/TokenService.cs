using Microsoft.Extensions.Options;
using Services.Dtos.Account;
using Services.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Utilities.Security;

namespace Services.Implementation.Account
{
    public class TokenService : ITokenService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public TokenService(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateJwtToken(AccountDto account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Sid, account.UserId.ToString()),
                new Claim(ClaimTypes.Surname, account.FullName)
            };

            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.IssuedAt,
                DateTime.Now.AddHours(Convert.ToDouble(_jwtOptions.WebExpirationInHours)),
                _jwtOptions.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
