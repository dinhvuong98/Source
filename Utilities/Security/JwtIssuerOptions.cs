using Microsoft.IdentityModel.Tokens;
using System;

namespace Utilities.Security
{
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public DateTime NotBefore { get; set; }

        public DateTime Expiration => IssuedAt.Add(ValidFor);

        public DateTime IssuedAt { get; set; }

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromDays(30);

        public int WebExpirationInHours { get; set; }

        public int MobileExpirationInHours { get; set; }

        public int ErpExpirationInHours { get; set; }

        public string JwtKey { get; set; }

        public SigningCredentials SigningCredentials { get; set; }
    }
}
