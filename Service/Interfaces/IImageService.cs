using Services.Dto.Shared;
using System;

namespace Services.Interfaces
{
    public interface IImageService
    {
        ImageUrlDto UploadImage(FileDescription imageFile);

        void DeleteImage(Guid guid);

        ImageUrlDto GetImageUrl(Guid guid);
    }
}
