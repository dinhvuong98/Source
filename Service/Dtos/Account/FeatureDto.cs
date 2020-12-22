using System;

namespace Services.Dtos.Account
{
    [Serializable]
    public class FeatureDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
