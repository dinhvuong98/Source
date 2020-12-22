using Data.Entity.Account;
using Services.Dtos.Account;
using Services.Dtos.Account.InputDtos;
using System;
using Utilities.Extensions;

namespace Services.Implementation.Account.Helpers
{
    public static class DtoToEntityConvertHelper
    {
        public static Group ToGroup(this CreateGroupDto dto)
        {
            if (dto == null) return null;

            Group entity = new Group();

            entity.Id = Guid.NewGuid();

            entity.Name = dto.Name;

            entity.Description = dto.Description;

            return entity;
        }

        public static UserGroup ToUserGroup(this ShortGroupDto dto)
        {
            if (dto == null) return null;

            var entity = new UserGroup();

            entity.GroupId = dto.Id;

            return entity;
        }

        public static Feature ToFeature(this FeatureDto dto)
        {
            if (dto == null) return null;

            Feature entity = new Feature();

            entity.CopyPropertiesFrom(dto);

            return entity;
        }

        public static User ToUser(this CreateOrUpdateUserDto dto)
        {
            if (dto == null) return null;

            var entity = new User();

            entity.CopyPropertiesFrom(dto);

            entity.Id = (Guid)(dto.Id == null ? Guid.NewGuid() : dto.Id);

            entity.ProvinceId = dto.Province == null ? null : (int?) int.Parse(dto.Province.Key);

            entity.DistrictId = dto.District == null ? null : (int?) int.Parse(dto.District.Key);

            entity.CommuneId = dto.Commune == null ? null : (int?) int.Parse(dto.Commune.Key);

            return entity;
        }

        public static Data.Entity.Account.Account ToAccount (this CreateOrUpdateUserDto dto)
        {
            if (dto == null) return null;

            var entity = new Data.Entity.Account.Account();

            entity.CopyPropertiesFrom(dto);

            entity.Id = Guid.NewGuid();


            return entity;
        }
    }
}
