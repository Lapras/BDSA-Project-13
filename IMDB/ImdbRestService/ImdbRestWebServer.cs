using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using ImdbRestService.Handlers;

namespace ImdbRestService
{
    class ImdbRestWebServer
    {
        // the listener for http requests
        private readonly HttpListener _listener;

        private ImdbEntities _imdbEntities;

        // give the base address as argument
        public ImdbRestWebServer(string uri)
        {
            _imdbEntities = new ImdbEntities();

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
                        // checks for a handler able to handle that specific path
                        var handler = handlers.FirstOrDefault(x => x.CanHandle(path[0]));

                        if (handler != null)
                        {
                            //updates the responseData
                            responseData = handler.Handle(path.Skip(1).ToList(), responseData);
                        }
                    }
                }
                // check for POST method
                else if (context.Request.HttpMethod == "POST")
                {
                    // split the request URL by '/'
                    var path = SplitUrl(context);
                    // in a post (and put) method we need to fetch the body of the HTTP request
                    // read the input stream from the request
                    var inputStream = new StreamReader(context.Request.InputStream);
                    var rawBody = inputStream.ReadToEnd();
                    // decode the text
                    var bodyDecoded = HttpUtility.UrlDecode(rawBody, Encoding.GetEncoding("iso-8859-1"));
                    // we assume somethind like "id=1&name=John&description=blabla", but it could also be JSON or XML
                    // of cause we should then do another parsing...
                    // now we can use the ParseQueryString in the HttpUtility
                    NameValueCollection namedValues = HttpUtility.ParseQueryString(bodyDecoded ?? "");
                    // that will create a map of name and values
                    int id = 0;
                    int.TryParse(namedValues["id"], out id);
                    if (id != 0)
                    {
                        // create the new object
                        var movie = new Movie
                        {
                            Id = id,
                            Title = namedValues["title"]
                        };
                        if (path.Count > 0)
                        {
                            // checks for a handler able to handle that specific path
                            var handler = handlers.FirstOrDefault(x => x.CanHandle(path[0]));
                            // save it in the database .... obviously this could lead to persistence errors
                            // one thing, you would properly not want to get the id a parameter, but compute it yourself 
                            // (or let the database take care of that)
                            // we'll just return (as debugging info the object we would add to the system)

                            if (handler != null)
                            {
                                // updates the responseData
                                responseData = handler.Handle(path.Skip(1).ToList(), responseData);
                            }
                        }
                    }
                    else
                    {
                        responseData = new ResponseData("Error creating Category: no id", HttpStatusCode.BadRequest);
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
