using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ASP_Client.Models;
using DtoSubsystem;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// Interface for the persistance layer to 
    /// </summary>
    public interface ICommunicationFacade
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
        Task<MovieDetailsDto> GetMovieDetailsLocallyAsync(int movieId);

        /// <summary>
        /// Send a rating to the server
        /// </summary>
        /// <param name="review">The review of the user</param>
        /// <returns>Reply of the server</returns>
        Task<ReplyDto> RateMovie(ReviewDto review);

        /// <summary>
        /// Get a movie without using the cache
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        Task<MovieDetailsDto> GetMovieDetailsLocallyAsyncForce(int movieId);

        /// <summary>
        /// Get the details of a specific person involved in the movie business
        /// </summary>
        /// <param name="personId">Id of the person to get the data of</param>
        /// <returns>Detailed data of the person</returns>
        Task<PersonDetailsDto> GetPersonDetailsLocallyAsync(int personId);


        /// <summary>
        /// Send a Login request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        Task<ReplyDto> Login(UserModel user);


        /// <summary>
        /// Send a Registration request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        Task<ReplyDto> Registration(UserModel user);
    }
}