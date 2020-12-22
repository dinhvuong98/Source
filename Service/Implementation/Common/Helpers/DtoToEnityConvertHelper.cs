using Data.Entity.Common;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Dtos.Temp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities.Constants;
using Utilities.Enums;
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

            entity.Id = (Guid)(dto.Id == null ? Guid.Empty : dto.Id);

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

        public static Work ToWork(this ShrimpCropManagementFactor dto)
        {
            if (dto == null) return null;

            Work entity = new Work();

            entity.Id = Guid.NewGuid();

            entity.ShrimpCropManagementFactorId = dto.Id;

            entity.FarmingLocationId = dto.ShrimpCrop.FarmingLocationId;

            entity.ShrimpBreedId = dto.ShrimpCrop.ShrimpBreedId;

            entity.Curator = dto.Curator;

            entity.CreatedAt = dto.CreatedAt;

            entity.CreatedBy = dto.CreatedBy;

            return entity;
        }

        public static ShrimpCrop ToShrimpCrop (this ShrimpCropResultDto dto)
        {
            if (dto == null) return null;

            var entity = new ShrimpCrop();

            entity.CopyPropertiesFrom(dto);

            entity.FromDate = dto.FromDate.FromUnixTimeStamp();

            entity.ToDate = dto.ToDate.FromUnixTimeStamp();

            entity.FarmingLocationId = dto.FarmingLocation.Id;

            entity.ShrimpBreedId = dto.ShrimpBreed.Id;

            return entity;
        }

        public static WorkPicture ToWorkPicture(this TempUploadFileDto dto)
        {
            if (dto == null) return null;

            WorkPicture entity = new WorkPicture();

            entity.CopyPropertiesFrom(dto);
            entity.FileId = dto.Id;
            entity.OrgFileExtension = Path.GetExtension(dto.OrgFileName);

            return entity;
        }

        public static FileManager ToFileManager(this TempUploadFileDto dto)
        {
            if (dto == null) return null;

            FileManager entity = new FileManager();

            entity.CopyPropertiesFrom(dto);

            return entity;
        }

        public static Notification ToNotification(this ShrimpCropManagementFactor dto)
        {
            if (dto == null) return null;

            Notification entity = new Notification();

            entity.Id = Guid.NewGuid();
            entity.Type = NotifyType.Notify.ToString();
            entity.ExecutionTime = dto.ExecutionTime;
            entity.FromDate = dto.FromDate == null ? null : dto.FromDate;
            entity.ToDate = dto.ToDate == null ? null : dto.ToDate;
            entity.UserId = dto.Curator;
            entity.Frequency = dto.Frequency;
            entity.ManagementFactorId = dto.ManagementFactorId;
            entity.FarmingLocationId = dto.ShrimpCrop.FarmingLocationId;
            entity.ShrimpCropId = dto.ShrimpCropId;
            entity.ShrimpCropManagementFactorId = dto.Id;

            return entity;
        }

        public static Notification ToWorkReMind(this WorkRemindResultDto dto, DateTime now)
        {
            if (dto == null) return null;

            Notification entity = new Notification();

            entity.Id = Guid.NewGuid();

            entity.ExecutionTime = dto.ExecutionTime;
            entity.Type = NotifyType.Remind.ToString();
            entity.FromDate = dto.FromDate == null ? null : dto.FromDate;
            entity.ToDate = dto.ToDate == null ? null : dto.ToDate;
            entity.UserId = dto.Curator;
            entity.WorkId = dto.Id;
            entity.ManagementFactorId = dto.ManagementFactorId;
            entity.FarmingLocationId = dto.FarmingLocationId;
            entity.ShrimpCropId = dto.ShrimpCropId;
            entity.ShrimpCropManagementFactorId = dto.ShrimpCropManagementFactorId;
            entity.FactorName   = "TEST REMIND";
            entity.CreatedAt = now;

            return entity;
        }
    }
}