using Domain.Enums;

using System;
using System.Runtime.Serialization;

namespace Domain.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public ApiExceptionType ExceptionType { get; }

        public ApiException(ApiExceptionType exceptionType, string message)
            : base(message)
        {
            ExceptionType = exceptionType;
        }

        public ApiException(ApiExceptionType exceptionType, string message, Exception innerException)
            : base(message, innerException)
        {
            ExceptionType = exceptionType;
        }

        // Constructor de serialización
        protected ApiException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        // Método de serialización
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
