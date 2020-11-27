using System;
using Services.Dto.Shared;
using System.Threading.Tasks;
using Data;
using Services.Interfaces.Internal;
using Utilities.Helpers;
using Dapper.FastCrud;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Data.Entity.Common;
using Utilities.Constants;

namespace Services.Implementation.Internal
{
    public class ConfigValueManager : BaseService, IConfigValueManager
    {
        private const int CacheExpirationDays = 1;
        private readonly ILogger<ConfigValueManager> _logger;
        private readonly IMemoryCache _cache;

        public ConfigValueManager(DatabaseConnectService databaseConnectService, IMemoryCache cache, ILogger<ConfigValueManager> logger) : base(databaseConnectService)
        {
            _cache = cache;
            _logger = logger;
        }

        private async Task<MasterData[]> AllConfigValuesAsync()
        {
            _logger.LogInformation("Start method get all config value");
            var result = await _cache.GetOrCreateAsync(
                CacheConst.MasterData,
                async entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromDays(CacheExpirationDays);
                    return await GetConfigKeyGroupsFromDbAsync();
                });
            _logger.LogInformation("End of method get all config value");
            return result;
        }

        private async Task<MasterData[]> GetConfigKeyGroupsFromDbAsync()
        {
            _logger.LogInformation("Start method get config key group");
            var query = await DatabaseConnectService.Connection.FindAsync<MasterData>();
            _logger.LogInformation("Start method get config key group");
            return query.ToArray();
        }

        public async Task<DictionaryItemDto[]> ConfigKeysByGroupAsync(string groupName)
        {
            _logger.LogInformation("Start method config key by group");
            var query = await DatabaseConnectService.Connection.FindAsync<MasterData>(x => x.Where($"group_name=@groupName")
                   .WithParameters(new { groupName = groupName }));
            _logger.LogInformation("End of method config key by group");
            return query.Select(
                x => new DictionaryItemDto
                {
                    Key = x.Key,
                    Value = x.Text,
                })
                .ToArray();
        }

        public string GetOrNull(string configKey)
        {
            _logger.LogInformation("Start method get or null");
            var all = AsyncHelper.RunSync(AllConfigValuesAsync).FirstOrDefault(x => x.Key == configKey)?.Key;
            _logger.LogInformation("End of method get or null");
            return all;
        }

        public string GetOrNull<T>(string configKeyValue)
        {
            _logger.LogInformation("Start method get or null by key");
            var all = AsyncHelper.RunSync(AllConfigValuesAsync).FirstOrDefault(x => x.GroupName == typeof(T).Name && x.Value == configKeyValue)?.Text;
            _logger.LogInformation("End of method get or null by key");
            return all;
        }
    }
}
