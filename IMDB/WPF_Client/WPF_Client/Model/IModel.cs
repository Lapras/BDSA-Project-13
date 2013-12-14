using System.Collections.ObjectModel;
using DtoSubsystem;

namespace WPF_Client.Model
{
    /// <summary>
    /// An interface containing the methods a model should have.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// The collection of MovieDtos from the supplied searchString.
        /// </summary>
        /// <param name="searchString">The input searchString</param>
        /// <returns>A collection of MovieDtos</returns>
        ObservableCollection<MovieDto> MovieDtos(string searchString);

        /// <summary>
        /// The collection of PersonDtos from the supplied searchString.
        /// </summary>
        /// <param name="searchString">The input searchString</param>
        /// <returns>A collection of PersonDtos</returns>
        ObservableCollection<PersonDto> PersonDtos(string searchString);

        /// <summary>
        /// The MovieDetailsDto from the supplied movieId.
        /// </summary>
        /// <param name="movieId">The input movieId.</param>
        /// <returns>A MovieDetailsDto.</returns>
        MovieDetailsDto MovieDetailsDto(int movieId);


        /// <summary>
        /// Creates a profile with the desired username and password.
        /// </summary>
        /// <param name="name">The desired username.</param>
        /// <param name="password">The desired password.</param>
        /// <returns>A boolean value whether the account was created or not.</returns>
        bool CreateProfile(string name, string password);

        /// <summary>
        /// Logs in the user with the supplied username and password.
        /// </summary>
        /// <param name="name">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>A boolean value whether the user was logged in or not.</returns>
        bool Login(string name, string password);

        /// <summary>
        /// The PersonDetailsDto with the supplied id.
        /// </summary>
        /// <param name="id">The id of the desired PersonDetailsDto.</param>
        /// <returns>A PersonDetailsDto</returns>
        PersonDetailsDto PersonDetailsDto(int id);

        /// <summary>
        /// Rates a movie movie.
        /// </summary>
        /// <param name="id">The id of the Movie.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="username">The name of the user that is rating.</param>
        /// <returns>A boolean value whether the movie was rated or not.</returns>
        bool RateMovie(int id, int rating, string username);
    }
}
