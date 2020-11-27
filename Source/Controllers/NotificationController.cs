using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc.WebAPI.Controllers;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Response;
using Services.Interfaces.Common;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/notify")]
    public class NotificationController : BaseApiController
    {
        #region Properties
        private readonly INotificationService _notificationService;
        #endregion

        #region Constructor
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        #endregion

        #region Public methods

        [HttpGet("filter/{page}/{pageSize}")]
        public async Task<BaseResponse<ItemResultDto<NotificationDto>>> GetNotification ([FromRoute] PageDto pageDto)
        {
            var response = new BaseResponse<ItemResultDto<NotificationDto>>
            {
                Data = await _notificationService.GetNotification(pageDto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        #endregion
    }
}
