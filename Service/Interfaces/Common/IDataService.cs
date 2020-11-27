using Services.Dtos.Common.InputDtos;
using Services.Dtos.Common;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces.Common
{
    public interface IDataService
    {
        Task<ItemResultDto<AreaDto>> GetAreas(PageDto pageDto, string searchKey);

        Task<ShortFarmingLocationDto[]> GetAllFarmingLocaiton();

        Task<ItemResultDto<FarmingLocationDto>> GetFarmingLocaiton(PageDto pageDto, string searchKey, string location);

        Task<ShortShrimpBreedDto[]> GetAllShrimpBreed();

        Task<ItemResultDto<ShrimpBreedDto>> GetShrimpBreed(PageDto pageDto, string searchKey);

        Task<MasterDataResultDto[]> GetMasterData(string groupsName);

        Task<ShortManagementFactorDto[]> GetAllManagementFactor();

        Task<ItemResultDto<ManagementFactorDto>> GetManagementFactor(PageDto pageDto, string search, string dataType, string FactorGroup);

        Task<ItemResultDto<ShrimpCropDto>> GetShrimpCrop(PageDto pageDto, string searchKey, string farmingLocationId, string shrimpBreedId);

        Task<ShrimpCropResultDto> CreateShrimpCrop(CreateShrimpCropDto createShrimpCropDto);

        Task<ShrimpCropResultDto> GetShrimpCropById(Guid id);

        Task<ShrimpCropResultDto> UpDateShrimpCrop(UpdateShrimpCropDto updateShrimpCropDto);

        Task<Guid> CreateOrUpdateShrimpCropManagementFactor(CreateShrimpCropManagementFactorDto dto);

        Task<bool> CancelShrimpCropManagementFactor(CancelShrimpCropManagementFactorDto dto);

        Task<bool> StopWork(Guid shrimpCropManagementFactorId);
    }
}
