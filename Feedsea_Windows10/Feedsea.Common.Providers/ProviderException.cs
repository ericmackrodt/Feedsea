using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Providers
{
    public class ProviderException : Exception
    {
        private ExceptionReason exceptionReason;

        public ProviderException() : base() { }

        public ProviderException(string message) : base(message) { }

        public ProviderException(string message, Exception innerException) : base(message, innerException) { }

        public ProviderException(ExceptionReason reason) : this(null, reason) { }

        public ProviderException(string message, ExceptionReason reason)
            : this(message)
        {
            exceptionReason = reason;
        }

        public ProviderException(string message, ExceptionReason reason, Exception inner, string content)
            : this(message, inner)
        {
            exceptionReason = reason;
        }

        public ExceptionReason Reason { get { return exceptionReason; } }
    }

    public enum ExceptionReason
    {
        NoInternetConnection
    }
}
