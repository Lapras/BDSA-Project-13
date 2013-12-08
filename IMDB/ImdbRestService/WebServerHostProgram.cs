using System;
using System.Diagnostics;

namespace ImdbRestService
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
            var server = new ImdbRestWebServer();

            try
            {
                // start the server
                server.Start("http://localhost:54321/");
                Debug.WriteLine("Server running ... press enter to stop");
                Console.ReadKey();
            }
            finally
            {
                server.Stop();
            }
        }
    }
}
