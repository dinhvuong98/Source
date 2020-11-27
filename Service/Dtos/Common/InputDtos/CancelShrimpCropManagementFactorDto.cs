using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Dtos.Common.InputDtos
{
    [Serializable]
    public class CancelShrimpCropManagementFactorDto
    {
        public Guid ShrimpCropManagementFactoId { get; set; }

        public long? ModifiedAt { get; set; }
    }
}
