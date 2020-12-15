using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Services.Interfaces.Internal
{
    public interface IStorageProvider
    {
        void CreateImage(Guid id, IFormFile formFile);

        void CreateImage(Guid id, byte[] formFile);

        void Create(Guid id, byte[] data);

        void Create(Guid id, IFormFile formFile);

        void Create(string filePath, IFormFile formFile);

        void Create(string filePath, byte[] data);

        void Create(Guid id, Stream stream);

        void Create(Guid id, string data);

        void Append(Guid id, byte[] data);

        void Overwrite(Guid id, byte[] data);

        byte[] Read(Guid id);

        Task<byte[]> ReadAsync(string id);

        string ReadText(Guid id);

        Stream ReadStream(Guid id);

        Stream WriteStream(Guid id);

        bool Delete(Guid id);

        bool Delete(string id);

        bool DeleteImage(Guid id);
    }
}
