using Services.Dtos.Temp;
using System;

namespace Services.Dtos.Common.InputDtos
{
    [Serializable]
    public class UpdateWorkPictureDto
    {
        public Guid WorkId { get; set; }

        public TempUploadFileDto[] Pictures { get; set; }
    }
}
