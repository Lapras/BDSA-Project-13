using System.Collections.Generic;
using System.Threading.Tasks;
using ASP_Client.Controllers;
using DtoSubsystem;

namespace ASP_Client.Repositories
{
    /// <summary>
    /// Class in charge of calling the communication facade for movie related actions and
    /// returning the Http responses it gets back.
    /// Implementing the IMovieRepository interface.
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        /// <summary>
        /// Get a list of movies maching the search string
        /// </summary>
        /// <param name="searchString">Movie to look for</param>
        /// <returns>List of movies matching the search string</returns>
        public Task<List<MovieDto>> GetMoviesAsync(string searchString)
        {
            return Storage.GetMoviesAsync(searchString);
        }

        /// <summary>
        /// Get detailed information about a specific movie
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        public Task<MovieDetailsDto> GetMovieDetailsAsync(int movieId)
        {
            return Storage.GetMovieDetailsLocallyAsync(movieId);
        }

        /// <summary>
        /// Send a rating to the server
        /// </summary>
        /// <param name="review">The review of the user</param>
        /// <returns>Reply of the server</returns>
        public Task<ReplyDto> RateMovie(RatingDto review)
        {
            return Storage.RateMovie(review);
        }

        /// <summary>
        /// Get a movie without using the cache
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        public Task<MovieDetailsDto> GetMovieDetailsAsyncForce(int movieId)
        {
            return Storage.GetMovieDetailsAsyncForce(movieId);
        }
    }
}