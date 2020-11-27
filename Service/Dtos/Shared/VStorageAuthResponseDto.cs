using Newtonsoft.Json;
using System;

namespace Services.Dto.Shared
{
    [Serializable]
    public class VStorageAuthResponseDto
    {
        [JsonProperty(PropertyName = "token")]
        public VStorageAuthResponseTokenDto Token { get; set; }

        public string SubjectToken { get; set; }
    }
    [Serializable]
    public class VStorageAuthResponseTokenDto
    {
        [JsonProperty(PropertyName = "expires_at")]
        public string ExpireAt { get; set; }

        [JsonProperty(PropertyName = "catalog")]
        public VStorageAuthResponseCatalogDto[] Catalogs { get; set; }
    }
    [Serializable]
    public class VStorageAuthResponseCatalogDto
    {
        [JsonProperty(PropertyName = "endpoints")]
        public VStorageAuthResponseEndPointDto[] Endpoints { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
    [Serializable]
    public class VStorageAuthResponseEndPointDto
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "interface")]
        public string Interface { get; set; }

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "region_id")]
        public string RegionId { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
