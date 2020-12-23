using Services.Dtos.Account;
using Services.Dtos.Account.InputDtos;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Account
{
    public interface IUserService
    {
        Task<LoginResultDto> Login(LoginDto loginDto);

        Task<UserDto[]> GetAllUser();

        Task<bool> ReadNotification();

        Task<bool> ChangePassword(ChangePassDto dto);

        Task<PageResultDto<DetailUserResultDto>> FilterUser(PageDto pageDto, string searchKey, string groupId);

        Task<DetailUserResultDto> GetDetailUserById(Guid id);

        Task<DetailUserResultDto> CreateOrUpdateUser(CreateOrUpdateUserDto dto);

        Task<bool> CheckAccount(CheckAccountDto dto);

        Task<bool> DeleteUser(Guid[] ids);
    }
}
