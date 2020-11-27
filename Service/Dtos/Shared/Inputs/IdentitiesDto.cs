using System;

namespace Services.Dto.Shared.Inputs
{
    [Serializable]
    public class IdentitiesDto
    {
        public Guid[] Ids { get; set; }
    }
}
