using System;

namespace WPF_Client.Exceptions
{
    /// <summary>
    /// Exception for when there is an exception in the REST service strategy.
    /// </summary>
    class RESTserviceException : Exception
    {
        public RESTserviceException()
            : base() { }

        public RESTserviceException(string message)
            : base(message) { }

        public RESTserviceException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public RESTserviceException(string message, Exception innerException)
            : base(message, innerException) { }

        public RESTserviceException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
