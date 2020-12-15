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
using Services.Dtos.Accounts.InputDto;

namespace Services.Implementation.Account
{
    public class UserService : BaseService, IUserService
    {
        #region Properties
        private readonly ITokenService _tokenService;
        private readonly ISessionService _sessionService;
        #endregion

        #region Constructor
        public UserService(DatabaseConnectService databaseConnectService, ITokenService tokenService, ISessionService sessionService): base(databaseConnectService)
        {
            _tokenService = tokenService;
            _sessionService = sessionService;
            DatabaseConnectService = databaseConnectService;
        }
        #endregion

        #region Public methods
        public async Task<LoginResultDto> Login(LoginDto loginDto)
        {
            var user = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                        .Include<Data.Entity.Account.Account>(join => join.LeftOuterJoin())
                        .Where($"bys_account.username = @UserName")
                        .WithParameters(new { UserName = loginDto.UserName}))).FirstOrDefault();

            if(user != null)
            {
                if(user.Account.FirstOrDefault().Password == loginDto.Password)
                {
                    if(user.Status == AccountStatus.Active.ToString())
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
                        throw new BusinessException("Mật khẩu này đang được sử dụng", ErrorCode.WRONG_PASSWORD);
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

        
        #endregion
    }
}
