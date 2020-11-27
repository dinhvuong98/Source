using System;

namespace Services.Dto.Shared
{
    [Serializable]
    public class FileDescription
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public string Guid { get; set; }

        public byte[] Data { get; set; }

        public string Desc { get; set; }
    }
}
