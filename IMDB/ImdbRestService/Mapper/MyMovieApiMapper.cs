using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.ImdbRepositories;

namespace ImdbRestService.Mapper
{
    public class MyMovieApiMapper : IMapper
    {
         private readonly IImdbEntities _imdbEntities;
        private readonly IExternalMovieDatabaseRepository _externalMovieDatabaseRepository;

        public MyMovieApiMapper(IImdbEntities imdbEntities = null, IExternalMovieDatabaseRepository externalMovieDatabaseRepository = null)
        {
            _imdbEntities = imdbEntities;
            _externalMovieDatabaseRepository = externalMovieDatabaseRepository ?? new ExternalMovieDatabaseRepository();
        }


        /// <summary>
        /// Get movies from the official IMDb database
        /// </summary>
        /// <param name="movieTitle">Title of the movie to search for</param>
        /// <param name="responseData">Reponse to be returned</param>
        /// <returns>Received data</returns>
        public async Task<ResponseData> Get(string movieTitle, ResponseData responseData)
        {      
                var movies = await _externalMovieDatabaseRepository.GetMoviesFromImdbAsync(movieTitle);
            
                if (movies.Count < 1)
            {
                movies.Add(new MovieDto { ErrorMsg = "Movie could not be found" });
            }
         
            var msg = new JavaScriptSerializer().Serialize(movies);
            return new ResponseData(msg, HttpStatusCode.OK);

        }

        public Task<ResponseData> Post(string path, ResponseData responseData)
        {
            throw new NotImplementedException();
        }
    }
}
