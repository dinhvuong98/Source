using System;

namespace Services.Dto.Shared
{
    [Serializable]
    public class SendMailDto
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public SendMailDto(string email, string name)
        {
            Email = email;
            Name = name;
        }
    }
}
