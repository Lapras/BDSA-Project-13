using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DtoSubsystem;

namespace ImdbRestService
{
    /// <summary>
    /// Class handling incoming REST requests
    /// </summary>
    public class ImdbRestWebServerAdapter
    {
        /// <summary>
        /// Listener for http requests
        /// </summary>
        private readonly HttpListener _listener;

        private readonly IPersistenceFacade _persistenceFacade;

        /// <summary>
        /// Message returned to the client
        /// </summary>
        public ResponseData ResponseData { get; private set; }

        public ImdbRestWebServerAdapter() : this(new PersistenceFacade())
        {
            
        }

        // give the base address as argument
        public ImdbRestWebServerAdapter(IPersistenceFacade persistenceFacade)
        {
            _listener = new HttpListener();
            _persistenceFacade = persistenceFacade;
        }

        /// <summary>
        /// Start the web server
        /// </summary>
        /// <param name="uri">Adress to connect to</param>
        public async void Start(string uri)
        {
            _listener.Prefixes.Add(uri);
            _listener.Start();
            while (true)
            {
                try
                {
                    // get next request
                    var context = await _listener.GetContextAsync();
                    // start a task for each request given the context of the request
                    await Task.Run(() => ProcessRequest(new Request(context.Request), new Response(context.Response)));
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (InvalidOperationException)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
            _listener.Prefixes.Clear();
        }

        /// <summary>
        /// Process a request
        /// </summary>
        /// <param name="response">Response we're sending back to the client</param>
        /// <param name="request">Request we receive from the server</param>
        public async void ProcessRequest(IRequest request, IResponse response)
        {
            try
            {
                ResponseData = new ResponseData("Error: page not found", HttpStatusCode.NotFound);

                // check for GET method
                if (request.HttpMethod == "GET")
                {
                    await GetResponse(request.RawUrl);
                }

                // check for POST method
                else if (request.HttpMethod == "POST")
                {
                    await PostRequest(request.RawUrl, request.InputStream);
                }

                if (ResponseData == null)
                {
                    ResponseData = new ResponseData("Database is unavailable",HttpStatusCode.InternalServerError);
                }
                // now we need to prepare the response. First we must encode it into a byte array
                // with the std. web encoding iso-8859-1
                byte[] result = Encoding.GetEncoding("iso-8859-1").GetBytes(ResponseData.Message);
                // then we must set the length of the response
                response.ContentLength64 = result.Length;
                response.StatusCode = (int)ResponseData.HttpStatusCode;
                // and the write the response back to the requester
                using (var stream = response.OutputStream)
                {
                    await stream.WriteAsync(result, 0, result.Length);
                }
            }
            catch (Exception ex) // not a good practice to use the general exception in a catch :(
            {
                Console.WriteLine("Request error: " + ex);
            }
        }

        /// <summary>
        /// Method process POST requests and hand the content to proper mappers
        /// </summary>
        /// <param name="rawUrl">Incoming REST request</param>
        /// <param name="inputStream">Incoming data to be posted</param>
        /// <returns></returns>
        private async Task PostRequest(string rawUrl, Stream inputStream)
        {
            // split the request URL by '/'
            var path = SplitUrl(rawUrl);
            // in a post (and put) method we need to fetch the body of the HTTP request
            // read the input stream from the request
            var stream = new StreamReader(inputStream);
            var rawBody = stream.ReadToEnd();

            // now we can use the ParseQueryString in the HttpUtility
            //NameValueCollection namedValues = HttpUtility.ParseQueryString(bodyDecoded ?? "");
            // that will create a map of name and values
            if (path.Count > 0)
            {
                // checks for a handler able to handle that specific path
                //    var handler = handlers.FirstOrDefault(x => x.CanHandle(path[0]));

                rawBody = rawBody.Replace("k__BackingField", "");
                rawBody = rawBody.Replace("<", "");
                rawBody = rawBody.Replace(">", "");

             //   var url = path.Skip(1).ToList();
              //  url.Add(rawBody);
                Dto expected = null;
                var key = path.First();

                switch (key)
                {
                    case "user":
                        if (path[1].Equals("registration"))
                        {
                            expected = new RegistrationDto();
                        }
                        else
                        {
                            expected = new LoginDto();
                        }
                      
                        break;
                    case "movies":
                        expected = new RatingDto();
                        break;
                    case "test":
                        expected = new TestDto();
                        break;
                }

                ResponseData = await _persistenceFacade.Post(rawBody, expected);
            }

            else
            {
                ResponseData = new ResponseData("Error creating Category: no id", HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Method process GET requests and hand the content to proper mappers
        /// </summary>
        /// <param name="rawUrl">Incoming REST request</param>
        /// <returns></returns>
        private async Task GetResponse(string rawUrl)
        {
            // split the request URL by '/'
            var path = SplitUrl(rawUrl);

            // if we have anything int the request
            if (path.Count > 0)
            {
                Dto expected = null;
                var key = path.First();

                switch (key)
                {
                    case "movies":
                        if (path[1].StartsWith("?"))
                        {
                            key = path[1].Substring(1).Split(new[] {'='})[1];
                            key = HttpUtility.UrlDecode(key); // removes %20 and adds whitespace instead

                            expected = new MovieDto();
                        }
                        else
                        {
                            key = path[1];
                            expected = new MovieDetailsDto();
                        }
                        break;
                    case "person":
                        if (path[1].StartsWith("?"))
                        {
                            key = path[1].Substring(1).Split(new[] { '=' })[1];
                            key = HttpUtility.UrlDecode(key); // removes %20 and adds whitespace instead
                            expected = new PersonDto();
                        }
                        else
                        {
                            key = path[1];
                            expected = new PersonDetailsDto();
                        }
                        break;
                    case "test":
                        expected = new TestDto();
                        break;
                }

                ResponseData = await _persistenceFacade.Get(key, expected);
            }
        }

        /// <summary>
        /// Method to split urls by '/' characters
        /// </summary>
        /// <param name="rawUrl">Incoming REST request </param>
        /// <returns>List of splitted parts</returns>
        private static List<string> SplitUrl(string rawUrl)
        {
            return rawUrl.Split(
                new[] { '/' },
                StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        
        /// <summary>
        /// Interface for our response data stream
        /// </summary>
        public interface IResponse
        {
            int ContentLength64 { set; }
            int StatusCode { set; }
            Stream OutputStream { get; }
        }

        /// <summary>
        /// Class representing the stream we use to send messages to the client
        /// </summary>
        public class Response : IResponse
        {
            private readonly HttpListenerResponse _response;

            public Response(HttpListenerResponse response)
            {
                _response = response;
            }

            public int ContentLength64
            {
                set { _response.ContentLength64 = value; }
            }

            public int StatusCode
            {
                set { _response.StatusCode = value; }
            }

            public Stream OutputStream
            {
                get { return _response.OutputStream; }
            }
        }

        /// <summary>
        /// Interface for the stream for incoming messages
        /// </summary>
        public interface IRequest
        {
            string HttpMethod { get; }
            string RawUrl { get; }
            Stream InputStream { get; }
        }

        /// <summary>
        /// Class representing incoming message stream
        /// </summary>
        public class Request : IRequest
        {
            private readonly HttpListenerRequest _request;

            public Request(HttpListenerRequest request)
            {
                _request = request;
            }

            public string HttpMethod
            {
                get { return _request.HttpMethod; }
            }

            public string RawUrl
            {
                get { return _request.RawUrl; }
            }

            public Stream InputStream
            {
                get { return _request.InputStream; }
            }
        }
    }
}