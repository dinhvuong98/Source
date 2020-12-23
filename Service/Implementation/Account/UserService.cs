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
using Data.Entity.Common;
using Services.Implementation.Common.Helpers;
using Dapper;
using Services.Interfaces.RedisCache;
using Utilities.Constants;

namespace Services.Implementation.Account
{
    public class UserService : BaseService, IUserService
    {
        #region Properties
        private readonly ITokenService _tokenService;
        private readonly ISessionService _sessionService;
        private readonly ICacheProvider _redisCache;
        private readonly ILogger<UserService> _logger;
        #endregion

        #region Constructor
        public UserService(DatabaseConnectService databaseConnectService, ITokenService tokenService, ISessionService sessionService, ICacheProvider redisCache, ILogger<UserService> logger) : base(databaseConnectService)
        {
            _tokenService = tokenService;
            _sessionService = sessionService;
            _redisCache = redisCache;
            _logger = logger;
            DatabaseConnectService = databaseConnectService;
        }
        #endregion

        #region Public methods
        public async Task<LoginResultDto> Login(LoginDto loginDto)
        {
            var user = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                        .Include<Data.Entity.Account.Account>(join => join.LeftOuterJoin())
                        .Where($"bys_account.username = @UserName")
                        .WithParameters(new { UserName = loginDto.UserName }))).FirstOrDefault();

