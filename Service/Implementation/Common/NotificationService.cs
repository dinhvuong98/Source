using Dapper;
using Dapper.FastCrud;
using Data;
using Data.Entity.Account;
using Data.Entity.Common;
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
using Utilities.Constants;
using Utilities.Enums;

namespace Services.Implementation.Common
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly ISessionService _sessionSevice;
        private readonly IMasterDataService _dataService;

        #region Construtor

        public NotificationService(DatabaseConnectService databaseConnectService, ISessionService sesionService, IMasterDataService dataService) : base(databaseConnectService)
        {
            _dataService = dataService;
            _sessionSevice = sesionService;
            DatabaseConnectService = databaseConnectService;
        }

        #endregion

        #region Public methods

        public async Task<PageResultDto<NotificationDto>> FilterNotification(PageDto pageDto)
        {
            var query = new StringBuilder();
            query.Append("SELECT a.id as Id, a.type as Type , a.execution_time as ExecutionTime , a.from_date as FromDate, a.to_date as ToDate , a.status as Status, a.frequency as Frequency, ");
            query.Append("      b.id as FarmingLocationId, b.name as FarmingLocationName, b.code as FarmingLocationCode, ");
            query.Append("      c.id as ManagementFactorId, c.name as ManagementFactorName, c.code as ManagementFactorCode, ");
            query.Append("      d.id as ShrimpCropId, d.name as ShrimpCropName, d.code as ShrimpCropCode, d.from_date as ShrimpCropFromDate, d.to_date as ShrimpCropToDate, ");
            query.Append("      e.id as ShrimpBreedId, e.name as ShrimpBreedName, e.code as ShrimpBreedCode, e.description as ShrimpBreedDescription, e.attachment as ShrimpBreedAttachment ");
            query.Append("FROM bys_main.bys_notification a ");
            query.Append("      LEFT JOIN bys_main.bys_farming_location b on b.id = a.farming_location_id ");
            query.Append("      LEFT JOIN bys_main.bys_management_factor c on c.id = a.management_factor_id ");
            query.Append("      LEFT JOIN bys_main.bys_shrimp_crop d on d.id = a.shrimp_crop_id ");
            query.Append("      LEFT JOIN bys_main.bys_shrimp_breed e on e.id = d.shrimp_breed_id ");
            query.Append("      LEFT JOIN bys_main.bys_shrimp_crop_management_factor g on g.id = a.shrimp_crop_management_factor_id ");
            query.Append("WHERE a.user_id = @UserId AND g.status != @CropFactorStatus ");

            var lastTimeNotification = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                                        .Where($"id = @UserId")
                                        .WithParameters(new { UserId = _sessionSevice.UserId }))).FirstOrDefault().LastTimeReadNotification;
            var param = new
            {
                UserId = _sessionSevice.UserId,
                Page = pageDto.Page,
                PageSize = pageDto.PageSize,
                Status = NotificationStatus.New.ToString(),
                LastTimeReadNotification = lastTimeNotification,
                CropFactorStatus = CropFactorStatus.StopWork.ToString()
            };

            // Items
            var countRecord = (await this.DatabaseConnectService.SelectAsync<NotificationResultDto>(query.ToString(), param)).Count;

            query.Append(" ORDER BY a.created_at DESC ");
            query.Append(" OFFSET @Page * @PageSize ROWS FETCH NEXT @pageSize ROWS ONLY; ");

            var factorGroups = await _dataService.GetMasterData(CommonConstants.FACTOR_GROUP);

            var items = (await this.DatabaseConnectService.SelectAsync<NotificationResultDto>(query.ToString(), param))
                        .Select(x => x
                        .ToNotificationDto(factorGroups.FirstOrDefault().Childs.Where(y => y.Code == x.FactorGroupName).FirstOrDefault()))
                        .ToArray();

            // ExtraData
            var queryExtraData = new StringBuilder();

            queryExtraData.Append("bys_notification.user_id = @UserId AND bys_shrimp_crop_management_factor.status != @CropFactorStatus AND bys_notification.status = @Status ");
            var countUnread = await this.DatabaseConnectService.Connection.CountAsync<Notification>(x => x
                                .Include<ShrimpCropManagementFactor>(j => j.LeftOuterJoin())
                                .Where($"{queryExtraData}")
                                .WithParameters(param));

            queryExtraData.Append("AND bys_notification.created_at > @LastTimeReadNotification ");
            var countNew = await this.DatabaseConnectService.Connection.CountAsync<Notification>(x => x
                                .Include<ShrimpCropManagementFactor>(j => j.LeftOuterJoin())
                                .Where($"{queryExtraData}")
                                .WithParameters(param));

            var extraData = new
            {
                countNew = countNew,
                countUnread = countUnread
            };

            var result = new PageResultDto<NotificationDto>
            {
                Items = items,
                TotalCount = countRecord,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(countRecord / (double)pageDto.PageSize),
                ExtraData = extraData
            };

            return result;
        }

        public async Task<bool> MarkAsReadNotification(string id)
        {
            var notification = (await this.DatabaseConnectService.Connection.FindAsync<Notification>(x => x
                            .Where($"bys_notification.id = @Id")
                            .WithParameters(new { Id = id }))).FirstOrDefault();

            if(notification == null)
            {
                return false;
            }

            notification.Status = NotificationStatus.Read.ToString();

            await this.DatabaseConnectService.Connection.UpdateAsync<Notification>(notification);

            return true;
        }

        public async Task<bool> MarkAllAsReadNotification(long timestamp)
        {
            var queryUnread = new StringBuilder();
            queryUnread.Append("bys_notification.user_id = @UserId AND bys_shrimp_crop_management_factor.status != @CropFactorStatus AND bys_notification.status = @Status ");

            var paramUnread = new
            {
                UserId = _sessionSevice.UserId,
                Status = NotificationStatus.New.ToString(),
                CropFactorStatus = CropFactorStatus.StopWork.ToString()
            };
            
            var notifyUnread = await this.DatabaseConnectService.Connection.FindAsync<Notification>(x => x
                                .Include<ShrimpCropManagementFactor>(j => j.LeftOuterJoin())
                                .Where($"{queryUnread}")
                                .WithParameters(paramUnread));

            if (notifyUnread == null)
            {
                return false;
            }

            var queryUpdate = new StringBuilder();
            queryUpdate.Append("UPDATE bys_main.bys_notification SET status = @Status Where id IN @Ids");

            var transaction = this.DatabaseConnectService.BeginTransaction();

            var paramUpDate = new
            {
                Status = NotificationStatus.Read,
                Ids = notifyUnread.Select(x => x.Id).ToArray()
            };

            this.DatabaseConnectService.Connection.Execute(queryUpdate.ToString(), paramUpDate, transaction);

            return true;
        }

        public async Task<bool> CreateRemind()
        {
            var query = new StringBuilder();
            query.Append("INSERT bys_main.bys_notification (id, type, factor_name, farming_location_name, execution_time, shrimp_crop_name, from_date, to_date, user_id, status, frequency, created_at, work_id, management_factor_id, farming_location_id, shrimp_crop_id, shrimp_crop_management_factor_id) ");
            query.Append("VALUES (@Id, @Type, @FactorName, @FarmingLocationName, @ExecutionTime, @ShrimpCropName, @FromDate, @ToDate, @UserId, @Status, @Frequency, @CreatedAt, @WorkId, @ManagementFactorId, @FarmingLocationId, @ShrimpCropId, @ShrimpCropManagementFactorId) ");

            var lRemind = await GetWorksRemind();

            if(lRemind == null)
            {
                return false;
            }

            this.DatabaseConnectService.Connection.Execute(query.ToString(), lRemind);

            return true;
        }

        #endregion

        #region Private mothods
        private async Task<List<Notification>> GetWorksRemind()
        {
            var now = DateTime.UtcNow;

            var query = new StringBuilder();

            query.Append(" SELECT a.id AS Id, a.execution_time AS ExecutionTime,a.shrimp_crop_management_factor_id AS ShrimpCropManagementFactorId,a.farming_location_id AS FarmingLocationId, ");
            query.Append("      a.shrimp_breed_id AS ShrimpBreedId,a.curator AS Curator, ");
            query.Append("      b.id AS ShrimpCropManagementFactorId,b.management_factor_id AS ManagementFactorId,b.shrimp_crop_id AS ShrimpCropId, ");
            query.Append("      b.from_date AS FromDate, b.to_date AS ToDate ");
            query.Append(" FROM bys_main.bys_work AS a ");
            query.Append("      LEFT JOIN bys_main.bys_shrimp_crop_management_factor b ON b.id = a.shrimp_crop_management_factor_id ");
            query.Append(" WHERE a.execution_time >= @Now ");
            query.Append("      AND datediff(MINUTE, @Now, a.execution_time) <= @Minute ");
            query.Append("      AND NOT EXISTS (SELECT work_id FROM bys_main.bys_notification WHERE a.id = work_id AND type = @Type) ");
            query.Append("      AND a.value IS NULL AND a.status = @StatusWork");

            var param = new
            {
                Type = NotifyType.Remind.ToString(),
                StatusWork = EntityStatus.Alive.ToString(),
                Now = now,
                Minute = 60
            };

            var result = this.DatabaseConnectService.Select<WorkRemindResultDto>(query.ToString(), param).Select(x => x.ToWorkReMind(now)).ToList();

            return result;
        }

        #endregion
    }
}
