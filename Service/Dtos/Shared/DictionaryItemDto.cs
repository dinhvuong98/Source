using System;

namespace Services.Dto.Shared
{
    [Serializable]
    public class DictionaryItemDto : DictionaryItemDto<string>
    {
        public DictionaryItemDto()
        {
        }

        public DictionaryItemDto(string key, string value) : base(key, value)
        {
        }
    }
    [Serializable]
    public class DictionaryItemDto<TKey> : DictionaryItemDto<TKey, string>
    {
        public DictionaryItemDto()
        {
        }

        public DictionaryItemDto(TKey key, string value) : base(key, value)
        {
        }
    }
    [Serializable]
    public class DictionaryItemDto<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        public string DisplayText { get; set; }

        public string Code { get; set; }

        public string TypeGroup { get; set; }

        public DictionaryItemDto()
        {
        }

        public DictionaryItemDto(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
