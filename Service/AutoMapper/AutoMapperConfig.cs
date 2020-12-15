using AutoMapper;
using Services.AutoMapper;

namespace Bys.Services.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DtoMappingProfile());
            });
        }
    }
}