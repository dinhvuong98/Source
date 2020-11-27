using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Common
{
    [Serializable]
    public class ShortShrimpBreedDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}
