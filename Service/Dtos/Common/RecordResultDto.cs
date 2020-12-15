using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class RecordResultDto
    {
        public long ModifiedAt { get; set; }

        public bool result { get; set; }
    }
}
