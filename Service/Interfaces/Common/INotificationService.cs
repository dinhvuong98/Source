using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Common
{
    public interface INotificationService
    {
        Task<ItemResultDto<NotificationDto>> GetNotification(PageDto pageDto);
    }
}
