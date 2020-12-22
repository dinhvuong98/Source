using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Account
{
    [Serializable]
    public class GroupDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long CreatedAt { get; set; }

        public int CountUsers { get; set; }

        public bool? IsDefault { get; set; }

        public FeatureDto[] Features { get; set; }

        public UserDto[] Users { get; set; }
    }
}
