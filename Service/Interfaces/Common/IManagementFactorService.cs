using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Common
{
    public interface IManagementFactorService
    {
        Task<ShortManagementFactorDto[]> GetAllManagementFactor();

        Task<PageResultDto<ManagementFactorDto>> FilterManagementFactor(PageDto pageDto, string search, string dataType, string FactorGroup);
    }
}
