using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ASP_Client.Models;

namespace ImdbService {
	/// <inheritdoc/>
	public class ImdbService : IImdbService {

		/// <inheritdoc />
		public List<MovieViewModel> GetMoviesByTitle(string needle)
		{
			using (var entities = new ImdbEntities())
			{
				if (String.IsNullOrEmpty(needle))
				{
					return (from m in entities.Movies
						select new MovieViewModel() {
							Id = m.Id,
							Title = m.Title,
							Year = m.Year
						}).ToList();
				}
				else
				{
					return (from m in entities.Movies where m.Title.Contains(needle)
					select new MovieViewModel()
					{
						Id = m.Id,
						Title = m.Title,
						Year = m.Year
					}).ToList();
				}
			}
		} // GetMoviesByTitle()
	}
}
