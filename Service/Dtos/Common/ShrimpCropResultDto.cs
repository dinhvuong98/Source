﻿using System;


namespace Services.Dtos.Common
{
    [Serializable]
    public class ShrimpCropResultDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long FromDate { get; set; }

        public long ToDate { get; set; }

        public ShortFarmingLocationDto FarmingLocation { get; set; }

        public ShrimpBreedDto ShrimpBreed { get; set; }

        public ShrimpCropManagementFactorDto[] Factors { get; set; }
    }
}
