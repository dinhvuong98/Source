
namespace Utilities.Constants
{
    public class CommonConstants
    {
        public static readonly string FACTOR_GROUP = "FactorGroup";
        public static readonly string LOCATION_TYPE = "LocationType";
        public static readonly string FACTOR_DATA_TYPE = "FactorDataType";
        public static readonly string SHRIMP_CROP_FREQUENCY = "ShrimpCropFrequency";
        public static readonly string WORK_STATUS = "WorkStatus";
        public static readonly string NOTIFY_TYPE = "NotifyType";
        public static readonly string CROP_FACTOR_STATUS = "CropFactorStatus";

        public static readonly string DefaultImageExtension = ".png";
        public static readonly string DefaultImageContentType = "image/png";
        public static readonly int DefaultBranchId = 1;
        public static readonly int DefaultCountryId = 1;
        public static readonly string VNCountryCode = "0084";
        public static readonly int DefaultCurrencyId = 100000;
        public static readonly int DefaultCompanyId = 1;
        public static readonly decimal DefaullExchangeRate = 1;
        public static readonly bool DefaultIsTransferred = false;
        public static readonly bool DefaultActiveCheck = true;
        public static readonly int MaxProductDepartmentDisplay = 3;
        public static readonly int MaxResultCount = 100;
    }

    public static class CacheConst
    {
        public static readonly string MasterData = "MasterData";
        public static readonly string AllUser = "AllUser";
        public static readonly string AllFarmingLocation = "AllFarmingLocation";
        public static readonly string AllShrimpCrop = "AllShrimpCrop";

    }

    public static class RedisCacheKey
    {
        public static readonly string KEY_VSTORAGE_TOKEN = "";
        public static readonly string VStorageExpiredMinutes = "";
    }

    public static class RedisCacheTime
    {
        public const int VStorageExpiredMinutes = 55;
    }
}
