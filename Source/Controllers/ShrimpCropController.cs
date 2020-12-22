using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Response;
using Services.Implementation.Common;
using Services.Interfaces.Common;
using Services.Interfaces.RedisCache;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Source.Controllers
{
    [Route("api/shrimp-crop")]
    [Authorize]
    public class ShrimpCropController : BaseApiController
    {
        #region Properties
        private readonly IShrimpCropService _shrimpCropService;
        private readonly ICacheProvider _redisCache;
        #endregion

        #region Constructor
        public ShrimpCropController(IShrimpCropService shrimpCropService, ICacheProvider cache)
        {
            _redisCache = cache;
            _shrimpCropService = shrimpCropService;
        }
        #endregion

        #region Public methods

        [HttpGet("get-all")]
        public async Task<BaseResponse<ShrimpCropDto[]>> GetAllShrimpCrop()
        {
            var result = _redisCache.GetByKey<ShrimpCropDto[]>(CacheConst.AllShrimpCrop);

            var response = new BaseResponse<ShrimpCropDto[]>
            {
                Status = false
            };

            if (result == null)
            {
                result = await _shrimpCropService.GetAllShrimpCrop();
                _redisCache.Set(CacheConst.AllShrimpCrop, result, TimeSpan.FromHours(1));
            }

            if(result.Length > 0)
            {
                response.Data = result;
                response.Status = true;
            }
            else
            {
                response.Error = new Error("Get data not success!", ErrorCode.GET_DATA_NOT_SUCCESS);
            }

            return await Task.FromResult(response);
        }

        [HttpGet("filter/{page}/{pageSize}")]
        public async Task<BaseResponse<PageResultDto<ShrimpCropDto>>> FilterShrimpCrop([FromRoute] PageDto pageDto, [FromQuery] string searchKey, [FromQuery] string farmingLocationId, [FromQuery] string shrimpBreedId)
        {
            var response = new BaseResponse<PageResultDto<ShrimpCropDto>>
            {
                Data = await _shrimpCropService.FilterShrimpCrop(pageDto, searchKey, farmingLocationId, shrimpBreedId),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPost("create")]
        public async Task<BaseResponse<ShrimpCropResultDto>> Create([FromBody] CreateShrimpCropDto dto)
        {
            if (dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<ShrimpCropResultDto>
            {
                Data = await _shrimpCropService.CreateShrimpCrop(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse<ShrimpCropResultDto>> GetShrimpCropById([FromRoute] Guid id)
        {
            if (id == null)
            {
                throw new BusinessException("Invalid parameter", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<ShrimpCropResultDto>
            {
                Data = await _shrimpCropService.GetShrimpCropById(id),
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
                Data = await _shrimpCropService.CreateOrUpdateShrimpCropManagementFactor(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPost("management-factor/cancel")]
        public async Task<BaseResponse<bool>> CancelShrimpCropManagementFactor([FromBody] CancelShrimpCropManagementFactorDto dto)
        {
            if (dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<bool>
            {
                Data = await _shrimpCropService.CancelShrimpCropManagementFactor(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }
        #endregion
    }
}
