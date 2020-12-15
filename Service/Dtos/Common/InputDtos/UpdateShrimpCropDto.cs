using System;

namespace Services.Dtos.Common.InputDtos
{
    [Serializable]
    // ke thua tu create (create co the them 1 field id null)
    public class UpdateShrimpCropDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public long FromDate { get; set; }

        public long ToDate { get; set; }

        public ShortFarmingLocationDto FarmingLocation { get; set; }

        public ShrimpBreedDto ShrimpBreed { get; set; }
    }
}
