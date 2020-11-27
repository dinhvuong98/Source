using Data.Entity.Common;
using Services.Dto.Shared;
using Services.Dtos.Common;
using Services.Implementation.Account.Helpers;
using System;
using System.Linq;
using Utilities.Enums;
using Utilities.Extensions;
using Utilities.Helpers;

namespace Services.Implementation.Common.Helpers
{
    public static class  EntityToDtoConvertHelper
    {
        public static MeasureUnitDto ToMeasureUnitDto(this MeasureUnit entity)
        {
            if (entity == null)
            {
                return null;
            }

            MeasureUnitDto dto = new MeasureUnitDto();

            dto.CopyPropertiesFrom(entity);

            return dto;

        }

        public static AreaDto ToAreaDto(this Areas entity)
        {
            if (entity == null)
            {
                return null;
            }

            AreaDto dto = new AreaDto();

            dto.CopyPropertiesFrom(entity);

            return dto;
        }

        public static FarmingLocationDto ToFarmingLocationDto(this FarmingLocation entity)
        {
            if (entity == null)
            {
                return null;
            }

            FarmingLocationDto dto = new FarmingLocationDto();

            dto.CopyPropertiesFrom(entity);
            dto.Areas = entity.Areas.ToAreaDto();
            dto.Type = entity.Type.ToDictionaryItemDto<LocationType>();

            return dto;

        }

        public static ShrimpBreedDto ToShrimpBreedDto(this ShrimpBreed entity)
        {
            if (entity == null)
            {
                return null;
            }

            ShrimpBreedDto dto = new ShrimpBreedDto();

            dto.CopyPropertiesFrom(entity);

            return dto;
        }

        public static ShortManagementFactorDto ToShortManagementFactorDto(this ManagementFactor entity)
        {
            if (entity == null)
            {
                return null;
            }

            ShortManagementFactorDto dto = new ShortManagementFactorDto();

            dto.CopyPropertiesFrom(entity);

            return dto;
        }

        public static ManagementFactorDto ToManagementFactorDto(this ManagementFactor entity, DictionaryItemDto factorGroup)
        {
            if (entity == null)
            {
                return null;
            }

            ManagementFactorDto dto = new ManagementFactorDto();
            dto.CopyPropertiesFrom(entity);

            dto.Unit = entity.MeasureUnit.ToMeasureUnitDto();

            dto.DataType = entity.DataType.ToDictionaryItemDto<FactorDataType>();

            dto.FactorGroup = factorGroup;

            return dto;
        }

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

        public static ShrimpCropDto ToShrimpCropDto(this ShrimpCrop entity)
        {
            if (entity == null) return null;

            ShrimpCropDto dto = new ShrimpCropDto();

            dto.CopyPropertiesFrom(entity);

            dto.ShrimpBreed = entity.ShrimpBreed.ToShrimpBreedDto();
            dto.FarmingLocation = entity.FarmingLocation.ToShortFarmingLocationDto();
            dto.FromDate = entity.FromDate.ToSecondsTimestamp();
            dto.ToDate = entity.ToDate.ToSecondsTimestamp();
            dto.HasManagementFactor = entity.ShrimpCropManagementFactors.Count > 0;

            return dto;
        }

        public static ShrimpCropManagementFactorDto ToShrimpCropManagementFactorDto(this ShrimpCropManagementFactor entity)
        {
            if (entity == null) return null;

            ShrimpCropManagementFactorDto dto = new ShrimpCropManagementFactorDto();
            dto.ManagementFactor = entity.ManagementFactor.ToShortManagementFactorDto();
            dto.Curator = entity.User.ToUserDto();
            dto.Frequency = entity.Frequency.ToDictionaryItemDto<ShrimpCropFrequency>();
            dto.Status = entity.Status.ToDictionaryItemDto<CropFactorStatus>();
            dto.CopyPropertiesFrom(entity);

            return dto;

        }

