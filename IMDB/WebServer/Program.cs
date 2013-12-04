#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Net;
using System.IO;

#endregion

namespace WebServer
{
	/// <summary>
	/// The web server host program
	/// </summary>
	class Program 
	{
		static void Main(string[] args) 
		{
			MyRestWeb msh = (MyRestWeb)ApplicationHost.CreateApplicationHost(
			   typeof(MyRestWeb), "/", Directory.GetCurrentDirectory() +@"\htdocs");
			msh.Start();
			Debug.WriteLine("Listening for requests on http://localhost:51234/");
			Console.ReadKey();
		}
	}

	/// <summary>
	/// The webserver
	/// </summary>
	public class MyRestWeb : MarshalByRefObject
	{
		private HttpListener _listener;

		public void Start()
		{
			_listener = new HttpListener();
			_listener.Prefixes.Add("http://localhost:51234/");
			_listener.Start();
			
			while (true)
			{
				ProcessRequest();
			} 
		}

		/// <summary>
		/// Proccess a request.
		/// </summary>
		public void ProcessRequest()
		{
			HttpListenerContext ctx = _listener.GetContext();
			string page = ctx.Request.Url.LocalPath.Replace("/", "");
			string query = ctx.Request.Url.Query.Replace("?", "");
			Debug.WriteLine("Received request for {0}?{1}", page, query);

			StreamWriter sw = new StreamWriter(ctx.Response.OutputStream);
				
			SimpleWorkerRequest swr = new SimpleWorkerRequest(page, query, sw);
			HttpRuntime.ProcessRequest(swr);

			sw.Flush();
			ctx.Response.Close();
		}
	}  
}
