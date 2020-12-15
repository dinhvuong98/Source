using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Interfaces.Common;
using Services.Dtos.Response;

namespace Source.Controllers
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
        public async Task<BaseResponse<PageResultDto<ManagementFactorDto>>> GetManagementFactor([FromRoute] PageDto pageDto, [FromQuery] string searchKey, [FromQuery] string dataType, [FromQuery] string factorGroup)
        {
            var response = new BaseResponse<PageResultDto<ManagementFactorDto>>
            {
                Data = await _dataService.FilterManagementFactor(pageDto, searchKey, dataType, factorGroup),
                Status = true
            };

            return await Task.FromResult(response);
        }

        #endregion
    }
}
