using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Common
{
    public class WorkResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime ExecutionTime { get; set; }

        public string Value { get; set; }

        public string Pictures { get; set; }

        public Guid FarmingLocationId { get; set; }

        public string FarmingLocationName { get; set; }

        public string FarmingLocationCode { get; set; }

        public Guid ShrimpBreedId { get; set; }

        public string ShrimpBreedName { get; set; }

        public string ShrimpBreedCode { get; set; }

        public string ShrimpBreedDescription { get; set; }

        public string ShrimpBreedAttachment { get; set; }

        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public Guid ShrimpCropId { get; set; }

        public string ShrimpCropName { get; set; }

        public string ShrimpCropCode { get; set; }

        public DateTime ShrimpCropFromDate { get; set; }

        public DateTime ShrimpCropToDate { get; set; }

        public Guid ManagementFactorId { get; set; }

        public string ManagementFactorName { get; set; }

        public string ManagementFactorCode { get; set; }

        public Guid MeasureUnitId { get; set; }

        public string MeasureUnitName { get; set; }

        public string MeasureUnitDesciption { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string SampleValue { get; set; }

        public string Description { get; set; }

        public bool MustNotEdit { get; set; }

        public int age { get; set; }
    }
}
