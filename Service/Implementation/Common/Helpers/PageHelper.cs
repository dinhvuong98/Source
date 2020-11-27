using Services.Dtos.Common;
using Services.Dtos.Common.InputDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementation.Common.Helpers
{
    public static class PageHelper
    {
        public static void PageValue <T> (ItemResultDto<T> resultDto, PageDto pageDto)
        {
            resultDto.PageIndex = pageDto.Page;
            resultDto.PageSize = pageDto.PageSize;
            resultDto.TotalPages = (int)Math.Ceiling((double)resultDto.TotalCount / (double)resultDto.PageSize);
        }
    }
}
