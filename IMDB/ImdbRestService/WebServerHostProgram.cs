using System;
using System.Web;
using ImdbRestService;

namespace WebServer
{
    /// <summary>
    /// The web server host program
    /// </summary>
    public class WebServerHostProgram
    {
        static void Main(string[] args)
        {
            // create a web server on the specific port (properly need to be administrator to run the hosting program
            // either start VS2012 as administrator or execute the .exe file as administrator. I prefer the first 
            // while developing so you can just work as normal and execute/debug from within VS
            var server = new ImdbRestWebServer("http://localhost:54321/");

            try
            {
                // start the server
                server.Start();
                Console.WriteLine("Server running ... press enter to stop");
                Console.ReadKey();
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
