using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Dto.Shared.Inputs;
using Services.Dtos.Response;
using Services.Dtos.Temp;
using Services.Interfaces.Internal;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Utilities.Common.Dependency;
using Utilities.Configuation;

namespace Source.Controllers
{
    [Authorize]
    public class FileController : BaseApiController
    {
        #region Property

        private readonly IFileService _fileService;

        private readonly VStorageConfig _storageConfig = SingletonDependency<IOptions<VStorageConfig>>.Instance.Value;

        #endregion

        #region Constructor

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Upload file
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("api/file")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<ActionResult<BaseResponse<TempUploadFileDto>>> UploadFile(
            [FromForm][Required] UploadFileDto dto
            )
        {
            var response = new BaseResponse<TempUploadFileDto>
            {
                Data = await _fileService.UploadTempFile(dto.File),
                Status = true
            };

            return Ok(response);
        }

        /// <summary>
        /// Download file
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/file/{fileName}")]
        public async Task<FileResult> DownloadFile([FromRoute][Required] string fileName)
        {
            var fileResult = await _fileService.DownloadFile(fileName);
            return AttachmentResult(fileResult);
        }
        #endregion
    }
}
