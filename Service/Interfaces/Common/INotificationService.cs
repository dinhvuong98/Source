using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System.Threading.Tasks;

namespace Services.Interfaces.Common
{
    public interface INotificationService
    {
        Task<PageResultDto<NotificationDto>> FilterNotification(PageDto pageDto);

        Task<bool> MarkAsReadNotification(string id);

        Task<bool> MarkAllAsReadNotification(long timestamp);

        Task<bool> CreateRemind();
    }
}
