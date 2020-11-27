using Services.Dto.Shared;
using System.Threading.Tasks;

namespace Services.Interfaces.Internal
{
    public interface IConfigValueManager
    {
        Task<DictionaryItemDto[]> ConfigKeysByGroupAsync(string groupName);

        string GetOrNull(string configKey);

        string GetOrNull<T>(string configKeyValue);
    }
}
