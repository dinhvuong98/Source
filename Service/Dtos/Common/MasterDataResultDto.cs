using Services.Dto.Shared;
using System;

namespace Services.Dtos.Common
{
    [Serializable]
    public class MasterDataResultDto
    {
        public string GroupName { get; set; }

        public DictionaryItemDto[] Childs { get; set; }
    }
}
