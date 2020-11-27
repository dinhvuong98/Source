using Services.Dto.Shared;
using System;

namespace Services.Dtos.Common
{
    public class NotificationDto
    {
        public Guid Id { get; set; }

        public DictionaryItemDto Type { get; set; }

        public ShortManagementFactorDto ManagementFactor { get; set; }

        public ShortFarmingLocationDto FarmingLocation { get; set; }

        public ShrimpCropDto ShrimpCrop { get; set; }

        public long ExecutionTime { get; set; }

        public long? FromDate { get; set; }

        public long? ToDate { get; set; }

        public string Status { get; set; }

        public DictionaryItemDto Frequency { get; set; }

        public DictionaryItemDto FactorGroup { get; set; }

        public long CreatedAt { get; set; }
    }
}
