using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using ASP_Client.Models;

namespace ImdbService {
	/// <summary>
	/// Web service for the imdb movie project.
	/// </summary>
	[ServiceContract]
	public interface IImdbService {

		/// <summary>
		/// Returns a list of movies where needle is a part of the title.
		/// </summary>
		/// <param name="needle"></param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped,
			UriTemplate = "Search/{needle}")]
		List<MovieViewModel> GetMoviesByTitle(string needle);
	}
}
