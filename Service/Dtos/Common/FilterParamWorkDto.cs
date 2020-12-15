using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class FilterParamWorkDto
    {
        public long? FromDate { get; set; }

        public long? ToDate { get; set; }

        public string FarmingLocationId { get; set; }

        public string ShrimpCropId { get; set; }

        public string FactorGroup { get; set; }

        public string Curator { get; set; }

        public string Status { get; set; }
    }
}
