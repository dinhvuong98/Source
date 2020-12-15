using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System.Threading.Tasks;

namespace Services.Interfaces.Common
{
    public interface INotificationService
    {
        Task<PageResultDto<NotificationDto>> FilterNotification(PageDto pageDto);

        Task<bool> MaskAsReadNotification(string id);

        Task<bool> CreateRemind();
    }
}
