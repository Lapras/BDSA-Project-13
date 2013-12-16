using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.ImdbRepositories;
using Newtonsoft.Json;

namespace ImdbRestService.Mapper
{
    /// <summary>
    /// Class in charge of handling movie requests
    /// </summary>
    public class MovieMapper : IMapper
    {
        private readonly IImdbEntities _imdbEntities;
        private readonly IExternalMovieDatabaseRepository _externalMovieDatabaseRepository;

        public MovieMapper(IImdbEntities imdbEntities = null, IExternalMovieDatabaseRepository externalMovieDatabaseRepository = null)
        {
            _imdbEntities = imdbEntities;
            _externalMovieDatabaseRepository = externalMovieDatabaseRepository ?? new ExternalMovieDatabaseRepository();
        }

        /// <summary>
        /// Method handling the response data by checking the path, get the movies,
        /// serialize them and returning them in the message in a new ResponseData object
        /// </summary>
        /// <param name="movieTitle"> the title of the movie to search for</param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Get(string movieTitle, ResponseData responseData)
        {
            var movies = GetMoviesByTitle(movieTitle);
            
            string msg;
            
            if (movies.Count != 0)
            {
               msg = new JavaScriptSerializer().Serialize(movies);
                return new ResponseData(msg, HttpStatusCode.OK);
            }

            msg = new JavaScriptSerializer().Serialize(movies);
            return new ResponseData(msg, HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Emtpy post method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public async Task<ResponseData> Post(string path, ResponseData responseData)
        {
            var newData = JsonConvert.DeserializeObject<List<MovieDetailsDto>>(path);

            if (newData.First().ErrorMsg == null)
            {
                AddMoviesToDb(newData);

                var msg =
               new JavaScriptSerializer().Serialize(new ReplyDto { Executed = true, Message = "Movies where added" });
                return new ResponseData(msg, HttpStatusCode.OK);
            }

            else
            {
                var msg =
               new JavaScriptSerializer().Serialize(new ReplyDto { Executed = true, Message = "Movie not found" });
                return new ResponseData(msg, HttpStatusCode.NoContent);
            }
           
        }

        /// <summary>
        /// Method recieving movies by title from the local database
        /// </summary>
        /// <param name="searchString"> the title to search for </param>
        /// <returns> a list of MovieDto's containing information on the movies found </returns>
        public List<MovieDto> GetMoviesByTitle(string searchString)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    return (from m in entities.Movies
                            where m.Title.ToLower().Contains(searchString.ToLower())
                            select new MovieDto
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Year = m.Year
                            }).Take(100).ToList();
                }
            }
            catch (Exception)
            {
                Console.Write("Local database is not available");
                return _externalMovieDatabaseRepository.GetMoviesFromImdbAsync(searchString).Result;
            }

        }


        /// <summary>
        /// Add Movie to local database
        /// </summary>
        /// <param name="movies">Movies to add to the local database</param>
        private void AddMoviesToDb(IEnumerable<MovieDetailsDto> movies)
        {
            try
            {
                using (var context = _imdbEntities ?? new ImdbEntities())
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