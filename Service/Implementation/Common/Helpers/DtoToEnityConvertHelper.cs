using Data.Entity.Common;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System;
using Utilities.Extensions;
using Utilities.Helpers;

namespace Services.Implementation.Common.Helpers
{
    public static class DtoToEnityConvertHelper
    {
        public static ShrimpCrop ToShrimpCrop(this ShrimpCropDto dto)
        {
            if (dto == null) return null;

            ShrimpCrop entity = new ShrimpCrop();

            entity.CopyPropertiesFrom(dto);

            return entity;
        }

        public static ShrimpCrop ToShrimpCrop (this CreateShrimpCropDto dto)
        {
            if (dto == null) return null;

            ShrimpCrop entity = new ShrimpCrop();

            entity.CopyPropertiesFrom(dto);

            entity.Id = Guid.NewGuid();
            entity.FromDate = dto.FromDate.FromUnixTimeStamp();
            entity.ToDate = dto.ToDate.FromUnixTimeStamp();
            entity.CreatedAt = DateTime.UtcNow;
            entity.FarmingLocationId = dto.FarmingLocation.Id;
            entity.ShrimpBreedId =  dto.ShrimpBreed.Id;

            return entity;
        }

        public static ShrimpCrop ToShrimpCrop(this UpdateShrimpCropDto dto)
        {
            if (dto == null) return null;

            ShrimpCrop entity = new ShrimpCrop();

            entity.CopyPropertiesFrom(dto); 
            entity.FromDate = dto.FromDate.FromUnixTimeStamp();
            entity.ToDate = dto.ToDate.FromUnixTimeStamp();
            entity.FarmingLocationId = dto.FarmingLocation.Id;
            entity.ShrimpBreedId = dto.ShrimpBreed.Id;

            return entity;
        }

        public static ManagementFactor ToManagementFactor(this ManagementFactorDto dto)
        {
            if (dto == null) return null;

            ManagementFactor entity = new ManagementFactor();

            entity.CopyPropertiesFrom(dto);

            return entity;
        }

        public static ShrimpCropManagementFactor ToShrimpCropManagementFactor(this CreateShrimpCropManagementFactorDto dto)
        {
            if (dto == null) return null;

            ShrimpCropManagementFactor entity = new ShrimpCropManagementFactor();

            entity.ShrimpCropId = dto.ShrimpCropId;
            entity.Frequency = dto.Frequency.Code;
            entity.ManagementFactorId = dto.ManagementFactor.Id;
            entity.Curator = dto.Curator.Id;
            entity.Frequency = dto.Frequency.Code;
            entity.ExecutionTime = dto.ExecutionTime.FromUnixTimeStamp();
            entity.FromDate = dto.FromDate == null ? null : dto.FromDate.FromUnixTimeStamp();
            entity.ToDate = dto.ToDate == null ? null : dto.ToDate.FromUnixTimeStamp();

            return entity;
        }

        public static Notification ToNotification(this CreateNotificationDto dto)
        {
            if (dto == null) return null;

            Notification entity = new Notification();

            entity.Id = Guid.NewGuid();
            entity.ExecutionTime = dto.ExecutionTime.FromUnixTimeStamp();
            entity.FromDate = dto.FromDate?.FromUnixTimeStamp();
            entity.ToDate = dto.ToDate?.FromUnixTimeStamp();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UserId = dto.ShrimpCropManagementFactor.Curator.Id;
            entity.WorkId = dto.WorkId;
            entity.ManagementFactorId = dto.ManagementFactor.Id;
            entity.FarmingLocationId = dto.FarmingLocation.Id;
            entity.ShrimpCropId = dto.ShrimpCrop.Id;

            return entity;
        }
    }
}
