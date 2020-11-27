using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Account;
using Services.Dtos.Account.InputDtos;
using Services.Interfaces.Account;
using Quartz.Util;
using Mvc.WebAPI.Controllers;
using Services.Dtos.Response;

namespace Aquaculture.Controllers
{
    [Route("api/")]
    [Authorize]
    public class AccountController : BaseApiController
    {
        #region Properties
        private readonly IUserService _userService;
        #endregion

        #region Constructor 
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Public methods

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<BaseResponse<LoginResultDto>> Login([FromBody]LoginDto loginDto)
        {
            if (loginDto == null
               || loginDto.UserName.IsNullOrWhiteSpace()
               || loginDto.Password.IsNullOrWhiteSpace())
            {
                throw new Exception("Invalid parameter!");
            }
            var response = new BaseResponse<LoginResultDto>
            {
                Data = await _userService.Login(loginDto),
                Status = true
            };

            return await Task.FromResult(response);
        }
        #endregion
    }
}
