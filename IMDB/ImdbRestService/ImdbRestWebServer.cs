using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ImdbRestService.Handlers;

namespace ImdbRestService
{
    class ImdbRestWebServer
    {
        // the listener for http requests
        private readonly HttpListener _listener;

        // give the base address as argument
        public ImdbRestWebServer(string uri)
        {
            _listener = new HttpListener();
            // set the 
            _listener.Prefixes.Add(uri);
        }

        // starting the web server
        public async void Start()
        {
            _listener.Start();
            while (true)
            {
                try
                {
                    // get next request
                    var context = await _listener.GetContextAsync();
                    // start a task for each request given the context of the request
                    Task.Run(() => ProcessRequest(context));
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

        // stop the web server
        public void Stop()
        {
            _listener.Stop();
        }

        // process a request
        private async void ProcessRequest(HttpListenerContext context)
        {
            var handlers = new List<IHandler>( new IHandler[] { new MovieHandler() });

            // you properly need to split this into a number of methods to make a readable
            // solution :-)
            try
            {
                var responseData = new ResponseData("Error: page not found", HttpStatusCode.NotFound);

                if (context.Request.HttpMethod == "GET")
                {
                    // split the request URL by '/'
                    var path = SplitUrl(context);

                    // if we have anything int the request
                    if (path.Count > 0)
                    {
                        var handler = handlers.FirstOrDefault(x => x.CanHandle(path[0]));

                        if (handler != null)
                        {
                            responseData = handler.Handle(path.Skip(1).ToList(), responseData);
                        }
                    }
                }

                // now we need to prepare the response. First we must encode it into a byte array
                // with the std. web encoding iso-8859-1
                byte[] result = Encoding.GetEncoding("iso-8859-1").GetBytes(responseData.Message);
                // then we must set the length of the response
                context.Response.ContentLength64 = result.Length;
                context.Response.StatusCode = (int)responseData.HttpStatusCode;
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

        

        // helper method
        private static List<string> SplitUrl(HttpListenerContext context)
        {
            return context.Request.RawUrl.Split(
                new[] { '/' },
                StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
