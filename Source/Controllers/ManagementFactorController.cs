using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.WebAPI.Controllers;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Interfaces.Common;
using Services.Dtos.Response;

namespace Aquaculture.Controllers
{
    [Route("api/management-factor")]
    [Authorize]
    public class ManagementFactorController : BaseApiController
    {
        #region Properties
        private readonly IDataService _dataService;
        #endregion

        #region Constructor
        public ManagementFactorController(IDataService dataService)
        {
            _dataService = dataService;
        }
        #endregion

        #region Public methods

        [HttpGet("get-all")]
        public async Task<BaseResponse<ShortManagementFactorDto[]>> GetAllShrimpBreed()
        {
            var response = new BaseResponse<ShortManagementFactorDto[]>
            {
                Data = await _dataService.GetAllManagementFactor(),
                Status = true
            };

            return await Task.FromResult(response);
        }
            
        [HttpGet("filter/{page}/{pageSize}")]
        public async Task<BaseResponse<ItemResultDto<ManagementFactorDto>>> GetManagementFactor([FromRoute] PageDto pageDto, [FromQuery] string searchKey, [FromQuery] string dataType, [FromQuery] string factorGroup)
        {
            var response = new BaseResponse<ItemResultDto<ManagementFactorDto>>
            {
                Data = await _dataService.GetManagementFactor(pageDto, searchKey, dataType, factorGroup),
                Status = true
            };

            return await Task.FromResult(response);
        }

        #endregion
    }
}
