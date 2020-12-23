using Dapper.FastCrud;
using Data;
using Data.Entity.Common;
using Microsoft.Extensions.Logging;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Implementation.Common.Helpers;
using Services.Interfaces.Common;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Common;
using Utilities.Constants;
using Utilities.Enums;

namespace Services.Implementation.Common
{
    public class ManagementFactorService : BaseService, IManagementFactorService
    {
        private readonly ILogger<ManagementFactorService> _logger;

        public ManagementFactorService(DatabaseConnectService databaseConnectionService, ILogger<ManagementFactorService> logger) : base(databaseConnectionService)
        {
            _logger = logger;
            DatabaseConnectService = databaseConnectionService;
        }

        public async Task<ShortManagementFactorDto[]> GetAllManagementFactor()
        {
            var managementFactorList = (await this.DatabaseConnectService.Connection.FindAsync<ManagementFactor>())
                                      .OrderByDescending(x => x.CreatedAt)
                                      .Select(x => x.ToShortManagementFactorDto()).ToArray();

            return managementFactorList;
        }

        public async Task<PageResultDto<ManagementFactorDto>> FilterManagementFactor(PageDto pageDto, string searchKey, string dataType, string factorGroup)
        {
            var query = new StringBuilder();
            query.Append("(@DataType is null OR bys_management_factor.data_type LIKE @DataType)");
            query.Append(" AND (@FactorGroup is null OR bys_management_factor.factor_group LIKE @FactorGroup)");
            query.Append(" AND (@SearchKey is null or (bys_management_factor.name LIKE @SearchKey OR bys_management_factor.description LIKE @SearchKey))");
            query.Append(" AND bys_management_factor.status = @Status");

            var param = new
            {
                DataType = dataType.FormatSearch(),
                FactorGroup = factorGroup.FormatSearch(),
                SearchKey = searchKey.FormatSearch(),
                Status = EntityStatus.Alive.ToString()
            };

            var factorGroupFromDb = await GetMasterData(CommonConstants.FACTOR_GROUP);

            var count = await this.DatabaseConnectService.Connection.CountAsync<ManagementFactor>(x => x
                                    .Include<MeasureUnit>(join => join.LeftOuterJoin())
                                    .Where($"{query}")
                                    .WithParameters(param));
            var items = (await this.DatabaseConnectService.Connection.FindAsync<ManagementFactor>(x => x
                                    .Include<MeasureUnit>(join => join.LeftOuterJoin())
                                    .Where($"{query}")
                                    .WithParameters(param)))
                                    .Skip(pageDto.Page * pageDto.PageSize)
                                    .Take(pageDto.PageSize)
                                    .Select(x => x.ToManagementFactorDto(factorGroupFromDb.Childs.Where(y => y.Code == x.FactorGroup).FirstOrDefault())).ToArray();

            var result = new PageResultDto<ManagementFactorDto>
            {
                Items = items,
                TotalCount = count,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageDto.PageSize),
            };

            return result;
        }

        #region Private methods

        private async Task<MasterDataResultDto> GetMasterData(string groupName)
        {
            var childs = (await this.DatabaseConnectService.Connection.FindAsync<MasterData>(x => x
                                .Where($"bys_master_data.group_name = @GroupName")
                                .WithParameters(new { GroupName = groupName }))).Select(x => x.ToDictionaryItemDto());

            var result = new MasterDataResultDto()
            {
                GroupName = groupName,
                Childs = childs.Where(x => x.TypeGroup == groupName).ToArray()
            };

            return result;
        }
        #endregion
    }
}
