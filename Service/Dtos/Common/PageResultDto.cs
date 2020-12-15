using System;

namespace Services.Dtos.Common
{
    public class PageResultDto<T>
    {
        public Object ExtraData { get; set; }

        public bool HasNextPage => PageIndex + 1 < TotalPages;

        public bool HasPreviousPage => PageIndex > 0;

        public T[] Items { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}
