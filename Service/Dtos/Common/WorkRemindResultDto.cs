using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class WorkRemindResultDto
    {
        public Guid Id { get; set; }

        public DateTime ExecutionTime { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public Guid Curator { get; set; }

        public Guid ManagementFactorId { get; set; }

        public Guid FarmingLocationId { get; set; }

        public Guid ShrimpCropId { get; set; }

        public Guid ShrimpCropManagementFactorId { get; set; }
    }
}
