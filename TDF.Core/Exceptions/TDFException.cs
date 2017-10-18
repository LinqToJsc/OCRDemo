using System;
using System.Runtime.Serialization;

namespace TDF.Core.Exceptions
{
    public class TDFException : Exception
    {
        public TDFException()
        {

        }

        public TDFException(string message) : base(message)
        {

        }

        public TDFException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        protected TDFException(SerializationInfo
    info, StreamingContext context)
            : base(info, context)
        {
        }

        public TDFException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
