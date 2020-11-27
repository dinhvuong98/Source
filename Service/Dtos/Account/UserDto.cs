using System;

namespace Services.Dtos.Account
{
    [Serializable]
    public class UserDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public string Address { get; set; }

        public long? UpdateDate { get; set; }
    }
}
