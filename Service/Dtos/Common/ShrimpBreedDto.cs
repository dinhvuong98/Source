using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class ShrimpBreedDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string Attachment { get; set; }
    }
}
