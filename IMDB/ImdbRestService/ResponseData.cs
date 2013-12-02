using System.Net;

namespace ImdbRestService
{
    public class ResponseData
    {
        public ResponseData(string message, HttpStatusCode httpStatusCode)
        {
            Message = message;
            HttpStatusCode = httpStatusCode;
        }

        public string Message { get; private set; }
        public HttpStatusCode HttpStatusCode { get; private set; }
    }
}
