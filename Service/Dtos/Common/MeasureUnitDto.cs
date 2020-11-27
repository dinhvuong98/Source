using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class MeasureUnitDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Desciption { get; set; }
    }
}
