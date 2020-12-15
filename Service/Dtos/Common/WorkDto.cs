using Services.Dtos.Account;
using Services.Dtos.Temp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Common
{
    [Serializable]
    public class WorkDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public long ExecutionTime { get; set; }

        public string Value { get; set; }

        public TempUploadFileDto[] Pictures { get; set; }

        public ShortFarmingLocationDto  FarmingLocation { get; set; }

        public ShrimpBreedDto ShrimpBreed { get; set; }

        public UserDto Curator { get; set; }

        public ShrimpCropDto ShrimpCrop { get; set; }

        public ShortManagementFactorDto ManagementFactor { get; set; }

        public MeasureUnitDto MeasureUnit { get; set; }

        public long ModifiedAt { get; set; }

        public string SampleValue { get; set; }

        public string Description { get; set; }

        public bool MustNotEdit { get; set; }

        public int age { get; set; }
    }
}
