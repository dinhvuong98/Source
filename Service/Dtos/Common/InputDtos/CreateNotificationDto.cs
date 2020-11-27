using Services.Dto.Shared;
using System;

namespace Services.Dtos.Common.InputDtos
{
    [Serializable]
    public class CreateNotificationDto
    {
        public long ExecutionTime { get; set; }

        public long? FromDate { get; set; }

        public long? ToDate { get; set; }

        public long CreatedAt { get; set; }

        public Guid UserId { get; set; }

        public DictionaryItemDto Frequency { get; set; }

        public Guid WorkId { get; set; }

        public ShortFarmingLocationDto FarmingLocation { get; set; }

        public ShortManagementFactorDto ManagementFactor { get; set; }

        public ShrimpCropDto ShrimpCrop { get; set; }

        public ShrimpCropManagementFactorDto ShrimpCropManagementFactor { get; set; }
    }
}
