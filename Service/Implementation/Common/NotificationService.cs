using Data;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using Services.Implementation.Common.Helpers;
using Services.Interfaces.Common;
using Services.Interfaces.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Services.Implementation.Common
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly ISessionService _sessionSevice;
        private readonly IDataService _dataService;

        #region Construtor

        public NotificationService(DatabaseConnectService databaseConnectService, ISessionService sesionService, IDataService dataService) : base(databaseConnectService)
        {
            _dataService = dataService;
            _sessionSevice = sesionService;
            DatabaseConnectService = databaseConnectService;
        }

        #endregion

        #region Public methods

        public async Task<ItemResultDto<NotificationDto>> GetNotification(PageDto pageDto)
        {
            var query = new StringBuilder();

            query.Append("SELECT a.id as Id, a.type as Type , a.execution_time as ExecutionTime , a.from_date as FromDate, a.to_date as ToDate , a.status as Status, a.frequency as Frequency, ");
            query.Append("b.id as FarmingLocationId, b.name as FarmingLocationName, b.code as FarmingLocationCode,");
            query.Append("c.id as ManagementFactorId, c.name as ManagementFactorName, c.code as ManagementFactorCode, c.");
            query.Append("d.id as ShrimpCropId, d.name as ShrimpCropName, d.code as ShrimpCropCode, d.from_date as ShrimpCropFromDate, d.to_date as ShrimpCropToDate,");
            query.Append("e.id as ShrimpBreedId, e.name as ShrimpBreedName, e.code as ShrimpBreedCode, e.description as ShrimpBreedDescription, e.attachment as ShrimpBreedAttachment");
            query.Append("FROM bys_main.bys_notification a");
            query.Append("LEFT JOIN bys_main.bys_farming_location b on b.id = a.farming_location_id");
            query.Append("LEFT JOIN bys_main.bys_management_factor c on c.id = a.management_factor_id");
            query.Append("LEFT JOIN bys_main.bys_shrimp_crop d on d.id = a.shrimp_crop_id");
            query.Append("LEFT JOIN bys_main.bys_shrimp_breed e on e.id = d.shrimp_breed_id");
            query.Append("WHERE a.user_id = @UserId");
            query.Append("ORDER BY a.created_at DESC");

            var parameter = new
            {
                //UserId = "7ab96dd1-83a7-4eed-9070-7a80ecaf52e8",
                UserId = _sessionSevice.UserId,
                Page = pageDto.Page,
                PageSize = pageDto.PageSize
            };

            var count = (await this.DatabaseConnectService.SelectAsync<NotificationResultDto>(query.ToString(), parameter)).Count;

            query.Append("OFFSET @Page * @PageSize ROWS FETCH NEXT @pageSize ROWS ONLY;");

            var factorGroups = await _dataService.GetMasterData(CommonConstants.FACTOR_GROUP);

            var items = (await this.DatabaseConnectService.SelectAsync<NotificationResultDto>(query.ToString(), parameter))
                        .Select(x => x
                            .ToNotificationDto(factorGroups
                                            .FirstOrDefault().Childs
                                            .Where(y => y.Code == x.FactorGroupName)
                                            .FirstOrDefault()))
                        .ToArray();

            var result = new ItemResultDto<NotificationDto>();

            result.TotalCount = count;

            result.Items = items;

            PageHelper.PageValue<NotificationDto>(result, pageDto);

            return result;
        }

        #endregion

        #region Private mothods



        #endregion
    }
}
