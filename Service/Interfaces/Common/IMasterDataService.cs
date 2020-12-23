using Services.Dtos.Common.InputDtos;
using Services.Dtos.Common;
using System.Threading.Tasks;
using Services.Dtos.Account;

namespace Services.Interfaces.Common
{
    public interface IMasterDataService
    {
        Task<PageResultDto<AreaDto>> FilterAreas(PageDto pageDto, string searchKey);

        Task<ShortFarmingLocationDto[]> GetAllFarmingLocaiton(string locationType);

        Task<PageResultDto<FarmingLocationDto>> FilterFarmingLocation(PageDto pageDto, string searchKey, string location);

        Task<ShortShrimpBreedDto[]> GetAllShrimpBreed();

        Task<PageResultDto<ShrimpBreedDto>> FilterShrimpBreed(PageDto pageDto, string searchKey);

        Task<MasterDataResultDto[]> GetMasterData(string groupsName);

        Task<FeatureDto[]> GetAllFeature();

        Task<AddressDto[]> GetAddressMasterData();
    }
}
