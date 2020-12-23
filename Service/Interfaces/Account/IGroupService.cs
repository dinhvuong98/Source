using Services.Dtos.Account;
using Services.Dtos.Account.InputDtos;
using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Account
{
    public interface IGroupService
    {
        Task<GroupDto[]> GetAllGroup();

        Task<PageResultDto<GroupDto>> FilterGroup(PageDto pageDto, string searchKey);

        Task<GroupDto> GetGroupById(Guid id);

        

        Task<GroupDto> CreateGroup(CreateGroupDto dto);
    }
}
