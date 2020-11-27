using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.WebAPI.Controllers;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Response;
using Services.Interfaces.Common;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Aquaculture.Controllers
{
    [Route("api/shrimp-crop")]
    [Authorize]
    public class ShrimpCropController : BaseApiController
    {
        #region Properties
        private readonly IDataService _dataService;
        #endregion

        #region Constructor
        public ShrimpCropController(IDataService dataService)
        {
            _dataService = dataService;
        }
        #endregion

        #region Public methods

        [HttpGet("filter/{page}/{pageSize}")]
        public async Task<BaseResponse<ItemResultDto<ShrimpCropDto>>> GetShrimCrop([FromRoute] PageDto pageDto, [FromQuery] string searchKey, [FromQuery] string farmingLocationId, [FromQuery] string shrimpBreedId)
        {
            var response = new BaseResponse<ItemResultDto<ShrimpCropDto>>
            {
                Data = await _dataService.GetShrimpCrop(pageDto, searchKey, farmingLocationId, shrimpBreedId),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPost("create")]
        public async Task<BaseResponse<ShrimpCropResultDto>> Create([FromBody] CreateShrimpCropDto dto)
        {
            if(dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<ShrimpCropResultDto>
            {
                Data = await _dataService.CreateShrimpCrop(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<ShrimpCropResultDto>> GetShrimpCropById([FromRoute] Guid id)
        {
            if(id == null)
            {
                throw new BusinessException("Invalid parameter", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<ShrimpCropResultDto>
            {
                Data = await _dataService.GetShrimpCropById(id),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPut("update")]
        public async Task<BaseResponse<ShrimpCropResultDto>> Update([FromBody] UpdateShrimpCropDto dto)
        {
            if(dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<ShrimpCropResultDto>
            {
                Data = await _dataService.UpDateShrimpCrop(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPost("management-factor/create-or-update")]
        public async Task<BaseResponse<Guid>> CreateOrUpdateShrimpCropManagementFactor([FromBody] CreateShrimpCropManagementFactorDto dto)
        {
            if (dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<Guid>
            {
                Data = await _dataService.CreateOrUpdateShrimpCropManagementFactor(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPost("management-factor/cancel")]
        public async Task<IActionResult> CancelShrimpCropManagementFactor([FromBody] CancelShrimpCropManagementFactorDto dto)
        {
            if (dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<bool>
            {
                Data = await _dataService.CancelShrimpCropManagementFactor(dto),
                Status = true
            };

            return Ok(response);
        }

        #endregion
    }
}
