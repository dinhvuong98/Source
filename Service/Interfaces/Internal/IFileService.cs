using Microsoft.AspNetCore.Http;
using Services.Dto.Shared;
using Services.Dtos.Temp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.Internal
{
    public interface IFileService
    {
        Task<TempUploadFileDto> UploadTempFile(IFormFile file);

        Task<FileDescription> DownloadFile(string fileName);
    }
}
