using System;

namespace Services.Dtos.Account.InputDtos
{
    [Serializable]
    public class CreateGroupDto
    {
        public string Name { get; set; }

        public string Description    { get; set; }

        public FeatureDto[] Features { get; set; }
    }
}
