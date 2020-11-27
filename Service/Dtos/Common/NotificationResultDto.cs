using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class NotificationResultDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public DateTime ExecutionTime { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Status { get; set; }

        public string Frequency { get; set; }

        public Guid FarmingLocationId { get; set; }

        public string FarmingLocationName { get; set; }

        public string FarmingLocationCode { get; set; }

        public Guid ManagementFactorId { get; set; }

        public string ManagementFactorName { get; set; }

        public string ManagementFactorCode { get; set; }

        public string FactorGroupName { get; set; }

        public Guid ShrimpCropId { get; set; }

        public string ShrimpCropName { get; set; }

        public string ShrimpCropCode { get; set; }

        public DateTime ShrimpCropFromDate { get; set; }

        public DateTime ShrimpCropToDate { get; set; }

        public Guid ShrimpBreedId { get; set; }

        public string ShrimpBreedName { get; set; }

        public string ShrimpBreedCode { get; set; }

        public string ShrimpBreedDescription { get; set; }

        public string ShrimpBreedAttachment { get; set; }
    }
}
