using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Quartz.Util;
using Services.Dto.Shared;
using System;
using System.IO;
using Utilities.Common;
using Utilities.Common.Dependency;
using Utilities.Configurations;
using Utilities.Constants;

namespace Services.Implementation.Internal.Helpers
{
    public static class ImageUrlHelper
    {
        private static readonly IUrlHelper UrlHelper = SingletonDependency<IUrlHelper>.Instance;

        private static readonly FileStorageConfig FileStorageConfig = SingletonDependency<IOptions<FileStorageConfig>>.Instance.Value;

        public static ImageUrlDto ToImageUrl(Guid? fileGuid, string extension)
        {
            var fileName = fileGuid.GetFileNameWithExtension(extension);

            return ToImageUrl(fileName);
        }

        public static ImageUrlDto ToImageUrl(Guid? fileGuid)
        {
            return ToImageUrl(fileGuid, CommonConstants.DefaultImageExtension);
        }

        public static ImageUrlDto ToImageUrl(this string fileName)
        {
            var name = GetFileGuid(fileName).GetValueOrDefault();
            return fileName.IsNullOrWhiteSpace()
                ? null
                : new ImageUrlDto
                {
                    Guid = name,
                    //LargeSizeUrl = LargeSizeUrl(string.Format("{0}.png", name)),
                    //ThumbSizeUrl = ThumbSizeUrl(string.Format("{0}.png", name)),
                };
        }

        //public static string LargeSizeUrl(string fileName)
        //{
        //    if (fileName.IsNullOrWhiteSpace())
        //    {
        //        return string.Empty;
        //    }

        //    return UrlHelper.AbsoluteContent(string.Join("/", FileStorageConfig.ImageRelativeRequestPath, FileStorageConfig.LargeImageFolderName, fileName));
        //}

        //public static string ThumbSizeUrl(string fileName)
        //{
        //    if (fileName.IsNullOrWhiteSpace())
        //    {
        //        return string.Empty;
        //    }
        //    return UrlHelper.AbsoluteContent(string.Join("/", FileStorageConfig.ImageRelativeRequestPath, FileStorageConfig.ThumbImageFolderName, fileName));
        //}

        public static string GetFileNameWithExtension(this Guid? fileGuild, string extension)
        {
            return fileGuild == null ? string.Empty : GetFileNameWithExtension(fileGuild.Value, extension);
        }

        public static string GetFileNameWithExtension(this Guid fileGuild, string extension)
        {
            return fileGuild + extension;
        }

        public static string NewImageFileName()
        {
            return GuidHelper.Create() + ".png";
        }

        public static Guid? GetFileGuid(string filename)
        {
            return GuidHelper.Parse(Path.GetFileNameWithoutExtension(filename));
        }

        public static string ToImageFileName(this Guid guid)
        {
            return guid + CommonConstants.DefaultImageExtension;
        }
    }
}
