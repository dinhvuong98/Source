using Data.Entity.Account;
using Services.Dtos.Account;
using Services.Dtos.Account.InputDtos;
using Services.Dtos.Accounts.InputDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Account
{
    public interface IUserService
    {
        Task<LoginResultDto> Login(LoginDto loginDto);

        Task<UserDto[]> GetAllUser();

        Task<bool> ReadNotification();

        Task<bool> ChangePassword(ChangePassDto dto);

    }
}
