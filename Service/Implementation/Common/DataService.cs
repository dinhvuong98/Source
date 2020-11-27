using Dapper.FastCrud;
using Data;
using Data.Entity.Account;
using Data.Entity.Common;
using Microsoft.Extensions.Logging;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Implementation.Common.Helpers;
using Services.Interfaces.Common;
using Services.Interfaces.Internal;
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
using Utilities.Helpers;

namespace Services.Implementation.Common
{
    public class DataService : BaseService, IDataService
    {
        #region PropertiesS
        private readonly ILogger<DataService> _logger;
        private readonly ISessionService _sessionService;
        #endregion

        #region #Constructor
        public DataService(DatabaseConnectService databaseConnectService, ILogger<DataService> logger, ISessionService sessionService) : base(databaseConnectService)
        {
            _logger = logger;
            _sessionService = sessionService;
            DatabaseConnectService = databaseConnectService;
        }

        #endregion

        #region Public methods

        public async Task<ItemResultDto<AreaDto>> GetAreas(PageDto pageDto, string searchKey)
        {
            var query = "bys_area.name LIKE @SearchKey OR bys_area.code LIKE @SearchKey OR bys_area.address LIKE @SearchKey";

            var count = await this.DatabaseConnectService.Connection.CountAsync<Areas>(x => x
                        .Where($"{query}")
                        .WithParameters(new { SearchKey = searchKey.FormatSearch() }));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<Areas>(x => x
                        .Where($"{query}")
                        .WithParameters(new { SearchKey = searchKey.FormatSearch() })))
                        .Skip(pageDto.Page * pageDto.PageSize)
                        .Take(pageDto.PageSize)
                        .Select(x => x.ToAreaDto()).ToArray();

            var result = new ItemResultDto<AreaDto>();

            result.Items = items;

            result.TotalCount = count;

            PageHelper.PageValue<AreaDto>(result, pageDto);

            return result;
        }

        public async Task<ShortFarmingLocationDto[]> GetAllFarmingLocaiton()
        {
            var farmingLocationList = (await this.DatabaseConnectService.Connection.FindAsync<FarmingLocation>())
                                       .Select(x => x.ToShortFarmingLocationDto()).ToArray();

            return farmingLocationList;
        }

        public async Task<ItemResultDto<FarmingLocationDto>> GetFarmingLocaiton(PageDto pageDto, string searchKey, string locationType)
        {
            string query = "(bys_farming_location.type LIKE @LocationType) AND (bys_farming_location.code LIKE @SearchKey OR bys_farming_location.name LIKE @SearchKey OR bys_area.name LIKE @SearchKey OR bys_farming_location.description LIKE @SearchKey)";

            var Count = await this.DatabaseConnectService.Connection.CountAsync<FarmingLocation>(x => x
                                    .Include<Areas>(join => join.LeftOuterJoin())
                                    .Where($"{query}")
                                    .WithParameters(new { SearchKey = searchKey.FormatSearch(), LocationType = locationType.FormatSearch() }));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<FarmingLocation>(x => x
                                    .Include<Areas>(join => join.LeftOuterJoin())
                                    .Where($"{query}")
                                    .WithParameters(new { SearchKey = searchKey.FormatSearch(), LocationType = locationType.FormatSearch() })))
                                    .Skip(pageDto.Page * pageDto.PageSize)
                                    .Take(pageDto.PageSize)
                                    .Select(x => x.ToFarmingLocationDto())
                                    .ToArray();

            var result = new ItemResultDto<FarmingLocationDto>();

            result.Items = items;

            result.TotalCount = Count;

            PageHelper.PageValue<FarmingLocationDto>(result, pageDto);

