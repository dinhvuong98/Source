using log4net;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;
using System.Net.Mime;
using System.Web;
using Microsoft.AspNetCore.Cors;
using Services.Dto.Shared;

namespace Source.Controllers
{
    [EnableCors("AllowOrigin")]
    public class BaseApiController : ControllerBase
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected FileResult AttachmentResult(FileDescription file)
        {
            // We can have record in database, but no actual file in file storage
            if (file.Data == null)
            {
                throw new BusinessException("File doesn't exits", Utilities.Enums.ErrorCode.NOT_EXIST);
            }

            var contentDisposition = new ContentDisposition
            {
                FileName = HttpUtility.UrlEncode(file.FileName),
                Inline = false
            };

            Response.Headers.Add("BYS-Content-Disposition", contentDisposition.ToString());

            return File(file.Data, file.ContentType, file.FileName);
        }
    }
}
