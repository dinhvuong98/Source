using Data.Entity.Account;
using Services.Dtos.Account;
using System.Linq;
using Utilities.Enums;
using Utilities.Extensions;
using Utilities.Helpers;

namespace Services.Implementation.Account.Helpers
{
    public static class EntityToDtoConvertHelper
    {
        public static AccountDto ToAccountDto(this User entity)
        {
            if (entity == null)
            {
                return null;
            }

            AccountDto dto = new AccountDto();

            dto.CopyPropertiesFrom(entity);

            dto.UserName = entity.Accounts.FirstOrDefault().UserName;
            dto.Id = entity.Accounts.FirstOrDefault().Id;
            dto.UserId = entity.Id;

            return dto;
        }

        public static LoginResultDto ToLoginResultDto(this User entity)
        {
            if(entity == null)
            {
                return null;
            }

            LoginResultDto dto = new LoginResultDto();

            dto.CopyPropertiesFrom(entity);

            dto.UserName = entity.Accounts.FirstOrDefault().UserName;
            dto.UserId = entity.Id;

            return dto;
        }

        public static UserDto ToUserDto(this User entity)
        {
            if (entity == null) return null;

            UserDto dto = new UserDto();

            dto.CopyPropertiesFrom(entity);

            return dto;
        }

        public static DetailUserResultDto ToDetailUserDto(this User entity)
        {
            if (entity == null) return null;

            DetailUserResultDto dto = new DetailUserResultDto();

            dto.CopyPropertiesFrom(entity);
            dto.UserName = entity.Accounts.FirstOrDefault().UserName;
            dto.ModifiedAt = entity.ModifiedAt.ToSecondsTimestamp();
            dto.LastTimeReadNotification = entity.LastTimeReadNotification.ToSecondsTimestamp();
            dto.Group = entity.UserGroups.Count < 1 ? null : entity.UserGroups.FirstOrDefault().Group.ToGroupDto();
            dto.IsActive = entity.Status == AccountStatus.Active.ToString();

            return dto;
        }

        public static GroupDto ToGroupDto(this Group entity)
        {
            if (entity == null) return null;

            GroupDto dto = new GroupDto();
            dto.CopyPropertiesFrom(entity);

            dto.CountUsers = entity.UserGroups.Where(x => x.Status == EntityStatus.Alive.ToString()).Count();
            dto.IsDefault = entity.isDefault;
            dto.CreatedAt = entity.CreatedAt.ToSecondsTimestamp();
            dto.Features = entity.GroupFeatures == null ? null : entity.GroupFeatures.Select(x => x.Feature.ToFeatureDto()).ToArray();

            return dto;
        }

        public static FeatureDto ToFeatureDto(this Feature entity)
        {
            if (entity == null) return null;

            FeatureDto dto = new FeatureDto();

            dto.CopyPropertiesFrom(entity);

            return dto;
        }

        public static ShortGroupDto ToShortGroupDto(this Group entity)
        {
            if (entity == null) return null;

            ShortGroupDto dto = new ShortGroupDto();

            dto.CopyPropertiesFrom(entity);

            return dto;
        }
    }
}