            return result;
        }

        public async Task<ShortShrimpBreedDto[]> GetAllShrimpBreed()
        {
            var shrimpBreedList = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpBreed>())
                                       .Select(x => x.ToShortShrimpBreedDto()).ToArray();

            return shrimpBreedList;
        }

        public async Task<ItemResultDto<ShrimpBreedDto>> GetShrimpBreed(PageDto pageDto, string searchKey)
        {
            var query = "(bys_shrimp_breed.code LIKE @SearchKey OR bys_shrimp_breed.name LIKE @SearchKey OR bys_shrimp_breed.description LIKE @SearchKey)";

            var count = await this.DatabaseConnectService.Connection.CountAsync<ShrimpBreed>(x => x
                                    .Where($"{query}")
                                    .WithParameters(new { SearchKey = searchKey.FormatSearch() }));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpBreed>(x => x
                                    .Where($"{query}")
                                    .WithParameters(new { SearchKey = searchKey.FormatSearch() })))
                                    .Skip(pageDto.Page * pageDto.PageSize)
                                    .Take(pageDto.PageSize)
                                    .Select(x => x.ToShrimpBreedDto()).ToArray();

            var result = new ItemResultDto<ShrimpBreedDto>();

            result.Items = items;

            result.TotalCount = count;

            PageHelper.PageValue<ShrimpBreedDto>(result, pageDto);

            return result;
        }

        public async Task<ShortManagementFactorDto[]> GetAllManagementFactor()
        {
            var managementFactorList = (await this.DatabaseConnectService.Connection.FindAsync<ManagementFactor>())
                                      .Select(x => x.ToShortManagementFactorDto()).ToArray();

            return managementFactorList;
        }

        public async Task<ItemResultDto<ManagementFactorDto>> GetManagementFactor(PageDto pageDto, string searchKey, string dataType, string factorGroup)
        {
            var query = "(bys_management_factor.data_type LIKE @DataType) AND (bys_management_factor.factor_group LIKE @FactorGroup) AND (bys_management_factor.name LIKE @SearchKey OR bys_management_factor.description LIKE @SearchKey)";

            var factorGroups = await GetMasterData(CommonConstants.FACTOR_GROUP);

            var count = await this.DatabaseConnectService.Connection.CountAsync<ManagementFactor>(x => x
                                    .Include<MeasureUnit>(join => join.LeftOuterJoin())
                                    .Where($"{query}")
                                    .WithParameters(new { SearchKey = searchKey.FormatSearch(), DataType = dataType.FormatSearch(), FactorGroup = factorGroup.FormatSearch() }));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<ManagementFactor>(x => x
                                    .Include<MeasureUnit>(join => join.LeftOuterJoin())
                                    .Where($"{query}")
                                    .WithParameters(new { SearchKey = searchKey.FormatSearch(), DataType = dataType.FormatSearch(), FactorGroup = factorGroup.FormatSearch() })))
                                    .Skip(pageDto.Page * pageDto.PageSize)
                                    .Take(pageDto.PageSize)
                                    .Select(x => x.ToManagementFactorDto(factorGroups.FirstOrDefault().Childs.Where(y => y.Code == x.FactorGroup).FirstOrDefault())).ToArray();

            var result = new ItemResultDto<ManagementFactorDto>();

            result.Items = items;

            result.TotalCount = count;

            PageHelper.PageValue<ManagementFactorDto>(result, pageDto);

            return result;
        }
        public async Task<ItemResultDto<ShrimpCropDto>> GetShrimpCrop(PageDto pageDto, string searchKey, string farmingLocationId, string shrimpBreedId)
        {
            var query = new StringBuilder();
            query.Append("(bys_shrimp_crop.name LIKE @SearchKey OR bys_shrimp_crop.code LIKE @SearchKey)");
            query.Append("AND (bys_shrimp_crop.farming_location_id LIKE @FarmingLocationId)");
            query.Append("AND (bys_shrimp_crop.shrimp_breed_id LIKE @ShrimpBreedId)");

            var count = await this.DatabaseConnectService.Connection.CountAsync<ShrimpCrop>(x => x
                        .Include<FarmingLocation>(join => join.LeftOuterJoin())
                        .Include<ShrimpBreed>(join => join.LeftOuterJoin())
                        .Where($"{query}")
                        .WithParameters(new { SearchKey = searchKey.FormatSearch(), FarmingLocationId = farmingLocationId.FormatSearch(), ShrimpBreedId = shrimpBreedId.FormatSearch() }));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCrop>(x => x
                        .Include<FarmingLocation>(join => join.LeftOuterJoin())
                        .Include<ShrimpBreed>(join => join.LeftOuterJoin())
                        .Where($"{query}")
                        .WithParameters(new { SearchKey = searchKey.FormatSearch(), FarmingLocationId = farmingLocationId.FormatSearch(), ShrimpBreedId = shrimpBreedId.FormatSearch() })))
                        .Skip(pageDto.Page * pageDto.PageSize)
                        .Take(pageDto.PageSize)
                        .Select(x => x.ToShrimpCropDto())
                        .ToArray();
                        
            var result = new ItemResultDto<ShrimpCropDto>();
        
            result.Items = items;

            result.TotalCount = count;

            PageHelper.PageValue<ShrimpCropDto>(result, pageDto);

            return result;
        }
        public async Task<MasterDataResultDto[]> GetMasterData(string groupsName)
        {
            string[] arrGroupName = groupsName.Split(',');

            var childs = (await this.DatabaseConnectService.Connection.FindAsync<MasterData>(x => x
                                .Where($"bys_master_data.group_name IN @GroupsName")
                                .WithParameters(new { GroupsName = arrGroupName }))).Select(x => x.ToDictionaryItemDto());

            var result = new List<MasterDataResultDto>();

            foreach (string groupName in arrGroupName)
            {
                result.Add(new MasterDataResultDto
                {
                    GroupName = groupName,
                    Childs = childs.Where(x => x.TypeGroup == groupName).ToArray()
                });
            }

            return result.ToArray();
        }

        public async Task<ShrimpCropResultDto> CreateShrimpCrop(CreateShrimpCropDto createShrimpCropDto)
        {
            _logger.LogInformation("Start method create shrimp crop");

            ValidateShrimpCrop(createShrimpCropDto, null);

            using (IDbTransaction transaction = this.DatabaseConnectService.BeginTransaction())
            {
                try
                {
                    ShrimpCrop shrimpCrop = createShrimpCropDto.ToShrimpCrop();

                    shrimpCrop.Code = await GenerateShrimpCropCode(transaction);

                    shrimpCrop.CreatedBy = _sessionService.UserId;

                    await this.DatabaseConnectService.Connection.InsertAsync(shrimpCrop, x => x.AttachToTransaction(transaction));

                    transaction.Commit();

                    _logger.LogInformation("end mothod create shrimp crop");

                    return await GetShrimpCropById(shrimpCrop.Id);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
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

            return result == null ? throw new BusinessException("Invalid Object!", ErrorCode.NOT_EXIST) : result;
        }

        public async Task<ShrimpCropResultDto> UpDateShrimpCrop(UpdateShrimpCropDto upateShrimpCropDto)
        {
            _logger.LogInformation("start method update shrimp crop");

            ValidateShrimpCrop(null, upateShrimpCropDto);

            var shrimpCrop = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCrop>(x => x
                            .Where($"bys_shrimp_crop.id = @Id")
                            .WithParameters(new { Id = upateShrimpCropDto.Id }))).FirstOrDefault();

            using (IDbTransaction transaction = this.DatabaseConnectService.BeginTransaction())
            {
                try
                {
                    ShrimpCrop shrimpCropUpdate = upateShrimpCropDto.ToShrimpCrop();

                    shrimpCropUpdate.ModifiedBy = _sessionService.UserId;
                    shrimpCropUpdate.CreatedAt = shrimpCrop.CreatedAt;
                    shrimpCropUpdate.CreatedBy = shrimpCrop.CreatedBy;
                    shrimpCropUpdate.Code = shrimpCrop.Code;

                    await this.DatabaseConnectService.Connection.UpdateAsync(shrimpCropUpdate, x => x.AttachToTransaction(transaction));

                    transaction.Commit();

                    _logger.LogInformation("end method update shrimp crop");

                    return await GetShrimpCropById(shrimpCropUpdate.Id);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<Guid> CreateOrUpdateShrimpCropManagementFactor(CreateShrimpCropManagementFactorDto dto)
        {
            // validate
            DateTime dateTime = DateTime.UtcNow;
            using (IDbTransaction transaction = this.DatabaseConnectService.BeginTransaction())
            {
                try
                {
                    ShrimpCropManagementFactor shrimpCropManagementFactor = dto.ToShrimpCropManagementFactor();
                    if (dto.Id == null)
                    {
                        shrimpCropManagementFactor.CreatedBy = _sessionService.UserId;
                        shrimpCropManagementFactor.CreatedAt = dateTime;
                        shrimpCropManagementFactor.Id = Guid.NewGuid();

                        await this.DatabaseConnectService.Connection.InsertAsync<ShrimpCropManagementFactor>(shrimpCropManagementFactor, x => x.AttachToTransaction(transaction));

                        if (dto.IsCreateWork)
                        {
                            // create work

                            // create notification
                        }
                    }
                    else
                    {
                        shrimpCropManagementFactor.ModifiedBy = _sessionService.UserId;

                        shrimpCropManagementFactor.ModifiedAt = dateTime;

                        await this.DatabaseConnectService.Connection.UpdateAsync<ShrimpCropManagementFactor>(shrimpCropManagementFactor, x => x.AttachToTransaction(transaction));
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

            if (shrimpCropManagementFactor == null)
                return false;

            shrimpCropManagementFactor.Status = CropFactorStatus.StopWork.ToString();

            shrimpCropManagementFactor.ModifiedAt = dto.ModifiedAt.FromUnixTimeStamp();

            shrimpCropManagementFactor.ModifiedBy = _sessionService.UserId;

            await this.DatabaseConnectService.Connection.UpdateAsync<ShrimpCropManagementFactor>(shrimpCropManagementFactor);

            return true;
        }


        public async Task<bool> StopWork(Guid shrimpCropManagementFactorId)
        {
            var work = await GetWorkById(shrimpCropManagementFactorId);

            if (work == null)
                return false;

            work.Status = CropFactorStatus.StopWork.ToString();

            work.ModifiedAt = DateTime.UtcNow;

            work.ModifiedBy = _sessionService.UserId;

            await this.DatabaseConnectService.Connection.UpdateAsync<Work>(work);

            return true;
        }

        #endregion

        #region Private methods
        private async Task<string> GenerateShrimpCropCode(IDbTransaction transaction)
        {
            var maxCode = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCrop>(x => x
                    .AttachToTransaction(transaction)))
                    .OrderByDescending(x => x.CreatedAt).Select(x => x.Code).FirstOrDefault();

            if(maxCode == null)
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

            return result == null ? throw new BusinessException("invalid object", ErrorCode.NOT_EXIST) : result;
        }

        private async Task<ShrimpCropDto> GetShrimpCrop(Guid id, IDbTransaction transaction)
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpCrop>(x => x.AttachToTransaction(transaction)
                        .Where($"bys_shrimp_crop.id = @Id")
                        .WithParameters(new { Id = id }))).Select(x => x.ToShrimpCropDto()).FirstOrDefault();

            return result == null ? throw new BusinessException("Invalid Object!", ErrorCode.NOT_EXIST) : result;
        }

        private async Task<Work> GetWorkById(Guid id)
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<Work>(x => x
                        .Where($"bys_work.id = @Id")
                        .WithParameters(new { Id = id }))).FirstOrDefault();

            return result == null ? throw new BusinessException("Invalid Object!", ErrorCode.NOT_EXIST) : result;
        }

        private void ValidateShrimpCrop(CreateShrimpCropDto createDto, UpdateShrimpCropDto updateDto)
        {
            if (createDto != null)
            {
                if (createDto.Name.Equals("")
                       || createDto.FarmingLocation == null
                       || createDto.ShrimpBreed == null
                       || DateTime.Compare(createDto.FromDate.FromUnixTimeStamp(), createDto.ToDate.FromUnixTimeStamp()) == 1)
                {
                    throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
                }
            }
            else
            {
                if (updateDto.Name.Equals("")
                       || updateDto.FarmingLocation == null
                       || updateDto.ShrimpBreed == null
                       || DateTime.Compare(updateDto.FromDate.FromUnixTimeStamp(), updateDto.ToDate.FromUnixTimeStamp()) == 1)
                {
                    throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
                }
            }
        }

        private async Task<bool> CreateWork(Work work, IDbTransaction transaction)
        {
            // validate
            await this.DatabaseConnectService.Connection.InsertAsync<Work>(work, x => x.AttachToTransaction(transaction));
            return true;
        }

        private async Task<bool> CreateNotification(Notification notification, IDbTransaction transaction)
        {
            // validate
            await this.DatabaseConnectService.Connection.InsertAsync<Notification>(notification, x => x.AttachToTransaction(transaction));

            return true;
        }

        #endregion
    }
}
