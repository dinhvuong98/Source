using AutoMapper;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.AutoMapper;
using Services.Implementation;
using Services.Implementation.Account;
using Services.Implementation.Common;
using Services.Implementation.Internal;
using Services.Implementation.RedisCache;
using Services.Interfaces;
using Services.Interfaces.Account;
using Services.Interfaces.Common;
using Services.Interfaces.Internal;
using Services.Interfaces.RedisCache;
using System;
using static Services.Implementation.Internal.CronJobService;

namespace Source.App_Start
{
    public class ApplicationServicesInstaller
    {
        public static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<DatabaseConnectService>();
            services.AddTransient<IConfigValueManager, ConfigValueManager>();
            services.AddTransient<IBaseService, BaseService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMasterDataService, MasterDataService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<ICacheProvider, RedisCacheProvider>();
            services.AddTransient<IWorkService, WorkService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IShrimpCropService, ShrimpCropService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IManagementFactorService, ManagementFactorService>();

            Mapper.Initialize(cfg => cfg.AddProfile<DtoMappingProfile>());
        }
    }
}
