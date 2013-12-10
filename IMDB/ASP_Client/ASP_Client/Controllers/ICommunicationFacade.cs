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
        List<MovieDto> GetMoviesAsync(string searchString);

        /// <summary>
        /// Get detailed information about a specific movie
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        MovieDetailsDto GetMovieDetailsLocallyAsync(int movieId);


        /// <summary>
        /// Get the details of a specific person involved in the movie business
        /// </summary>
        /// <param name="personId">Id of the person to get the data of</param>
        /// <returns>Detailed data of the person</returns>
        PersonDetailsDto GetPersonDetailsLocallyAsync(int personId);


        /// <summary>
        /// Send a Login request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        ReplyDto Login(UserModel user);


        /// <summary>
        /// Send a Registration request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        ReplyDto Registration(UserModel user);
    }
}