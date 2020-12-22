using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Response;
using Services.Interfaces.Common;
using System.Threading.Tasks;

namespace Source.Controllers
{
    [Route("api/notify")]
    [Authorize]
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
        public async Task<BaseResponse<PageResultDto<NotificationDto>>> FilterNotification ([FromRoute] PageDto pageDto)
        {
            var response = new BaseResponse<PageResultDto<NotificationDto>>
            {
                Data = await _notificationService.FilterNotification(pageDto),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPatch("{id}/mark-as-read")]
        public async Task<BaseResponse<bool>> MarkAsReadNotification([FromRoute] string id)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _notificationService.MarkAsReadNotification(id),
                Status = true
            };

            return await Task.FromResult(response);
        }

        [HttpPatch("mark-all-as-read")]
        public async Task<BaseResponse<bool>> MarkAllAsReadNotification([FromBody] long Timestamp)
        {
            var response = new BaseResponse<bool>
            {
                Data = await _notificationService.MarkAllAsReadNotification(Timestamp),
                Status = true
            };

            return await Task.FromResult(response);
        }


        #endregion
    }
}
