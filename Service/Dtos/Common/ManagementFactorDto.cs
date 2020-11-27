using Services.Dto.Shared;
using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class ManagementFactorDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DictionaryItemDto DataType { get; set; }

        public string SampleValue { get; set; }
        
        public string Description { get; set; }

        public DictionaryItemDto FactorGroup { get; set; }

        public MeasureUnitDto Unit { get; set; }

        public long? ModifiedAt { get; set; }
    }
}
