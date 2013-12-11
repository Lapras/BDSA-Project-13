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

            if (path != null || path.Count == 1)
            {

                var firstSegment = path.First();
                if (firstSegment.StartsWith("?"))
                {
                    var key = firstSegment.Substring(1).Split(new[] { '=' })[0];
                    var value = firstSegment.Split(new[] { '=' })[1];

                        String msg;

                        switch (key)
                        {
                            case "title":

                                var movies = GetMoviesByTitle(value);

                                if (movies.Count == 0)
                                {
                                    movies = await GetMoviesFromIMDbAsync(value);
                                    AddMoviesToDb(movies);
                                }

                                msg = new JavaScriptSerializer().Serialize(movies);
                                return new ResponseData(msg, HttpStatusCode.OK);
                                break;

                            case "movieId":

                                var movie = GetMovieById(Convert.ToInt32(value));

                                msg = new JavaScriptSerializer().Serialize(movie);
                                return new ResponseData(msg, HttpStatusCode.OK);
                                break;

                            
                        }
                }

                if (!firstSegment.StartsWith("?"))
                {
                    var key = path.First();

                    switch (key)
                    {
                        case "review":

                            Console.WriteLine("REVIWING");

                            path[1] = path[1].Replace("k__BackingField", "");
                            path[1] = path[1].Replace("<", "");
                            path[1] = path[1].Replace(">", "");

                            // Parse Json object back to data
                            var data = JsonConvert.DeserializeObject<ReviewDto>(path[1]);

                            Console.WriteLine("Movie: {0}\nUser: {1}\nRating: {2}", data.MovieId, data.Username,
                                data.Rating);

                            if (MovieAndProfileExist(data.MovieId, data.Username))
                            {
                                // acutally push to database
                                AddRatingToDatabase(data);

                                var ratingAddedMsg =
                                    new JavaScriptSerializer().Serialize(new ReplyDto
                                    {
                                        Executed = true,
                                        Message = "Rating was added"
                                    });
                                return new ResponseData(ratingAddedMsg, HttpStatusCode.OK);
                            }


                            var ratingNotAddedMsg =
                                new JavaScriptSerializer().Serialize(new ReplyDto
                                {
                                    Executed = true,
                                    Message = "Rating could not be added"
                                });
                            return new ResponseData(ratingNotAddedMsg, HttpStatusCode.OK);

                            break;
                    }

                }



            }

            return responseData;
        }

        /// <summary>
        /// Add Rating to local database
        /// </summary>
        /// <param name="data">MovieId, Username, Rating</param>
        private void AddRatingToDatabase(ReviewDto data)
        {
            // Add rating to review table & Update rating attribute in movies

            using (var entities = new ImdbEntities())
            {
                
            }

        //    throw new NotImplementedException();
        }

        /// <summary>
        /// Check if movie and user are in our database
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>True if both exist, else false</returns>
        private bool MovieAndProfileExist(int movieId, string username)
        {
            using (var entities = new ImdbEntities())
            {
                var matchingMovies = (from movie in entities.Movies
                    where movie.Id == movieId
                    select movie.Title).ToList();

                var matchingProfiles = (from user in entities.User
                    where user.name == username
                    select user.name).ToList();

                return matchingProfiles.Count > 0 && matchingMovies.Count > 0;
            }
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

        /// <summary>
        /// Get a movie vom the MyMovieApi interface
        /// </summary>
        /// <param name="searchString">Name of the movie to search for</param>
        /// <returns>List of matching movies</returns>
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
                            SeriesYear = m.SeriesYear,
                            Rating = 5
                        }).ToList();

                    movie[0].Participants = new List<PersonDto>();

                    foreach (var participant in participants)
                    {
                        movie[0].Participants.Add(participant);
                    }
                

                return movie[0];
            }
        }

        /// <summary>
        /// Add Movie to local database
        /// </summary>
        /// <param name="movies">Movies to add to the local database</param>
		private void AddMoviesToDb(IEnumerable<MovieDto> movies)
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