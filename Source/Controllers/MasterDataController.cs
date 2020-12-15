using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Response;
using Services.Interfaces.Common;
using Services.Interfaces.RedisCache;
using Utilities.Constants;
using Utilities.Enums;

namespace Source.Controllers
{
    [Route("api/data")]
    [Authorize]
    public class MasterDataController : BaseApiController
    {
        #region Properties
        private readonly IDataService _dataService;
        private readonly ICacheProvider _redisCache;
        #endregion

        #region Constructor
        public MasterDataController(IDataService dataService, ICacheProvider redisCache)
        {
            _dataService = dataService;
            _redisCache = redisCache;
        }
        #endregion

        #region public methods
        [HttpGet("filter-area/{page}/{pageSize}")]
        public async Task<BaseResponse<PageResultDto<AreaDto>>> FilterAreas([FromRoute] PageDto pageDto,[FromQuery] string searchKey)
        {
            var response = new BaseResponse<PageResultDto<AreaDto>>
            {
                Data = await _dataService.FilterAreas(pageDto, searchKey),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("filter-farming-location/{page}/{pageSize}")]
        public async Task<BaseResponse<PageResultDto<FarmingLocationDto>>> FilterFarmingLocation([FromRoute] PageDto pageDto, [FromQuery] string searchKey, [FromQuery] string locationType)
        {
            var response = new BaseResponse<PageResultDto<FarmingLocationDto>>
            {
                Data = await _dataService.FilterFarmingLocation(pageDto, searchKey, locationType),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("farming-location/get-all")]
        public async Task<BaseResponse<ShortFarmingLocationDto[]>> GetAllFarmingLocation([FromQuery] string locationType)
        {
            var result = _redisCache.GetByKey<ShortFarmingLocationDto[]>(CacheConst.AllFarmingLocation);

            var response = new BaseResponse<ShortFarmingLocationDto[]>
            {
                Status = false
            };

            if (result == null)
            {
                result = await _dataService.GetAllFarmingLocaiton(locationType);
                _redisCache.Set(CacheConst.AllFarmingLocation, result, TimeSpan.FromHours(1));
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

        [HttpGet("filter-shrimp-breed/{page}/{pageSize}")]
        public async Task<BaseResponse<PageResultDto<ShrimpBreedDto>>> FilterShrimpBreed([FromRoute] PageDto pageDto, [FromQuery] string searchKey)
        {
            var response = new BaseResponse<PageResultDto<ShrimpBreedDto>>
            {
                Data = await _dataService.FilterShrimpBreed(pageDto, searchKey),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("shrimp-breed/get-all")]
        public async Task<BaseResponse<ShortShrimpBreedDto[]>> GetAllShrimpBreed()
        {
            var response = new BaseResponse<ShortShrimpBreedDto[]>
            {
                Data = await _dataService.GetAllShrimpBreed(),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("masterdata")]
        public async Task<BaseResponse<MasterDataResultDto[]>> GetMasterData([FromQuery] string groupsName)
        {
            var result = _redisCache.GetByKey<MasterDataResultDto[]>(groupsName);

            var response = new BaseResponse<MasterDataResultDto[]>
            {
                Status = false
            };

            if(result == null)
            {
                result = await _dataService.GetMasterData(groupsName);
                _redisCache.Set(groupsName, result, TimeSpan.FromHours(1));
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

        #endregion
    }
}
