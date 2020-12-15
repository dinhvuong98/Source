using Services.Dto.Shared;
using Services.Dtos.Account;
using System;

namespace Services.Dtos.Common.InputDtos
{
    [Serializable]
    public class CreateShrimpCropManagementFactorDto
    {
        public Guid? Id { get; set; }

        public Guid  ShrimpCropId { get; set; }

        public bool IsCreateWork { get; set; }

        public ShortManagementFactorDto ManagementFactor { get; set; }

        public UserDto Curator { get; set; }

        public DictionaryItemDto Frequency { get; set; }

        public long? ExecutionTime { get; set; }

        public long? FromDate { get; set; }

        public long? ToDate { get; set; }

        public long ModifiedAt { get; set; }

    }
}
