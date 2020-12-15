using Services.Dtos.Common.InputDtos;
using Services.Dtos.Common;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Common
{
    public interface IDataService
    {
        Task<PageResultDto<AreaDto>> FilterAreas(PageDto pageDto, string searchKey);

        Task<ShortFarmingLocationDto[]> GetAllFarmingLocaiton(string locationType);

        Task<PageResultDto<FarmingLocationDto>> FilterFarmingLocation(PageDto pageDto, string searchKey, string location);

        Task<ShortShrimpBreedDto[]> GetAllShrimpBreed();

        Task<PageResultDto<ShrimpBreedDto>> FilterShrimpBreed(PageDto pageDto, string searchKey);

        Task<MasterDataResultDto[]> GetMasterData(string groupsName);

        Task<ShortManagementFactorDto[]> GetAllManagementFactor();

        Task<PageResultDto<ManagementFactorDto>> FilterManagementFactor(PageDto pageDto, string search, string dataType, string FactorGroup);

        Task<ShrimpCropDto[]> GetAllShrimpCrop();

        Task<PageResultDto<ShrimpCropDto>> FilterShrimpCrop(PageDto pageDto, string searchKey, string farmingLocationId, string shrimpBreedId);

        Task<ShrimpCropResultDto> CreateShrimpCrop(CreateShrimpCropDto createShrimpCropDto);

        Task<ShrimpCropResultDto> GetShrimpCropById(Guid id);

        Task<Guid> CreateOrUpdateShrimpCropManagementFactor(CreateShrimpCropManagementFactorDto dto);

        Task<bool> CancelShrimpCropManagementFactor(CancelShrimpCropManagementFactorDto dto);
    }
}
