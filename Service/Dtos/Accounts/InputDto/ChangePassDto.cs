using System;

namespace Services.Dtos.Accounts.InputDto
{
    [Serializable]
    public class ChangePassDto
    {
        public Guid AccountId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
