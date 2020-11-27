using System;
using Utilities.Enums;

namespace Utilities.Exceptions
{
    public class BusinessException : Exception
    {
        public ErrorCode ErrorCode { get; set; }

        public object ErrorData { get; set; }

        public BusinessException() : base("Business exception")
        {
        }

        public BusinessException(string errorMessage) : base(errorMessage)
        {
        }

        public BusinessException(string errorMessage, ErrorCode errorCode) : base(errorMessage)
        {
            this.ErrorCode = errorCode;
        }

        public BusinessException(string errorMessage, ErrorCode errorCode, object errorData) : base(errorMessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorData = errorData;
        }
    }
}
