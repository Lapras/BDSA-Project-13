using System.Collections.Generic;
using System.Threading.Tasks;
using DtoSubsystem;

namespace ASP_Client.Repositories
{
    /// <summary>
    /// Interface for the methods concerning movies
    /// </summary>
    public interface IMovieRepository
    {
        /// <summary>
        /// Get a list of movies maching the search string
        /// </summary>
        /// <param name="searchString">Movie to look for</param>
        /// <returns>List of movies matching the search string</returns>
        Task<List<MovieDto>> GetMoviesAsync(string searchString);

        /// <summary>
        /// Get detailed information about a specific movie
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        Task<MovieDetailsDto> GetMovieDetailsAsync(int movieId);

        /// <summary>
        /// Send a rating to the server
        /// </summary>
        /// <param name="review">The review of the user</param>
        /// <returns>Reply of the server</returns>
        Task<ReplyDto> RateMovie(RatingDto review);

        /// <summary>
        /// Get a movie without using the cache
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        Task<MovieDetailsDto> GetMovieDetailsAsyncForce(int movieId);
    }
}