using Microsoft.AspNetCore.Http;

namespace Services.Dto.Shared.Inputs
{
    public class ImageFileUploadDto
    {
        public IFormFile File { get; set; }
    }
}
