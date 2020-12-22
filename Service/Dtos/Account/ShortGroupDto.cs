using System;

namespace Services.Dtos.Account
{
    [Serializable]
    public class ShortGroupDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string GroupCode { get; set; }

        public string Description { get; set; }
    }
}
