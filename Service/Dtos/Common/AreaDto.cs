using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class AreaDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public decimal? Area { get; set; }

        public string Address { get; set; }
    }
}
