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

        public static DictionaryItemDto ToDictionaryItemDto(this Country entity)
        {
            return entity == null
                ? null
                : new DictionaryItemDto
                {
                    Key = entity.Id.ToString(),
                    Code = entity.CountryCode,
                    Value = entity.Name
                };
        }

        public static DictionaryItemDto ToDictionaryItemDto(this Province entity)
        {
            return entity == null
                ? null
                : new DictionaryItemDto
                {
                    Key = entity.Id.ToString(),
                    Code = entity.ProvinceCode,
                    Value = entity.Name
                };
        }

        public static DictionaryItemDto ToDictionaryItemDto(this District entity)
        {
            return entity == null
                ? null
                : new DictionaryItemDto
                {
                    Key = entity.Id.ToString(),
                    Code = entity.DistrictCode,
                    Value = entity.Name
                };
        }

        public static DictionaryItemDto ToDictionaryItemDto(this Commune entity)
        {
            return entity == null
                ? null
                : new DictionaryItemDto
                {
                    Key = entity.Id.ToString(),
                    Code = entity.CommuneCode,
                    Value = entity.Name
                };
        }
    }
}
