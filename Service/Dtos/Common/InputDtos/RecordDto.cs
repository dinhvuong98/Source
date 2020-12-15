using System;

namespace Services.Dtos.Common.InputDtos
{
    public class RecordDto
    {
        public Guid WorkId { get; set; }

        public string Value { get; set; }

        public long ModifiedAt { get; set; }
    }
}
