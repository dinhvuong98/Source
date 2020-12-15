using Data.Entity.Common;
using Services.Dto.Shared;

namespace Services.Implementation.Common.Helpers
{
    public static class DictionaryHelper
    {
        public static DictionaryItemDto ToDictionaryItemDto(this MasterData entity)
        {
            if (entity == null) return null;

            DictionaryItemDto dto = new DictionaryItemDto
            {
                Key = entity.Key,
                Value = entity.Text,
                DisplayText = entity.Text,
                Code = entity.Value,
                TypeGroup = entity.GroupName
            };

            return dto;
        }
    }
}
