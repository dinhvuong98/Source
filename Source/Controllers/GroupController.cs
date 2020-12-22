using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Account;
using Services.Dtos.Account.InputDtos;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Response;
using Services.Interfaces.Account;
using Services.Interfaces.RedisCache;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Source.Controllers
{
    [Route("api/group")]
    [Authorize]
    public class GroupController : BaseApiController
    {
        #region Properties
        private readonly IUserService _userService;
        private readonly ICacheProvider _redisCache;
        #endregion

        #region Constructor 
        public GroupController(IUserService userService, ICacheProvider redisCache)
        {
            _redisCache = redisCache;
            _userService = userService;
        }

        #endregion

        #region Public methods

        [HttpGet("filter/{page}/{pageSize}")]
        public async Task<BaseResponse<PageResultDto<GroupDto>>> FilterGroup([FromRoute] PageDto pageDto, [FromQuery] string searchKey, [FromQuery] string groupId)
        {
            var response = new BaseResponse<PageResultDto<GroupDto>>
            {
                Data = await _userService.FilterGroup(pageDto, searchKey),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("get-all")]
        public async Task<BaseResponse<GroupDto[]>> GetAllGroup()
        {
            var result = _redisCache.GetByKey<GroupDto[]>(CacheConst.AllGroup);

            var response = new BaseResponse<GroupDto[]>
            {
                Status = false
            };

            if (result == null)
            {
                result = await _userService.GetAllGroup();
                _redisCache.Set(CacheConst.AllGroup, result, TimeSpan.FromHours(1));
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

        [HttpGet("{groupId}")]
        public async Task<BaseResponse<GroupDto>> GetGroupById([FromRoute] Guid groupId)
        {
            var response = new BaseResponse<GroupDto>
            {
                Data = await _userService.GetGroupById(groupId),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPost("create")]
        public async Task<BaseResponse<GroupDto>> CreateGroup([FromBody] CreateGroupDto dto)
        {
            if(dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<GroupDto>
            {
                Data = await _userService.CreateGroup(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        #endregion
    }
}
