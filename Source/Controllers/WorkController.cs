using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Response;
using Services.Interfaces.Common;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Source.Controllers
{
    [Route("api/work")]
    [Authorize]
    public class WorkController : BaseApiController
    {
        #region Properties
        private readonly IWorkService _workService;
        #endregion

        #region Constructor
        public WorkController(IWorkService workService)
        {
            _workService = workService;
        }
        #endregion

        #region Public methods

        [HttpGet("filter/{page}/{pageSize}")]
        public async Task<BaseResponse<PageResultDto<WorkDto>>> FilterWork([FromRoute] PageDto pageDto, [FromQuery] FilterParamWorkDto dto)
        {
            var response = new BaseResponse<PageResultDto<WorkDto>>
            {
                Data = await _workService.FilterWork(pageDto, dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpDelete("{shrimCropManagementFactorId}/stop")]
        public async Task<BaseResponse<bool>> StopWork([FromRoute] Guid shrimCropManagementFactorId)
        {
            if (shrimCropManagementFactorId == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<bool>
            {
                Data = await _workService.StopWork(shrimCropManagementFactorId),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPut("record")]
        public async Task<BaseResponse<RecordResultDto>> RecordWork([FromBody] RecordDto dto)
        {
            if (dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<RecordResultDto>
            {
                Data = await _workService.RecordWord(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPut("update-picture")]
        public async Task<BaseResponse<bool>> UpdatePicture([FromBody] UpdateWorkPictureDto dto)
        {
            if(dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<bool>
            {
                Data = await _workService.UpdatePicture(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }
        #endregion
    }
}
