using Microsoft.Extensions.Logging;
using Services.Interfaces.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Implementation.Internal
{
    public class JobService : CronJobService
    {
        private readonly ILogger<JobService> _logger;
        private readonly INotificationService _notificationService;

        public JobService(IScheduleConfig<JobService> config, ILogger<JobService> logger, INotificationService notification) : base(config.CronExpression, config.TimeZoneInfo)
        {
            _notificationService = notification;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} job is working.");

            _notificationService.CreateRemind();

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
    }
}
