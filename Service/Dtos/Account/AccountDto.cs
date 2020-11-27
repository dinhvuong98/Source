using System;

namespace Services.Dtos.Account
{
    public class AccountDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public String UserName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string JwtToken { get; set; }

        public string Status { get; set; }
    }
}
