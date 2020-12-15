using AutoMapper;
using Data.Entity.Common;

using Services.Dtos.Temp;

namespace Services.AutoMapper
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<TempUploadFile, TempUploadFileDto>();
            CreateMap<TempUploadFileDto, TempUploadFile>();
        }
    }
}