            if (user != null)
            {
                if (user.Accounts.FirstOrDefault().Password == loginDto.Password)
                {
                    if (user.Status == AccountStatus.Active.ToString())
                    {
                        var result = user.ToLoginResultDto();
                        result.JwtToken = _tokenService.GenerateJwtToken(user.ToAccountDto());

                        return result;
                    }
                    throw new BusinessException("Tài khoản chưa được kích hoạt", ErrorCode.ACCOUNT_NOT_ACTIVE);
                }
                throw new BusinessException("Mật khẩu không đúng", ErrorCode.WRONG_PASSWORD);
            }
            throw new BusinessException("Tên tài khoản không đúng", ErrorCode.WRONG_USERNAME);
        }

        public async Task<bool> ChangePassword(ChangePassDto dto)
        {
            var account = (await DatabaseConnectService.Connection.FindAsync<Data.Entity.Account.Account>(x => x
                                .Include<User>(join => join.InnerJoin())
                                .Where($"bys_account.user_id = @Id")
                                .WithParameters(new { Id = _sessionService.UserId }))).FirstOrDefault();

            if (account != null)
            {
                if (account.User.Status == AccountStatus.Active.ToString())
                {
                    if (account.Password == dto.OldPassword)
                    {
                        if (account.Password != dto.NewPassword)
                        {
                            account.Password = dto.NewPassword;
                            await DatabaseConnectService.Connection.UpdateAsync(account);

                            return true;
                        }
                        throw new BusinessException("Mật khẩu này đang được sử dụng", ErrorCode.FAIL);
                    }
                    throw new BusinessException("Mật khẩu cũ không đúng", ErrorCode.WRONG_PASSWORD);
                }
                throw new BusinessException("Tài khoản chưa được kích hoạt", ErrorCode.INVALID_STATUS);
            }

            return false;
        }

        public async Task<UserDto[]> GetAllUser()
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                        .Where($"bys_user.status = @Status")
                        .WithParameters(new { Status = AccountStatus.Active.ToString() })))
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(x => x.ToUserDto()).ToArray();

            return result;
        }

        public async Task<bool> ReadNotification()
        {
            var user = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                        .Where($"bys_user.id = @UserId")
                        .WithParameters(new { UserId = _sessionService.UserId }))).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            user.LastTimeReadNotification = DateTime.UtcNow;

            await this.DatabaseConnectService.Connection.UpdateAsync<User>(user);

            return true;
        }

        public async Task<PageResultDto<DetailUserResultDto>> FilterUser(PageDto pageDto, string searchKey, string groupId)
        {
            _logger.LogInformation("start method filter user");

            var query = new StringBuilder();

            query.Append("@SearchKey is null OR (bys_account.username LIKE @SearchKey OR bys_user.fullname LIKE @SearchKey OR bys_user.email LIKE @SearchKey) ");
            query.Append("AND (@GroupId is null OR (bys_users_groups.group_id = @GroupId)) ");
            query.Append("AND bys_user.status = @Status");

            var param = new
            {
                SearchKey = searchKey.FormatSearch(),
                GroupId = groupId,
                Status = AccountStatus.Active.ToString()
            };

            var count = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                         .Include<Data.Entity.Account.Account>(j => j.InnerJoin())
                         .Include<UserGroup>(j => j.LeftOuterJoin())
                         .Where($"{query}")
                         .WithParameters(param))).Distinct().Count();

            var items = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                         .Include<Data.Entity.Account.Account>(j => j.InnerJoin())
                         .Include<UserGroup>(j => j.LeftOuterJoin())
                         .Include<Group>(j => j.LeftOuterJoin())
                         .Where($"{query}")
                         .WithParameters(param)))
                         .OrderByDescending(x => x.CreatedAt)
                         .Skip(pageDto.Page * pageDto.PageSize)
                         .Take(pageDto.PageSize)
                         .Select(x => x.ToDetailUserDto()).ToArray();

            var result = new PageResultDto<DetailUserResultDto>
            {
                Items = items,
                TotalCount = count,
                PageIndex = pageDto.Page,
                PageSize = pageDto.PageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageDto.PageSize),
            };

            _logger.LogInformation("end method filter user");

            return result;
        }

        

        public async Task<DetailUserResultDto> GetDetailUserById(Guid id)
        {
            _logger.LogInformation("start method get detail by id");

            var user = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                        .Include<Data.Entity.Account.Account>(j => j.InnerJoin())
                        .Include<UserGroup>(j => j.LeftOuterJoin())
                        .Include<Group>(j => j.LeftOuterJoin())
                        .Where($"bys_user.id = @UserId")
                        .WithParameters(new { UserId = id }))).FirstOrDefault();

            if (user == null) return null;

            var result = user.ToDetailUserDto();

            if (user.ProvinceId.HasValue)
            {
                result.Province = (await this.DatabaseConnectService.Connection.FindAsync<Province>(x => x
                                    .Where($"bys_province.id = @ProvinecId")
                                    .WithParameters(new { ProvinecId = user.ProvinceId }))).FirstOrDefault().ToDictionaryItemDto();
            }

            if (user.DistrictId.HasValue)
            {
                result.District = (await this.DatabaseConnectService.Connection.FindAsync<District>(x => x
                                    .Where($"bys_district.id = @DistrictId")
                                    .WithParameters(new { DistrictId = user.DistrictId }))).FirstOrDefault().ToDictionaryItemDto();
            }

            if (user.CommuneId.HasValue)
            {
                result.Commune = (await this.DatabaseConnectService.Connection.FindAsync<Commune>(x => x
                                    .Where($"bys_commune.id = @CommuneId")
                                    .WithParameters(new { CommuneId = user.CommuneId }))).FirstOrDefault().ToDictionaryItemDto();
            }

            _logger.LogInformation("end method get detail user by id");

            return result;
        }

        public async Task<DetailUserResultDto> CreateOrUpdateUser(CreateOrUpdateUserDto dto)
        {
            ValidateCreateOrUpDateUser(dto);

            var checkAccount = new CheckAccountDto
            {
                UserName = dto.Id == null ? dto.UserName : null,
                Email = dto.Email
            };

            await CheckAccount(checkAccount);

            var now = DateTime.UtcNow;

            using (IDbTransaction trans = this.DatabaseConnectService.Connection.BeginTransaction())
            {
                try
                {
                    if (dto.Id == null)
                    {
                        var user = dto.ToUser();
                        user.CreatedAt = now;
                        user.CreatedBy = _sessionService.UserId;
                        await this.DatabaseConnectService.Connection.InsertAsync<User>(user, x => x.AttachToTransaction(trans));

                        var account = dto.ToAccount();
                        account.CreatedAt = now;
                        account.CreatedBy = _sessionService.UserId;
                        account.UserId = user.Id;
                        await this.DatabaseConnectService.Connection.InsertAsync<Data.Entity.Account.Account>(account, x => x.AttachToTransaction(trans));

                        if (dto.Group != null)
                        {
                            var userGroup = dto.Group.ToUserGroup();
                            userGroup.UserId = user.Id;
                            userGroup.CreatedAt = now;

                            await this.DatabaseConnectService.Connection.InsertAsync<UserGroup>(userGroup, x => x.AttachToTransaction(trans));
                        }
                        dto.Id = user.Id;
                    }
                    else
                    {
                        var user = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                                    .AttachToTransaction(trans)
                                    .Include<UserGroup>(join => join.LeftOuterJoin())
                                    .Where($"bys_user.id = @UserId")
                                    .WithParameters(new { UserId = dto.Id }))).FirstOrDefault();

                        var userGroupAlive = user.UserGroups.Where(x => x.Status == EntityStatus.Alive.ToString()).FirstOrDefault();

                        var userInsert = dto.ToUser();
                        userInsert.ModifiedAt = now;
                        userInsert.UpdateBy = _sessionService.UserId;
                        userInsert.CreatedAt = user.CreatedAt;
                        userInsert.CreatedBy = user.CreatedBy;

                        await this.DatabaseConnectService.Connection.UpdateAsync<User>(userInsert, x => x.AttachToTransaction(trans));

                        if (dto.Group != null)
                        {
                            var userGroupInsert = dto.Group.ToUserGroup();
                            userGroupInsert.UserId = user.Id;
                            userGroupInsert.CreatedAt = now;
                            await this.DatabaseConnectService.Connection.InsertAsync<UserGroup>(userGroupInsert, x => x.AttachToTransaction(trans));

                            if (userGroupAlive != null)
                            {
                                userGroupAlive.Status = EntityStatus.Delete.ToString();
                                await this.DatabaseConnectService.Connection.UpdateAsync<UserGroup>(userGroupAlive, x => x.AttachToTransaction(trans));
                            }
                        }
                    }

                    trans.Commit();

                    _redisCache.Remove(CacheConst.AllUser);

                    return await GetDetailUserById((Guid)dto.Id);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw new BusinessException(e.Message, ErrorCode.INTERNAL_SERVER_ERROR);
                }
            }
        }

        public async Task<bool> CheckAccount(CheckAccountDto dto)
        {
            var query = new StringBuilder();

            query.Append("bys_user.email = @Email");
            if (dto.UserName != null)
            {
                query.Append(" OR bys_account.username = @UserName");
            }

            var param = new
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var user = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                        .Include<Data.Entity.Account.Account>(j => j.LeftOuterJoin())
                        .Where($"{query}")
                        .WithParameters(param))).FirstOrDefault();

            return user != null ? (user.Email == dto.Email ? true : throw new BusinessException("Username or email exists", ErrorCode.USERNAME_EMAIL_EXIST)) : true;
        }

        public async Task<bool> DeleteUser(Guid[] ids)
        {
            ValidateDeleteUser(ids);

            var query = new StringBuilder();
            query.Append("UPDATE bys_user SET status = @Status WHERE id IN @Ids");

            var param = new
            {
                Status = AccountStatus.Delete.ToString(),
                Ids = ids
            };

            var trans = this.DatabaseConnectService.Connection.BeginTransaction();

            try
            {
                await this.DatabaseConnectService.Connection.ExecuteAsync(query.ToString(), param, trans);

                trans.Commit();

                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                throw new BusinessException(e.Message, ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        #endregion

        #region Private methods

        private void ValidateCreateOrUpDateUser(CreateOrUpdateUserDto dto)
        {
            if ((dto.Id == null && dto.Password.IsNullOrWhiteSpace())
                || dto.UserName.IsNullOrWhiteSpace()
                || dto.FullName.IsNullOrWhiteSpace()
                || dto.Email.IsNullOrWhiteSpace()
                )
            {
                throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
            }
        }

        private void ValidateDeleteUser(Guid[] ids)
        {
            foreach (var item in ids)
            {
                if (item == null)
                {
                    break;
                    throw new BusinessException("Invalid parameter!", ErrorCode.INVALID_PARAMETER);
                }
            }
        }
       
        #endregion
    }
}
