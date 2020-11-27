using System;

namespace Services.Dto.Shared
{
    [Serializable]
    public class ImageUrlDto
    {
        public Guid Guid { get; set; }

        public string ThumbSizeUrl { get; set; }

        public string LargeSizeUrl { get; set; }
    }
}
