using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Account.InputDtos
{
    [Serializable]
    public class ChangeGroupDto
    {
        public Guid UserId { get; set; }

        public Guid GroupId { get; set; }
    }
}
