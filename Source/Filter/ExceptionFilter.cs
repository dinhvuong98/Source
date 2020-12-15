using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Dtos.Response;
using System.Net;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Source.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            BaseResponse<bool?> apiErrorResult;

            if (exception.GetType() == typeof(BusinessException) || exception.GetType().BaseType == typeof(BusinessException))
            {
                // handle bussiness exception
                apiErrorResult = new BaseResponse<bool?>(new Error(((BusinessException)exception).Message, ((BusinessException)exception).ErrorCode, ((BusinessException)exception).ErrorData));

                // Result asigned to a result object but in destiny the response is empty. This is a known bug of .net core 1.1
                //It will be fixed in .net core 1.1.2. See https://github.com/aspnet/Mvc/issues/5594 for more information
                context.Result = new BadRequestObjectResult(apiErrorResult);

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                apiErrorResult = new BaseResponse<bool?>(new Error(context.Exception.Message, ErrorCode.INTERNAL_SERVER_ERROR));

                // Result asigned to a result object but in destiny the response is empty. This is a known bug of .net core 1.1
                //It will be fixed in .net core 1.1.2. See https://github.com/aspnet/Mvc/issues/5594 for more information
                context.Result = new BadRequestObjectResult(apiErrorResult);

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            }


            context.ExceptionHandled = true;
        }
    }
}
