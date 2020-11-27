using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Utilities.Common.Dependency;
using Utilities.Configuation;
using Utilities.Configurations;

namespace Source.App_Start
{
    public class CoreServicesInstaller
    {
        public static void ConfigCoreService(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            services.Configure<RedisCacheSettings>(configuration.GetSection(nameof(RedisCacheSettings)));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMemoryCache();

            // Add cors
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options
                    .WithOrigins(SingletonDependency<IOptions<AppSettings>>.Instance.Value.FEDomain,
                                SingletonDependency<IOptions<AppSettings>>.Instance.Value.FEDomainLocal)
                    .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization)
                    .AllowAnyMethod()
                    .WithExposedHeaders("Content-Disposition"));
            });
            services.AddMvcCore();
            services.AddAuthentication();
        }
    }
}
