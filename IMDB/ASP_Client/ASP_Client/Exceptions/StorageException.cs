using System;

namespace ASP_Client.Exceptions
{
    /// <summary>
    /// Exception for when there is an exception in the Storage.
    /// </summary>
    class StorageException : Exception
    {
        public StorageException()
            : base() { }

        public StorageException(string message)
            : base(message) { }

        public StorageException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public StorageException(string message, Exception innerException)
            : base(message, innerException) { }

        public StorageException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
