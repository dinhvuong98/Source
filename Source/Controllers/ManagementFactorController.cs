using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Interfaces.Common;
using Services.Dtos.Response;
using Services.Interfaces.RedisCache;
using Utilities.Constants;
using System;
using Utilities.Enums;

namespace Source.Controllers
{
    [Route("api/management-factor")]
    [Authorize]
    public class ManagementFactorController : BaseApiController
    {
        #region Properties
        private readonly IManagementFactorService _managementFactorService;
        private readonly ICacheProvider _redisCache;
        #endregion

        #region Constructor
        public ManagementFactorController(IManagementFactorService managementFactorService, ICacheProvider cacheProvider)
        {
            _managementFactorService = managementFactorService;
            _redisCache = cacheProvider;
        }
        #endregion

        #region Public methods

        [HttpGet("get-all")]
        public async Task<BaseResponse<ShortManagementFactorDto[]>> GetAllManagementFactor()
        {
            var result = _redisCache.GetByKey<ShortManagementFactorDto[]>(CacheConst.AllManagementFactor);

            var response = new BaseResponse<ShortManagementFactorDto[]>
            {
                Status = false
            };

            if (result == null)
            {
                result = await _managementFactorService.GetAllManagementFactor();
                _redisCache.Set(CacheConst.AllManagementFactor, result, TimeSpan.FromHours(1));
            }

            if (result.Length > 0)
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
        public async Task<BaseResponse<PageResultDto<ManagementFactorDto>>> FilterManagementFactor([FromRoute] PageDto pageDto, [FromQuery] string searchKey, [FromQuery] string dataType, [FromQuery] string factorGroup)
        {
            var response = new BaseResponse<PageResultDto<ManagementFactorDto>>
            {
                Data = await _managementFactorService.FilterManagementFactor(pageDto, searchKey, dataType, factorGroup),
                Status = true
            };

            return await Task.FromResult(response);
        }

        #endregion
    }
}
