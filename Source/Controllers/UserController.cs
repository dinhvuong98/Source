﻿using System;
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
    [Route("api/user")]
    [Authorize]
    public class UserController : BaseApiController
    {
        #region Properties
        private readonly IUserService _userService;
        private readonly ICacheProvider _redisCache;
        #endregion

        #region Constructor 
        public UserController(IUserService userService, ICacheProvider redisCache)
        {
            _redisCache = redisCache;
            _userService = userService;
        }

        #endregion

        #region Public methods
        [HttpGet("get-all")]
        public async Task<BaseResponse<UserDto[]>> GetAllUser()
        {
            var result = _redisCache.GetByKey<UserDto[]>(CacheConst.AllUser);

            var response = new BaseResponse<UserDto[]>
            {
                Status = false
            };

            if (result == null)
            {
                result = await _userService.GetAllUser();
                _redisCache.Set(CacheConst.AllUser, result, TimeSpan.FromHours(1));
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

        [HttpPatch("read-notification")]
        public async Task<BaseResponse<bool>> ReadNotification()
        {
            var response = new BaseResponse<bool>
            {
                Data = await _userService.ReadNotification(),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("filter/{page}/{pageSize}")]
        public async Task<BaseResponse<PageResultDto<DetailUserResultDto>>> FilterUser([FromRoute] PageDto pageDto, [FromQuery] string searchKey, [FromQuery] string groupId)
        {
            var response = new BaseResponse<PageResultDto<DetailUserResultDto>>
            {
                Data = await _userService.FilterUser(pageDto, searchKey, groupId),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpGet("{userId}")]
        public async Task<BaseResponse<DetailUserResultDto>> GetUserById([FromQuery] Guid userId)
        {
            if(userId == Guid.Empty)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<DetailUserResultDto>
            {
                Data = await _userService.GetDetailUserById(userId),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPost("create")]
        public async Task<BaseResponse<DetailUserResultDto>> CreateUser([FromBody] CreateOrUpdateUserDto dto)
        {
            if(dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<DetailUserResultDto>
            {
                Data = await _userService.CreateOrUpdateUser(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPut("update")]
        public async Task<BaseResponse<DetailUserResultDto>> UpdateUser([FromBody] CreateOrUpdateUserDto dto)
        {
            if (dto == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<DetailUserResultDto>
            {
                Data = await _userService.CreateOrUpdateUser(dto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpDelete("delete")]
        public async Task<BaseResponse<bool>> DeleteUser([FromBody] Guid[] ids)
        {
            if(ids == null)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var response = new BaseResponse<bool>
            {
                Data = await _userService.DeleteUser(ids),
                Status = true
            };

            return await Task.FromResult(response);
        }
        #endregion
    }
}
