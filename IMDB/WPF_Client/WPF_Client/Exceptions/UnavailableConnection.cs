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
    class UnavailableConnection : Exception
    {
        public UnavailableConnection()
            : base() { }

        public UnavailableConnection(string message)
            : base(message) { }

        public UnavailableConnection(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public UnavailableConnection(string message, Exception innerException)
            : base(message, innerException) { }

        public UnavailableConnection(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
