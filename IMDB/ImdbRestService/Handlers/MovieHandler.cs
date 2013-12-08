using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using Newtonsoft.Json;

namespace ImdbRestService.Handlers
{
    /// <summary>
    /// Class in charge of handling movie requests
    /// </summary>
    public class MovieHandler : IHandler
    {
        private readonly IImdbEntities _imdbEntities;
        private const string PathSegment = "movies";

        public MovieHandler(IImdbEntities imdbEntities = null)
        {
            _imdbEntities = imdbEntities;
        }

        /// <summary>
        /// Method checking if the given path segment matches the one that
        /// this handler can handle
        /// </summary>
        /// <param name="pathSegment"> the input path segment string </param>
        /// <returns> wether or not the class is able to handle the request </returns>
        public bool CanHandle(string pathSegment)
        {
            return pathSegment.Equals(PathSegment, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Method handling the response data by checking the path, get the movies,
        /// serialize them and returning them in the message in a new ResponseData objec
        /// </summary>
        /// <param name="path"> the path used to see how to operate the data </param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Handle(List<string> path, ResponseData responseData)
        {
            if (path != null && path.Count == 1)
            {
                var firstSegment = path.First();
                if (firstSegment.StartsWith("?"))
                {
                    var key = firstSegment.Substring(1).Split(new[] { '=' })[0];
                    var value = firstSegment.Split(new[] { '=' })[1];

                    if (key == "title")
                    {
                        var movies = GetMoviesByTitle(value);

                        if (movies.Count == 0)
                        {
                            movies = await GetMoviesFromIMDbAsync(value);
							AddMoviesToDb(movies);
                        }

                        var msg = new JavaScriptSerializer().Serialize(movies);
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
					
					if (key == "movieId")
                    {
                        var movie = GetMovieById(Convert.ToInt32(value));

                        var msg = new JavaScriptSerializer().Serialize(movie);
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                }
            }

            return responseData;
        }

        /// <summary>
        /// Method recieving movies by title from the local database
        /// </summary>
        /// <param name="title"> the title to search for </param>
        /// <returns> a list of MovieDto's containing information on the movies found </returns>
        public List<MovieDto> GetMoviesByTitle(string title)
        {
            using (var entities = _imdbEntities ?? new ImdbEntities())
            {
                if (String.IsNullOrEmpty(title))
                {
                    return (from m in entities.Movies
                            select new MovieDto
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Year = m.Year
                            }).Take(20).ToList();
                }

                return (from m in entities.Movies
                        where m.Title.Contains(title)
                        select new MovieDto
                        {
                            Id = m.Id,
                            Title = m.Title,
                            Year = m.Year
                        }).ToList();
            }
        }

        private async Task<List<MovieDto>> GetMoviesFromIMDbAsync(string searchString)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("http://mymovieapi.com/?title=" + searchString);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (result.Equals("{\"code\":404, \"error\":\"Film not found\"}"))
                    {
                        return new List<MovieDto>() {};     
                    }           

                    return JsonConvert.DeserializeObject<List<MovieDto>>(result);
                }

                return new List<MovieDto>();

                //return JsonConvert.DeserializeObject<List<MovieDto>>(
                //    await httpClient.GetStringAsync("http://mymovieapi.com/?title=" + searchString)
                //);
            }
        }
		
		 /// <summary>
        /// Method recieving a movie by id from the local database
        /// </summary>
        /// <param name="id"> id of the movie we search for </param>
        /// <returns> movie we requested </returns>
        public MovieDetailsDto GetMovieById(int id)
        {
            using (var entities = new ImdbEntities())
            {
                var participants = (from peo in entities.People
                                    join par in entities.Participates on peo.Id equals par.Person_Id
                                    join m in entities.Movies on par.Movie_Id equals m.Id
                                    where m.Id == id
                                    group peo by new
                                    {
                                        peo.Id,
                                        peo.Name,
                                        par.CharName
                                    }
                                    into grouping
                                    select new PersonDto
                                    {
                                        Id = grouping.Key.Id,
                                        Name = grouping.Key.Name,
                                        CharacterName = grouping.Key.CharName
                                    }).ToList();

                var movie = (from m in entities.Movies
                        where m.Id == id
                        select new MovieDetailsDto
                        {
                            Id = m.Id,
                            Title = m.Title,
                            Year = m.Year,
                            Kind = m.Kind,
                            EpisodeNumber = m.EpisodeNumber,
                            EpisodeOf_Id = m.EpisodeOf_Id,
                            SeasonNumber = m.SeasonNumber,
                            SeriesYear = m.SeriesYear
                        }).ToList();

                    movie[0].Participants = new List<PersonDto>();

                    foreach (var participant in participants)
                    {
                        movie[0].Participants.Add(participant);
                    }
                

                return movie[0];
            }
        }

		private void    AddMoviesToDb(IEnumerable<MovieDto> movies)
		{
			//Adding found movies to app server database
			
			using (var context = new ImdbEntities())
			{
				foreach (var movie in movies)
				{
					context.Movies.Add(new Movie
					{
						Id = movie.Id,
						EpisodeNumber = movie.EpisodeNumber,
						EpisodeOf_Id = movie.EpisodeOf_Id,
						Kind = movie.Kind,
						SeasonNumber = movie.SeasonNumber,
						SeriesYear= movie.SeriesYear,
						Title = movie.Title,
						Year = movie.Year
					});
				}
				context.SaveChanges();
			}
			
		}
    }
}