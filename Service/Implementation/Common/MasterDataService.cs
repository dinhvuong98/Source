using Dapper.FastCrud;
using Data;
using Data.Entity.Account;
using Data.Entity.Common;
using Microsoft.Extensions.Logging;
using Services.Dtos.Account;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Implementation.Account.Helpers;
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

namespace Services.Implementation.Common
{
    public class MasterDataService : BaseService, IMasterDataService
    {
        #region Properties
        private readonly ILogger<MasterDataService> _logger;
        #endregion

        #region #Constructor
        public MasterDataService(
            DatabaseConnectService databaseConnectService, 
            ILogger<MasterDataService> logger, 
            ISessionService sessionService,
            ICacheProvider cacheProvider) : base(databaseConnectService)
        {
            _logger = logger;
            DatabaseConnectService = databaseConnectService;
        }

        #endregion

        #region Public methods

        public async Task<PageResultDto<AreaDto>> FilterAreas(PageDto pageDto, string searchKey)
        {
            var query = "@SearchKey is null or (bys_area.name LIKE @SearchKey OR bys_area.code LIKE @SearchKey OR bys_area.address LIKE @SearchKey)";

            var param = new
            {
                SearchKey = searchKey.FormatSearch()
            };

            var count = await this.DatabaseConnectService.Connection.CountAsync<Areas>(x => x
                        .Where($"{query}")
                        .WithParameters(param));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<Areas>(x => x
                        .Where($"{query}")
                        .WithParameters(param)))
                        .Skip(pageDto.Page * pageDto.PageSize)
                        .Take(pageDto.PageSize)
                        .Select(x => x.ToAreaDto()).ToArray();

            var result = new PageResultDto<AreaDto>
            {
                Items = items,
                TotalCount = count,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageDto.PageSize),
            };

            return result;
        }

        public async Task<ShortFarmingLocationDto[]> GetAllFarmingLocaiton(string locationType)
        {
            var farmingLocationList = (await this.DatabaseConnectService.Connection.FindAsync<FarmingLocation>(x => x
                                    .Where($"@locationType is null OR (bys_farming_location.type LIKE @LocationType)")
                                    .WithParameters(new {LocationType = locationType })))
                                    .OrderByDescending(x => x.CreatedAt)
                                    .Select(x => x.ToShortFarmingLocationDto()).ToArray();

            return farmingLocationList;
        }

        public async Task<PageResultDto<FarmingLocationDto>> FilterFarmingLocation(PageDto pageDto, string searchKey, string locationType)
        {
            var query = new StringBuilder();
            query.Append("(@LocationType is null OR (bys_farming_location.type LIKE @LocationType))");
            query.Append(" AND (@SearchKey is null or (bys_farming_location.code LIKE @SearchKey OR bys_farming_location.name LIKE @SearchKey OR bys_area.name LIKE @SearchKey OR bys_farming_location.description LIKE @SearchKey))");

            var param = new
            {
                SearchKey = searchKey.FormatSearch(),
                LocationType = locationType.FormatSearch()
            };

            var count = await this.DatabaseConnectService.Connection.CountAsync<FarmingLocation>(x => x
                                    .Include<Areas>(join => join.LeftOuterJoin())
                                    .Where($"{query}")
                                    .WithParameters(param));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<FarmingLocation>(x => x
                                    .Include<Areas>(join => join.LeftOuterJoin())
                                    .Where($"{query}")
                                    .WithParameters(param)))
                                    .Skip(pageDto.Page * pageDto.PageSize)
                                    .Take(pageDto.PageSize)
                                    .Select(x => x.ToFarmingLocationDto())
                                    .ToArray();

            var result = new PageResultDto<FarmingLocationDto>
            {
                Items = items,
                TotalCount = count,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageDto.PageSize),
            };

            return result;
        }

        public async Task<ShortShrimpBreedDto[]> GetAllShrimpBreed()
        {
            var shrimpBreedList = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpBreed>())
                                    .OrderByDescending(x => x.CreatedAt)
                                    .Select(x => x.ToShortShrimpBreedDto()).ToArray();

            return shrimpBreedList;
        }

        public async Task<PageResultDto<ShrimpBreedDto>> FilterShrimpBreed(PageDto pageDto, string searchKey)
        {
            var query = new StringBuilder();
            query.Append("@SearchKey is null OR (bys_shrimp_breed.code LIKE @SearchKey OR bys_shrimp_breed.name LIKE @SearchKey OR bys_shrimp_breed.description LIKE @SearchKey)");

            var param = new
            {
                SearchKey = searchKey.FormatSearch()
            };

            var count = await this.DatabaseConnectService.Connection.CountAsync<ShrimpBreed>(x => x
                                    .Where($"{query}")
                                    .WithParameters(param));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<ShrimpBreed>(x => x
                                    .Where($"{query}")
                                    .WithParameters(param)))
                                    .Skip(pageDto.Page * pageDto.PageSize)
                                    .Take(pageDto.PageSize)
                                    .Select(x => x.ToShrimpBreedDto()).ToArray();

            var result = new PageResultDto<ShrimpBreedDto>
            {
                Items = items,
                TotalCount = count,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageDto.PageSize),
            };

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

        public async Task<AddressDto[]> GetAddressMasterData()
        {
            var countries = await this.DatabaseConnectService.Connection.FindAsync<Country>(x => x
                          .Include<Province>(j => j.LeftOuterJoin())
                          .Include<District>(j => j.LeftOuterJoin())
                          .Include<Commune>(j => j.LeftOuterJoin())
                          .Where($"bys_country.id != @Id AND bys_province.id != @Id AND bys_district.id != @Id AND bys_commune.id != @Id")
                          .WithParameters(new {Id = 0 }));

            var result = countries.Select(x => x.ToAddressDto()).OrderBy(x => x.Name).ToArray();

            return result;
        }

        public async Task<FeatureDto[]> GetAllFeature()
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<Feature>(x => x
                            .Where($"bys_feature.status = @Status")
                            .WithParameters(new { Status = EntityStatus.Alive.ToString() })))
                            .Select(x => x.ToFeatureDto()).ToArray();
            return result;
        }

        #endregion
    }
}
