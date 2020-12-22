using System;

namespace Services.Dtos.Account.InputDtos
{
    [Serializable]
    public class ChangePassDto
    {
        public Guid AccountId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
