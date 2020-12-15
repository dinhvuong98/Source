using Services.Dto.Shared;
using Services.Implementation.Internal.Helpers;
using Services.Interfaces;
using Services.Interfaces.Internal;
using System;
using Utilities.Exceptions;

namespace Services.Implementation
{
    public class ImageService : IImageService
    {
        private IStorageProvider _storageProvider;

        public ImageService(IStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public ImageUrlDto UploadImage(FileDescription imageFile)
        {
            if (!imageFile.IsImageFile())
            {
                throw new BusinessException("Your Upload is not a Images!");
            }

            var fileName = ImageUrlHelper.NewImageFileName();

            _storageProvider.CreateImage(ImageUrlHelper.GetFileGuid(fileName).GetValueOrDefault(), imageFile.Data);

            return ImageUrlHelper.ToImageUrl(fileName);
        }

        public void DeleteImage(Guid guid)
        {
            _storageProvider.DeleteImage(guid);
        }

        public ImageUrlDto GetImageUrl(Guid guid)
        {
            return ImageUrlHelper.ToImageUrl(guid);
        }

    }
}
