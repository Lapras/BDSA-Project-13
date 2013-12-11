using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Exceptions
{
    /// <summary>
    /// Exception for when there is no available connection.
    /// </summary>
    class UnavailableConnectionException : Exception
    {
        public UnavailableConnectionException()
            : base() { }

        public UnavailableConnectionException(string message)
            : base(message) { }

        public UnavailableConnectionException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public UnavailableConnectionException(string message, Exception innerException)
            : base(message, innerException) { }

        public UnavailableConnectionException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
