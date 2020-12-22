using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Account.InputDtos
{
    [Serializable]
    public class CheckAccountDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
