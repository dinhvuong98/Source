using System;
namespace Services.Dtos.Account.InputDtos
{
    [Serializable]
    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string DeviceType { get; set; }

        public string Token { get; set; }
    }
}