        public static ShrimpCropResultDto ToShrimpCropResultDto(this  ShrimpCrop entity)
        {
            if (entity == null) return null;

            ShrimpCropResultDto dto = new ShrimpCropResultDto();

            dto.CopyPropertiesFrom(entity);
            dto.FarmingLocation = entity.FarmingLocation.ToShortFarmingLocationDto();
            dto.ShrimpBreed = entity.ShrimpBreed.ToShrimpBreedDto();
            dto.FromDate = entity.FromDate.ToSecondsTimestamp();
            dto.ToDate = entity.ToDate.ToSecondsTimestamp();

            dto.Factor = entity.ShrimpCropManagementFactors == null ? null : entity.ShrimpCropManagementFactors.Select(x => x.ToShrimpCropManagementFactorDto()).ToArray();

            return dto;
        }

        public static ShortFarmingLocationDto ToShortFarmingLocationDto(this FarmingLocation entity)
        {
            if (entity == null) return null;

            ShortFarmingLocationDto dto = new ShortFarmingLocationDto();

            dto.CopyPropertiesFrom(entity);
           
            return dto;
        }

        public static ShortShrimpBreedDto ToShortShrimpBreedDto(this ShrimpBreed entity)
        {
            if (entity == null) return null;

            ShortShrimpBreedDto dto = new ShortShrimpBreedDto();

            dto.CopyPropertiesFrom(entity);

            return dto;
        }

        public static NotificationDto ToNotificationDto(this Notification entity)
        {
            if (entity == null) return null;

            NotificationDto dto = new NotificationDto();

            dto.Id = entity.Id;
            dto.Type = entity.Type.ToDictionaryItemDto<NotifyType>();
            dto.ManagementFactor = entity.ManagementFactor.ToShortManagementFactorDto();
            dto.FarmingLocation = entity.FarmingLocation.ToShortFarmingLocationDto();
            dto.ShrimpCrop = entity.ShrimpCrop.ToShrimpCropDto();
            dto.ExecutionTime = entity.ExecutionTime.ToSecondsTimestamp();
            dto.FromDate = entity.FromDate?.ToSecondsTimestamp();
            dto.ToDate = entity.ToDate?.ToSecondsTimestamp();
            dto.Frequency = entity.Frequency.ToDictionaryItemDto<ShrimpCropFrequency>();
            dto.Status = entity.Status;
            dto.CreatedAt = entity.CreatedAt.ToSecondsTimestamp();

            return dto;
        }

        public static NotificationDto ToNotificationDto(this NotificationResultDto entity, DictionaryItemDto factorGroups)
        {
            if (entity == null) return null;

            ShortFarmingLocationDto farmingLocation = new ShortFarmingLocationDto
            {
                Id = entity.FarmingLocationId,
                Name = entity.FarmingLocationName,
                Code = entity.FarmingLocationCode
            };

            NotificationDto dto = new NotificationDto()
            {
                Id = entity.Id,
                FarmingLocation = farmingLocation,

                ManagementFactor = new ShortManagementFactorDto
                {
                    Id = entity.ManagementFactorId,
                    Name = entity.ManagementFactorName,
                    Code = entity.ManagementFactorCode
                },

                ShrimpCrop = new ShrimpCropDto
                {
                    Id = entity.ShrimpCropId,
                    Name = entity.ShrimpCropName,
                    Code = entity.ShrimpCropCode,
                    FromDate = entity.ShrimpCropFromDate.ToSecondsTimestamp(),
                    ToDate = entity.ShrimpCropToDate.ToSecondsTimestamp(),
                    FarmingLocation = farmingLocation,
                    ShrimpBreed = new ShrimpBreedDto
                    {
                        Id = entity.ShrimpBreedId,
                        Name = entity.ShrimpBreedName,
                        Code = entity.ShrimpBreedCode,
                        Description = entity.ShrimpBreedDescription,
                        Attachment = entity.ShrimpBreedAttachment
                    }
                },

                ExecutionTime = entity.ExecutionTime.ToSecondsTimestamp(),
                FromDate = entity.FromDate.ToSecondsTimestamp(),
                ToDate = entity.ToDate.ToSecondsTimestamp(),
                Status = entity.Status,
                CreatedAt = DateTime.UtcNow.ToSecondsTimestamp(),
                Type = entity.Type.ToDictionaryItemDto<NotifyType>(),
                Frequency = entity.Frequency.ToDictionaryItemDto<ShrimpCropFrequency>(),
                FactorGroup = factorGroups
            };

            return dto;
        }
    }
}
