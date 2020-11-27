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

        public async Task<UserDto[]> GetAllUser()
        {
            var result = (await this.DatabaseConnectService.Connection.FindAsync<User>(x => x
                        .Where($"bys_user.status = @Status")
                        .WithParameters(new { Status = AccountStatus.Active.ToString() }))).Select(x => x.ToUserDto()).ToArray();

            var x = _sessionService.UserId;

            return result;
        }

        #endregion
    }
}
