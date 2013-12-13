using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;

namespace WPF_Client.Storage
{
    /// <summary>
    /// Interface that the strategies of the storage must implement.
    /// </summary>
    public interface IStorageStrategy
    {
        /// <summary>
        /// Retrives the MovieDtos.
        /// </summary>
        /// <param name="searchString">The input search string.</param>
        /// <returns>A collection of MovieDtos.</returns>
        ObservableCollection<MovieDto> MovieDtos(string searchString);

        /// <summary>
        /// Retrives the MovieDetailsDto.
        /// </summary>
        /// <param name="movieId">The input movieId</param>
        /// <returns>A MovieDetailsDto.</returns>
        MovieDetailsDto MovieDetailsDto(int movieId);

        /// <summary>
        /// Sends a userprofile requests.
        /// </summary>
        /// <param name="name">The requested username</param>
        /// <param name="password">The requested password.</param>
        /// <returns>A boolean value whether the user was created or not.</returns>
        bool CreateProfile(string name, string password);

        /// <summary>
        /// Logs in the user with the supplied username and password.
        /// </summary>
        /// <param name="name">The input username.</param>
        /// <param name="password">The input password.</param>
        /// <returns>A boolean value whether the login was successfull or not.</returns>
        bool Login(string name, string password);

        /// <summary>
        /// Retrieves a PersonDetailsDto with the supplied personId.
        /// </summary>
        /// <param name="id">The input personId.</param>
        /// <returns>The PersonDetailsDto</returns>
        PersonDetailsDto PersonDetailsDto(int id);

        /// <summary>
        /// Rates a movie.
        /// </summary>
        /// <param name="id">The movieId.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="username">The username that rates the movie.</param>
        /// <returns></returns>
        bool RateMovie(int id, int rating, string username);
    }
}
