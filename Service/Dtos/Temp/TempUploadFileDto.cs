using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Temp
{
    [Serializable]
    public class TempUploadFileDto
    {
        public Guid Id { get; set; }

        public string OrgFileName { get; set; }

        public string OrgFileExtension { get; set; }

        public string FileUrl { get; set; }

        public string Container { get; set; }
    }
}
