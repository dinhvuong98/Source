using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation.Common
{
    public interface IShrimpCropService
    {
        Task<ShrimpCropDto[]> GetAllShrimpCrop();

        Task<PageResultDto<ShrimpCropDto>> FilterShrimpCrop(PageDto pageDto, string searchKey, string farmingLocationId, string shrimpBreedId);

        Task<ShrimpCropResultDto> CreateShrimpCrop(CreateShrimpCropDto createShrimpCropDto);

        Task<ShrimpCropResultDto> GetShrimpCropById(Guid id);

        Task<Guid> CreateOrUpdateShrimpCropManagementFactor(CreateShrimpCropManagementFactorDto dto);

        Task<bool> CancelShrimpCropManagementFactor(CancelShrimpCropManagementFactorDto dto);
    }
}
