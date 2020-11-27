using System;

namespace Services.Dtos.Account
{
    [Serializable]
    public class LoginResultDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }

        public string JwtToken { get; set; }

        public string Avatar { get; set; }

        public string[] Roles { get; set; }
    }
}
