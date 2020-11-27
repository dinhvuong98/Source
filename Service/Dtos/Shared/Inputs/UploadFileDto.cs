using System;
using Microsoft.AspNetCore.Http;

namespace Services.Dto.Shared.Inputs
{
    public class UploadFileDto
    {
        public IFormFile File { get; set; }
    }
}
