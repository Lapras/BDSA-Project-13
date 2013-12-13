using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.ImdbRepositories;
using Newtonsoft.Json;

namespace ImdbRestService.Handlers
{
    /// <summary>
    /// Class in charge of handling movie requests
    /// </summary>
    public class MovieHandler : IHandler
    {
        private readonly IImdbEntities _imdbEntities;
        private readonly IExternalMovieDatabaseRepository _externalMovieDatabaseRepository;
        private const string PathSegment = "movies";

        public MovieHandler(IImdbEntities imdbEntities = null, IExternalMovieDatabaseRepository externalMovieDatabaseRepository = null)
        {
            _imdbEntities = imdbEntities;
            _externalMovieDatabaseRepository = externalMovieDatabaseRepository ?? new ExternalMovieDatabaseRepository();
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
                                    movies = await _externalMovieDatabaseRepository.GetMoviesFromIMDbAsync(value);
                                    AddMoviesToDb(movies);
                                    if (movies.Count < 1)
                                    {
                                        movies.Add(new MovieDto {ErrorMsg = "Movie could not be found"});
                                    }
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


                            path[1] = path[1].Replace("k__BackingField", "");
                            path[1] = path[1].Replace("<", "");
                            path[1] = path[1].Replace(">", "");

                            // Parse Json object back to data
                            var data = JsonConvert.DeserializeObject<ReviewDto>(path[1]);

                            //Console.WriteLine("Movie: {0}\nUser: {1}\nRating: {2}", data.MovieId, data.Username,
                            //    data.Rating);


                            if (MovieAndProfileExist(data.MovieId, data.Username) && !AlreadyRated(data.MovieId, data.Username))
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

                            if (MovieAndProfileExist(data.MovieId, data.Username) && AlreadyRated(data.MovieId, data.Username))
                            {
                                // acutally push to database

                                UpdateRatingToDatabase(data);

                                var ratingAddedMsg =
                                    new JavaScriptSerializer().Serialize(new ReplyDto
                                    {
                                        Executed = true,
                                        Message = "Rating was added updated"
                                    });
                                return new ResponseData(ratingAddedMsg, HttpStatusCode.OK);
                            }


                            var ratingNotAddedMsg =
                                new JavaScriptSerializer().Serialize(new ReplyDto
                                {
                                    Executed = false,
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
        /// Try to either add or update a rating in the database
        /// </summary>
        /// <param name="data">Review data needed to modify the database</param>
        private void UpdateRatingToDatabase(ReviewDto data)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {

                    int userId = FindUserIdFromUsername(data.Username);

                    var test =
                        entities.Rating.Where(r => r.movie_id == data.MovieId && r.user_Id == userId).SingleOrDefault();

                    test.rating1 = data.Rating;


                    entities.SaveChanges();

                    foreach (var rating in entities.Rating)
                    {
                        Console.WriteLine("-----------");
                        Console.WriteLine(rating.id);
                        Console.WriteLine(rating.movie_id);
                        Console.WriteLine(rating.rating1 + " EDITED RATING");
                        Console.WriteLine(rating.user_Id);
                        Console.WriteLine("-----------");

                    }
                }
            }
            catch (Exception)
            {
                Console.Write("Database is not available");
            }
        }

        /// <summary>
        /// Check if a movie is already rated by a specific user
        /// </summary>
        /// <param name="movieId">Movie to check with</param>
        /// <param name="username">Name to check with</param>
        /// <returns>True if already rated</returns>
        private bool AlreadyRated(int movieId, string username)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    int userId = FindUserIdFromUsername(username);

                    var alreadyRated = (from r in entities.Rating
                        where r.movie_id == movieId && r.user_Id == userId
                        select r).ToList();

                    if (alreadyRated.Count >= 1)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Database is not available");
                return true;
            }
        }

        /// <summary>
        /// Get the id of a user matching the username
        /// </summary>
        /// <param name="username">Name of the user to look for</param>
        /// <returns>The users id</returns>
        private int FindUserIdFromUsername(string username)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    var findProfile = (from u in entities.User
                        where u.name == username
                        select u).First();

                    return findProfile.Id;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Database is not available");
                return -1;
            }
        }

        /// <summary>
        /// Add Rating to local database
        /// </summary>
        /// <param name="data">MovieId, Username, Rating</param>
        private void AddRatingToDatabase(ReviewDto data)
        {
            // Add rating to review table & Update rating attribute in movies

            using (var entities = _imdbEntities ?? new ImdbEntities())
            {
                //we find the next rating id
                int id;
                if (!entities.Rating.Any())
                {
                    id = 1;
                }
                else
                {

                    id = entities.Rating.Max(u => u.id) + 1;

                    //Console.WriteLine("RATING ID NO: " + id);
                }



                

                entities.Rating.Add(new Rating()
                {
                    id = id,
                    rating1 = data.Rating,
                    user_Id = FindUserIdFromUsername(data.Username),
                    movie_id = data.MovieId
                });


                
                entities.SaveChanges();

                foreach (var rating in entities.Rating)
                {
                    Console.WriteLine("-----------");
                    Console.WriteLine(rating.id);
                    Console.WriteLine(rating.movie_id);
                    Console.WriteLine(rating.rating1);
                    Console.WriteLine(rating.user_Id);
                    Console.WriteLine("-----------");

                }



            }

        }

        /// <summary>
        /// Check if movie and user are in our database
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="username">Id of the user</param>
        /// <returns>True if both exist, else false</returns>
        private bool MovieAndProfileExist(int movieId, string username)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
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
            catch (Exception)
            {
                Console.WriteLine("Local database is not available");
                return true;
            }
            
        }

        /// <summary>
        /// Method recieving movies by title from the local database
        /// </summary>
        /// <param name="searchString"> the title to search for </param>
        /// <returns> a list of MovieDto's containing information on the movies found </returns>
        public List<MovieDto> GetMoviesByTitle(string searchString) 
        {
            var title = HttpUtility.UrlDecode(searchString); // removes %20 and adds whitespace instead


            try
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
                            where m.Title.ToLower().Contains(title.ToLower())
                            select new MovieDto
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Year = m.Year
                            }).ToList();
                }
            }
            catch (Exception e)
            {
                Console.Write("Local database is not available");
                return _externalMovieDatabaseRepository.GetMoviesFromIMDbAsync(title).Result;
            }
           
        }

	    /// <summary>
        /// Method recieving a movie by id from the local database
        /// </summary>
        /// <param name="id"> id of the movie we search for </param>
        /// <returns> movie we requested </returns>
        public MovieDetailsDto GetMovieById(int id)
        {
		     try
		     {
                 using (var entities = _imdbEntities ?? new ImdbEntities())
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
                                      AvgRating = m.Avg_rating
                                  }).ToList();

                     movie[0].Participants = new List<PersonDto>();

                     foreach (var participant in participants)
                     {
                         movie[0].Participants.Add(participant);
                     }


                     return movie[0];
                 }
		     }
		     catch (Exception)
		     {
                 Console.Write("Local Database is not available");
                 return new MovieDetailsDto { ErrorMsg = "Local Database not available", Participants = new List<PersonDto>() };
		     }
      
        }

        /// <summary>
        /// Add Movie to local database
        /// </summary>
        /// <param name="movies">Movies to add to the local database</param>
		private void AddMoviesToDb(IEnumerable<MovieDto> movies)
		{
            try
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
                            SeriesYear = movie.SeriesYear,
                            Title = movie.Title,
                            Year = movie.Year
                        });
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                Console.Write("Local Database is not available");
            }
		}
    }
}