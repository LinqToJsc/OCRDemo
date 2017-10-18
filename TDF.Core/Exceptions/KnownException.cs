using System;
using System.Runtime.Serialization;

namespace TDF.Core.Exceptions
{
    public class KnownException : TDFException
    {
        public int ErrorCode { get; set; }

        public KnownException()
        {

        }

        public KnownException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public KnownException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        protected KnownException(SerializationInfo
    info, StreamingContext context)
            : base(info, context)
        {
        }

        public KnownException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
