using Services.Dto.Shared;
using Services.Interfaces.Internal;
using Utilities.Common.Dependency;

namespace Services.Implementation.Common.Helpers
{
    public static class MasterDataHelper
    {
        public static DictionaryItemDto ToDictionaryItemDto<T>(this string value)
        {
            return value == null
                ? null
                : new DictionaryItemDto
                {
                    Key = value,
                    Code = value,
                    Value = SingletonDependency<IConfigValueManager>.Instance.GetOrNull<T>(value) ?? value,
                    DisplayText = SingletonDependency<IConfigValueManager>.Instance.GetOrNull<T>(value) ?? value
                };
        }
    }
}
