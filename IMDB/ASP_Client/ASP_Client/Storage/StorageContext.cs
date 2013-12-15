using System.Collections.Generic;
using System.Threading.Tasks;
using ASP_Client.Controllers;
using ASP_Client.Models;
using DtoSubsystem;

namespace ASP_Client.Storage
{
    public static class StorageContext
    {
        private readonly static ICommunicationStrategy Connect = new RestCommunicationStrategy();

        /// <summary>
        /// Get a list of movies maching the search string
        /// </summary>
        /// <param name="searchString">Movie to look for</param>
        /// <returns>List of movies matching the search string</returns>
        public static async Task<List<MovieDto>> GetMoviesAsync(string searchString)
        {
            return await Connect.GetMoviesAsync(searchString);
        }

        /// <summary>
        /// Get detailed information about a specific movie
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        public static async Task<MovieDetailsDto> GetMovieDetailsLocallyAsync(int movieId)
        {
            return await Connect.GetMovieDetailsAsync(movieId);
        }

        /// <summary>
        /// Get the details of a specific person involved in the movie business
        /// </summary>
        /// <param name="personId">Id of the person to get the data of</param>
        /// <returns>Detailed data of the person</returns>
        public static async Task<PersonDetailsDto> GetPersonDetailsAsync(int personId)
        {
            return await Connect.GetPersonDetailsAsync(personId);
        }

        /// <summary>
        /// Send a Login request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        public static async Task<ReplyDto> Login(UserModel user)
        {
            return await Connect.Login(user);
        }

        /// <summary>
        /// Send a Registration request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        public static async Task<ReplyDto> Registration(UserModel user)
        {
            return await Connect.Registration(user);
        }

        /// <summary>
        /// Send a rating to the server
        /// </summary>
        /// <param name="review">The review of the user</param>
        /// <returns>Reply of the server</returns>
        public static async Task<ReplyDto> RateMovie(RatingDto review)
        {
            return await Connect.RateMovie(review);
        }

        /// <summary>
        /// Get a movie without using the cache
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        public static async Task<MovieDetailsDto> GetMovieDetailsAsyncForce(int movieId)
        {
            return await Connect.GetMovieDetailsLocallyAsyncForce(movieId);
        }

        /// <summary>
        /// Get a list of actors matching the given name
        /// </summary>
        /// <param name="personName">Name of the person to look for</param>
        /// <returns>List of the people matching the given name</returns>
        public static async Task<List<PersonDto>> GetPersonAsync(string personName)
        {
            return await Connect.GetPersonAsync(personName);
        }
    }
}