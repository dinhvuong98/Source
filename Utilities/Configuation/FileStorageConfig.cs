using System.IO;

namespace Utilities.Configurations
{
    public class FileStorageConfig
    {
        public string Host { get; set; }

        public string LargeImageFolderName => "large";

        public string ThumbImageFolderName => "thumb";

        public string ImageAbsolutePhysicalPath { get; set; }

        public string DocumentAbsolutePhysicalPath { get; set; }

        public string LargeImageAbsolutePhysicalPath => Path.Combine(ImageAbsolutePhysicalPath, LargeImageFolderName);

        public string ThumbImageAbsolutePhysicalPath => Path.Combine(ImageAbsolutePhysicalPath, ThumbImageFolderName);

        public string ImageRelativeRequestPath { get; set; }
    }
}
