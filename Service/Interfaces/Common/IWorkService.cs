using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Common
{
    public interface IWorkService
    {
        Task<PageResultDto<WorkDto>> FilterWork(PageDto pageDto, FilterParamWorkDto paramDto);

        Task<bool> StopWork(Guid shrimpCropManagementFactorId);

        Task<RecordResultDto> RecordWord(RecordDto dto);

        Task<bool> UpdatePicture(UpdateWorkPictureDto dto);
    }
}
