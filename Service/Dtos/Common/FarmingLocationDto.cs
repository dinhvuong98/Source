using Services.Dto.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Common
{
    [Serializable]
    public class FarmingLocationDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public DictionaryItemDto Type { get; set; }

        public Decimal? Area { get; set; }

        public string Description { get; set; }

        public string Attachment { get; set; }

        public AreaDto Areas { get; set; }
    }
}
