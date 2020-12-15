using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Common.InputDtos
{
    [Serializable]
    public class WorkPictureDto
    {
        public Guid Id { get; set; }

        public string OrgFileName { get; set; }

        public string OrgFileExtension { get; set; }

        public string FileUrl { get; set; }

        public string Container { get; set; }
    }
}
