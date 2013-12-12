using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ImdbRestService.Handlers;

namespace ImdbRestService
{
    /// <summary>
    /// Class handling incoming REST requests
    /// </summary>
    public class ImdbRestWebServer
    {
        /// <summary>
        /// Listener for http requests
        /// </summary>
        private readonly HttpListener _listener;
        /// <summary>
        /// Message returned to the client
        /// </summary>
        public ResponseData ResponseData { get; private set; }

        // give the base address as argument
        public ImdbRestWebServer()
        {
            _listener = new HttpListener();
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
                    await Task.Run(() => ProcessRequest(context));
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
        /// <param name="context">Received data</param>
        /// <param name="handlers">Handlers to process received data</param>
        private async void ProcessRequest(HttpListenerContext context, List<IHandler> handlers = null)
        {
            handlers = handlers ?? new List<IHandler>(new IHandler[] { new MovieHandler(), new ProfileHandler(), new PersonHandler() });
   
            // you properly need to split this into a number of methods to make a readable
            // solution :-)
            try
            {
                ResponseData = new ResponseData("Error: page not found", HttpStatusCode.NotFound);

                if (context.Request.HttpMethod == "GET")
                {
                    await GetResponse(context, handlers);
                }

                // check for POST method
                else if (context.Request.HttpMethod == "POST")
                {
                    await PostRequest(context, handlers);
                }

                if (ResponseData == null)
                {
                    ResponseData = new ResponseData("Database is unavailable",HttpStatusCode.InternalServerError);
                }
                // now we need to prepare the response. First we must encode it into a byte array
                // with the std. web encoding iso-8859-1
                byte[] result = Encoding.GetEncoding("iso-8859-1").GetBytes(ResponseData.Message);
                // then we must set the length of the response
                context.Response.ContentLength64 = result.Length;
                context.Response.StatusCode = (int)ResponseData.HttpStatusCode;
                // and the write the response back to the requester
                using (var stream = context.Response.OutputStream)
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
        /// Method process POST requests and hand the content to proper handlers
        /// </summary>
        /// <param name="context">Received message</param>
        /// <param name="handlers">Number of handlers to pass the received data to</param>
        /// <returns></returns>
        private async Task PostRequest(HttpListenerContext context, List<IHandler> handlers)
        {
            // split the request URL by '/'
            var path = SplitUrl(context);
            // in a post (and put) method we need to fetch the body of the HTTP request
            // read the input stream from the request
            var inputStream = new StreamReader(context.Request.InputStream);
            var rawBody = inputStream.ReadToEnd();

            // now we can use the ParseQueryString in the HttpUtility
            //NameValueCollection namedValues = HttpUtility.ParseQueryString(bodyDecoded ?? "");
            // that will create a map of name and values
            if (path.Count > 0)
            {
                // checks for a handler able to handle that specific path
                var handler = handlers.FirstOrDefault(x => x.CanHandle(path[0]));

                var url = path.Skip(1).ToList();
                url.Add(rawBody);

                if (handler != null)
                {
                    // handle the request and update the response data
                    ResponseData = await handler.Handle(url, ResponseData);
                }
            }

            else
            {
                ResponseData = new ResponseData("Error creating Category: no id", HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Method process GET requests and hand the content to proper handlers
        /// </summary>
        /// <param name="context">Received message</param>
        /// <param name="handlers">Number of handlers to pass the received data to</param>
        /// <returns></returns>
        private async Task GetResponse(HttpListenerContext context, List<IHandler> handlers)
        {
            // split the request URL by '/'
            var path = SplitUrl(context);

            // if we have anything int the request
            if (path.Count > 0)
            {
                // checks for a handler able to handle that specific path
                var handler = handlers.FirstOrDefault(x => x.CanHandle(path[0]));

                if (handler != null)
                {
                    //updates the responseData
                    ResponseData = await handler.Handle(path.Skip(1).ToList(), ResponseData);
                }
            }
        }

        /// <summary>
        /// Method to split urls by '/' characters
        /// </summary>
        /// <param name="context">Context to split</param>
        /// <returns>List of splitted parts</returns>
        private static List<string> SplitUrl(HttpListenerContext context)
        {
            return context.Request.RawUrl.Split(
                new[] { '/' },
                StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
