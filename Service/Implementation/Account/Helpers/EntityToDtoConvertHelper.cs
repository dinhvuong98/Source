using Data.Entity.Account;
using Services.Dtos.Account;
using System.Linq;
using Utilities.Extensions;

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

            dto.UserName = entity.Account.FirstOrDefault().UserName;
            dto.Id = entity.Account.FirstOrDefault().Id;
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

            dto.UserName = entity.Account.FirstOrDefault().UserName;
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
    }
}
