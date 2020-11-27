using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.WebAPI.Controllers;
using Services.Dtos.Account;
using Services.Dtos.Response;
using Services.Interfaces.Account;

namespace Aquaculture.Controllers
{
    [Route("api/user")]
    [Authorize]
    public class UserController : BaseApiController
    {
        #region Properties
        private readonly IUserService _userService;
        #endregion

        #region Constructor 
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Public methods
        [HttpGet("get-all")]
        public async Task<BaseResponse<UserDto[]>> GetAllUser()
        {
            var response = new BaseResponse<UserDto[]>
            {
                Data = await _userService.GetAllUser(),
                Status = true
            };

            return await Task.FromResult(response);
        }
        #endregion


    }
}
