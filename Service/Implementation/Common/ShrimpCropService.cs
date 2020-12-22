using Dapper;
using Dapper.FastCrud;
using Data;
using Data.Entity.Account;
using Data.Entity.Common;
using Microsoft.Extensions.Logging;
using Quartz.Util;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Implementation.Common.Helpers;
using Services.Interfaces.Common;
using Services.Interfaces.Internal;
using Services.Interfaces.RedisCache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Services.Implementation.Common
{
    public class ShrimpCropService : BaseService, IShrimpCropService
    {
        #region Properties
        private readonly ILogger<DataService> _logger;
        private readonly ISessionService _sessionService;
        private readonly ICacheProvider _redisCache;
        #endregion

        #region #Constructor
        public ShrimpCropService(
            DatabaseConnectService databaseConnectService,
            ILogger<DataService> logger,
            ISessionService sessionService,
            ICacheProvider cacheProvider) : base(databaseConnectService)
        {
            _logger = logger;
            _sessionService = sessionService;
            _redisCache = cacheProvider;
            DatabaseConnectService = databaseConnectService;
        }
        #endregion

        public async Task<ShrimpCropDto[]> GetAllShrimpCrop()
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCrop>(x => x
                        .Include<FarmingLocation>(join => join.LeftOuterJoin())
                        .Include<ShrimpBreed>(join => join.LeftOuterJoin())
                        .Include<ShrimpCropManagementFactor>(join => join.LeftOuterJoin())))
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(x => x.ToShrimpCropDto())
                        .ToArray();

            return result;
        }

        public async Task<ShrimpCropResultDto> CreateShrimpCrop(CreateShrimpCropDto dto)
        {
            _logger.LogInformation("Start method create shrimp crop");

            ValidateShrimpCrop(dto);

            ShrimpCrop shrimpCrop = dto.ToShrimpCrop();

            shrimpCrop.Code = await GenerateShrimpCropCode();

            shrimpCrop.CreatedBy = _sessionService.UserId;

            await this.DatabaseConnectService.Connection.InsertAsync(shrimpCrop);

            _redisCache.Remove(CacheConst.AllShrimpCrop);

            _logger.LogInformation("end method create shrimp crop");


            return await GetShrimpCropById(shrimpCrop.Id);
        }

        public async Task<ShrimpCropResultDto> GetShrimpCropById(Guid id)
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCrop>(x => x
                        .Include<FarmingLocation>(join => join.LeftOuterJoin())
                        .Include<ShrimpBreed>(join => join.LeftOuterJoin())
                        .Include<ShrimpCropManagementFactor>(join => join.LeftOuterJoin())
                        .Include<User>(join => join.LeftOuterJoin())
                        .Include<ManagementFactor>(join => join.LeftOuterJoin())
                        .Where($"bys_shrimp_crop.id = @Id")
                        .WithParameters(new { Id = id }))).Select(x => x.ToShrimpCropResultDto()).FirstOrDefault();

            return result == null ? throw new BusinessException("", ErrorCode.INTERNAL_SERVER_ERROR) : result;
        }

        public async Task<PageResultDto<ShrimpCropDto>> FilterShrimpCrop(PageDto pageDto, string searchKey, string farmingLocationId, string shrimpBreedId)
        {
            var query = new StringBuilder();
            query.Append("(@SearchKey is null or (bys_shrimp_crop.name LIKE @SearchKey OR bys_shrimp_crop.code LIKE @SearchKey))");
            query.Append(" AND (@FarmingLocationId is null or bys_shrimp_crop.farming_location_id LIKE @FarmingLocationId)");
            query.Append(" AND (@ShrimpBreedId is null or bys_shrimp_crop.shrimp_breed_id LIKE @ShrimpBreedId)");

            var param = new
            {
                FarmingLocationId = farmingLocationId.FormatSearch(),
                ShrimpBreedId = shrimpBreedId.FormatSearch(),
                SearchKey = searchKey.FormatSearch()
            };

            var count = await this.DatabaseConnectService.Connection.CountAsync<ShrimpCrop>(x => x
                        .Include<FarmingLocation>(join => join.LeftOuterJoin())
                        .Include<ShrimpBreed>(join => join.LeftOuterJoin())
                        .Where($"{query}")
                        .WithParameters(param));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCrop>(x => x
                        .Include<FarmingLocation>(join => join.LeftOuterJoin())
                        .Include<ShrimpBreed>(join => join.LeftOuterJoin())
                        .Include<ShrimpCropManagementFactor>(join => join.LeftOuterJoin())
                        .Where($"{query}")
                        .WithParameters(param)))
                        .OrderByDescending(x => x.CreatedAt)
                        .Skip(pageDto.Page * pageDto.PageSize)
                        .Take(pageDto.PageSize)
                        .Select(x => x.ToShrimpCropDto())
                        .ToArray();

            var result = new PageResultDto<ShrimpCropDto>
            {
                Items = items,
                TotalCount = count,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageDto.PageSize),
            };

            return result;
        }

        public async Task<Guid> CreateOrUpdateShrimpCropManagementFactor(CreateShrimpCropManagementFactorDto dto)
        {
            var shrimpCrop = await GetShrimpCropById(dto.ShrimpCropId);

            ValidateCreateShrimpCropManagementFactor(dto, shrimpCrop.FromDate, shrimpCrop.ToDate);

            DateTime now = DateTime.UtcNow;

            using (IDbTransaction transaction = this.DatabaseConnectService.BeginTransaction())
            {
                try
                {
                    ShrimpCropManagementFactor shrimpCropManagementFactor = dto.ToShrimpCropManagementFactor();
                    shrimpCropManagementFactor.CreatedBy = _sessionService.UserId;
                    shrimpCropManagementFactor.CreatedAt = now;

                    if (dto.IsCreateWork)
                    {
                        shrimpCropManagementFactor.Status = CropFactorStatus.HasWork.ToString();

                        if (dto.Id == null)
                        {
                            shrimpCropManagementFactor.Id = Guid.NewGuid();
                            await this.DatabaseConnectService.Connection.InsertAsync<ShrimpCropManagementFactor>(shrimpCropManagementFactor, x => x.AttachToTransaction(transaction));
                        }

                        await this.DatabaseConnectService.Connection.UpdateAsync<ShrimpCropManagementFactor>(shrimpCropManagementFactor, x => x.AttachToTransaction(transaction));

                        // create work
                        shrimpCropManagementFactor.ShrimpCrop = shrimpCrop.ToShrimpCrop();
                        await CreateWork(shrimpCropManagementFactor, transaction);

                        // create notification
                        Notification notification = shrimpCropManagementFactor.ToNotification();
                        notification.CreatedAt = now;
                        await this.DatabaseConnectService.Connection.InsertAsync<Notification>(notification, x => x.AttachToTransaction(transaction));
                    }
                    else
                    {
                        shrimpCropManagementFactor.Id = Guid.NewGuid();
                        await this.DatabaseConnectService.Connection.InsertAsync<ShrimpCropManagementFactor>(shrimpCropManagementFactor, x => x.AttachToTransaction(transaction));
                    }

                    transaction.Commit();

                    return shrimpCropManagementFactor.Id;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<bool> CancelShrimpCropManagementFactor(CancelShrimpCropManagementFactorDto dto)
        {
            var shrimpCropManagementFactor = await GetShrimpCropManagementFactorById(dto.ShrimpCropManagementFactoId);

            shrimpCropManagementFactor.Status = CropFactorStatus.StopWork.ToString();

            shrimpCropManagementFactor.ModifiedAt = DateTime.UtcNow;

            shrimpCropManagementFactor.ModifiedBy = _sessionService.UserId;

            await this.DatabaseConnectService.Connection.UpdateAsync<ShrimpCropManagementFactor>(shrimpCropManagementFactor);

            return true;
        }

        private async Task<string> GenerateShrimpCropCode()
        {
            var maxCode = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCrop>())
                    .OrderByDescending(x => x.CreatedAt).Select(x => x.Code).FirstOrDefault();

            if (maxCode == null)
            {
                return GenerateCodeConstants.Code + DateTime.UtcNow.ToString("yy") + string.Format(GenerateCodeConstants.Format, 1);
            }

            string[] arrMaxCode = maxCode.Split('.');

            var nextCode = string.Format(GenerateCodeConstants.Format, int.Parse(arrMaxCode[2]) + 1);

            var result = GenerateCodeConstants.Code + DateTime.UtcNow.ToString("yy") + nextCode;

            return result;
        }

        private async Task<ShrimpCropManagementFactor> GetShrimpCropManagementFactorById(Guid id)
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCropManagementFactor>(x => x
                        .Where($"bys_shrimp_crop_management_factor.id = @Id")
                        .WithParameters(new { Id = id }))).FirstOrDefault();

            return result == null ? throw new BusinessException("", ErrorCode.INTERNAL_SERVER_ERROR) : result;
        }

        private void ValidateShrimpCrop(CreateShrimpCropDto dto)
        {
            if (dto.Name.IsNullOrWhiteSpace()
                   || dto.FarmingLocation == null
                   || dto.ShrimpBreed == null
                   || dto.FromDate > dto.ToDate)
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }
        }

        private void ValidateCreateShrimpCropManagementFactor(CreateShrimpCropManagementFactorDto dto, long ShrimpCropFromDate, long ShrimpCropToDate)
        {
            if (dto.ManagementFactor == null || dto.Curator == null || dto.Frequency == null

                // OneTime
                || (dto.Frequency.Code == ShrimpCropFrequency.Onetime.ToString()
                        && (dto.ExecutionTime == null

                            || dto.ExecutionTime < ShrimpCropFromDate

                            || dto.ExecutionTime > ShrimpCropToDate))
                // Daily
                || (dto.Frequency.Code == ShrimpCropFrequency.Daily.ToString()
                        && (dto.FromDate == null || dto.ToDate == null

                                || dto.FromDate < ShrimpCropFromDate
                                || dto.FromDate > ShrimpCropToDate

                                || dto.ToDate < ShrimpCropFromDate
                                || dto.ToDate > ShrimpCropToDate

                                || dto.FromDate > dto.ToDate)))
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }
        }

        private async Task CreateWork(ShrimpCropManagementFactor dto, IDbTransaction transaction)
        {
            dto.CreatedBy = _sessionService.UserId;

            if (dto.Frequency == ShrimpCropFrequency.Onetime.ToString())
            {
                Work workOneTime = dto.ToWork();
                workOneTime.ExecutionTime = (DateTime)dto.ExecutionTime;
                await this.DatabaseConnectService.Connection.InsertAsync<Work>(workOneTime, x => x.AttachToTransaction(transaction));
            }
            else
            {
                var query = new StringBuilder();
                query.Append("INSERT INTO bys_main.bys_work(id, name, value , execution_time, shrimp_crop_management_factor_id, created_at, created_by, modified_at, modified_by, farming_location_id, shrimp_breed_id, curator, status)");
                query.Append("VALUES (@Id, @Name, @Value, @ExecutionTime, @ShrimpCropManagementFactorId, @CreatedAt, @CreatedBy, @ModifiedAt, @ModifiedBy, @FarmingLocationId, @ShrimpBreedId, @Curator, @Status)");

                var works = GetWorksInsert(dto);
                this.DatabaseConnectService.Connection.Execute(query.ToString(), works, transaction);
            }
        }

        private List<Work> GetWorksInsert(ShrimpCropManagementFactor dto)
        {
            TimeSpan days = (TimeSpan)(dto.ToDate - dto.FromDate);
            var lWork = new List<Work>();

            Work work = new Work();
            for (int i = 0; i <= days.TotalDays; i++)
            {
                work = dto.ToWork();
                work.ExecutionTime = ((DateTime)dto.ExecutionTime).AddDays(i);
                lWork.Add(work);
            }
            return lWork;
        }
    }
}
