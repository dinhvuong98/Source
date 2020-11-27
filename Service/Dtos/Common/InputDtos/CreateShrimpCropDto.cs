using System;

namespace Services.Dtos.Common.InputDtos
{
    [Serializable]
    public class CreateShrimpCropDto
    {
        public string Name { get; set; }

        public long FromDate { get; set; }

        public long ToDate { get; set; }

        public ShortFarmingLocationDto FarmingLocation { get; set; }

        public ShrimpBreedDto ShrimpBreed { get; set; }
    }
}
