using System.Net;

namespace ImdbRestService
{
    /// <summary>
    /// ResponseData is a class used for storing the message and http status code in one
    /// object instead of having them as a variable message and status
    /// </summary>
    public class ResponseData
    {
        public string Message { get; private set; }
        public HttpStatusCode HttpStatusCode { get; private set; }

        public ResponseData(string message, HttpStatusCode httpStatusCode)
        {
            Message = message;
            HttpStatusCode = httpStatusCode;
        }
    }
}
