using Data;
using Services.Dtos.Account;
using Services.Dtos.Account.InputDtos;
using Services.Interfaces.Account;
using System.Threading.Tasks;
using Data.Entity.Account;
using System.Linq;
using Services.Implementation.Account.Helpers;
using Dapper.FastCrud;
using Utilities.Enums;
using Utilities.Exceptions;
using Services.Interfaces.Internal;
using System;
using Services.Dtos.Common.InputDtos;
using Microsoft.Extensions.Logging;
using System.Text;
using Services.Dtos.Common;
using Utilities.Common;
using Quartz.Util;
using System.Data;
using Services.Interfaces.RedisCache;
using Utilities.Constants;

namespace Services.Implementation.Account
{
    public class GroupService : BaseService, IGroupService
    {
        #region Properties
        private readonly ISessionService _sessionService;
        private readonly ICacheProvider _redisCache;
        private readonly ILogger<GroupService> _logger;
        #endregion

        #region Constructor
        public GroupService(DatabaseConnectService databaseConnectService, ISessionService sessionService, ICacheProvider redisCache, ILogger<GroupService> logger) : base(databaseConnectService)
        {
            _sessionService = sessionService;
            _redisCache = redisCache;
            _logger = logger;
            DatabaseConnectService = databaseConnectService;
        }
        #endregion

        #region Publish method

        public async Task<GroupDto[]> GetAllGroup()
        {
            _logger.LogInformation("start method get all group");

            var param = new
            {
                GroupStatus = EntityStatus.Alive.ToString(),
            };

            var result = (await this.DatabaseConnectService.Connection.FindAsync<Group>(x => x
                        .Include<UserGroup>(j => j.LeftOuterJoin())
                        .Where($"bys_group.status = @GroupStatus")
                        .WithParameters(param)))
                        .Select(x => x.ToGroupDto()).ToArray();

            _logger.LogInformation("end method get all group");

            return result;
        }

        public async Task<PageResultDto<GroupDto>> FilterGroup(PageDto pageDto, string searchKey)
        {
            _logger.LogInformation("start method filter group");

            var query = new StringBuilder();
            query.Append("@SearchKey is null or (bys_group.name LIKE @SearchKey) AND bys_group.status = @GroupStatus");

            var param = new
            {
                SearchKey = searchKey.FormatSearch(),
                GroupStatus = EntityStatus.Alive.ToString(),
            };

            var count = await this.DatabaseConnectService.Connection.CountAsync<Group>(x => x
                        .Where($"{query}")
                        .WithParameters(param));

            var items = (await this.DatabaseConnectService.Connection.FindAsync<Group>(x => x
                        .Include<GroupFeature>(j => j.LeftOuterJoin())
                        .Include<UserGroup>(j => j.LeftOuterJoin())
                        .Where($"{query}")
                        .WithParameters(param)))
                        .Skip(pageDto.Page * pageDto.PageSize)
                        .Take(pageDto.PageSize)
                        .Select(x => x.ToGroupDto()).ToArray();

            var result = new PageResultDto<GroupDto>
            {
                Items = items,
                TotalCount = count,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageDto.PageSize),
            };

            _logger.LogInformation("end method filter group");

            return result;
        }

        public async Task<GroupDto> GetGroupById(Guid id)
        {
            _logger.LogInformation("start method get group by id");

            var result = (await this.DatabaseConnectService.Connection.FindAsync<Group>(x => x
                            .Where($"bys_group.id = @Id")
                            .WithParameters(new { Id = id }))).Select(x => x.ToGroupDto()).FirstOrDefault();

            _logger.LogInformation("end method get group by id");
            return result;
        }

       

        public async Task<GroupDto> CreateGroup(CreateGroupDto dto)
        {
            await ValidateCreateGroup(dto);

            var now = DateTime.UtcNow;
            var createdBy = _sessionService.UserId;

            using (IDbTransaction trans = this.DatabaseConnectService.Connection.BeginTransaction())
            {
                try
                {
                    var group = dto.ToGroup();
                    group.CreatedAt = now;
                    group.CreatedBy = createdBy;

                    await this.DatabaseConnectService.Connection.InsertAsync<Group>(group, x => x.AttachToTransaction(trans));

                    if (dto.Features.Length > 0)
                    {
                        foreach (var item in dto.Features)
                        {
                            var groupFeature = new GroupFeature
                            {
                                FeatureId = item.Id,
                                GroupId = group.Id,
                                CreatedAt = now,
                                ModifiedAt = now
                            };

                            await this.DatabaseConnectService.Connection.InsertAsync<GroupFeature>(groupFeature, x => x.AttachToTransaction(trans));
                        }
                    }

                    trans.Commit();

                    _redisCache.Remove(CacheConst.AllGroup);

                    //return await GetGroupById(group.Id);

                    return null;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw new BusinessException(e.Message, ErrorCode.INTERNAL_SERVER_ERROR);
                }
            }
        }

        #endregion

        #region Private methods

        private async Task ValidateCreateGroup(CreateGroupDto dto)
        {
            if (dto.Name.IsNullOrWhiteSpace() || dto.Description.IsNullOrWhiteSpace())
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }

            var validateGroupName = (await this.DatabaseConnectService.Connection.FindAsync<Group>(x => x
                                    .Where($" bys_group.name = @GroupName ")
                                    .WithParameters(new { GroupName = dto.Name }))).FirstOrDefault();

            if (validateGroupName != null)
                throw new BusinessException("Group name already exists", ErrorCode.GROUP_NAME_EXIST);
        }

        private bool CheckUserGroup(Guid groupId, UserGroup[] userGroup)
        {
            foreach (var item in userGroup)
            {
                if (item.GroupId == groupId)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
