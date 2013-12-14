using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;
using WPF_Client.Storage;

namespace WPF_Client.Model
{
    /// <summary>
    /// The Model class represents the main business data of the application.
    /// </summary>
    public class Model : IModel
    {
        private Storage.Storage _storage;

        public Model()
        {
            _storage = new Storage.Storage(new RESTStrategy("http://localhost:54321"));
        }

        /// <summary>
        /// The collection of MovieDtos from the supplied searchString.
        /// </summary>
        /// <param name="searchString">The input searchString</param>
        /// <returns>A collection of MovieDtos</returns>s
        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {
            return _storage.MovieDtos(searchString);
        }

        /// <summary>
        /// The collection of PersonDtos from the supplied searchString.
        /// </summary>
        /// <param name="searchString">The input searchString</param>
        /// <returns>A collection of PersonDtos</returns>s
        public ObservableCollection<PersonDto> PersonDtos(string searchString)
        {
            return _storage.PersonDtos(searchString);
        }

        /// <summary>
        /// The MovieDetailsDto from the supplied movieId.
        /// </summary>
        /// <param name="movieId">The input movieId.</param>
        /// <returns>A MovieDetailsDto.</returns>
        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            return _storage.MovieDetailsDto(movieId);
        }

        /// <summary>
        /// Creates a profile with the desired username and password.
        /// </summary>
        /// <param name="name">The desired username.</param>
        /// <param name="password">The desired password.</param>
        /// <returns>A boolean value whether the account was created or not.</returns>
        public bool CreateProfile(string name, string password)
        {
            return _storage.CreateProfile(name, password);
        }

        /// <summary>
        /// Logs in the user with the supplied username and password.
        /// </summary>
        /// <param name="name">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>A boolean value whether the user was logged in or not.</returns>
        public bool Login(string name, string password)
        {
            return _storage.Login(name, password);
        }

        /// <summary>
        /// The PersonDetailsDto with the supplied id.
        /// </summary>
        /// <param name="id">The id of the desired PersonDetailsDto.</param>
        /// <returns>A PersonDetailsDto</returns>
        public PersonDetailsDto PersonDetailsDto(int id)
        {
            return _storage.PersonDetailsDto(id);
        }

        /// <summary>
        /// Rates a movie movie.
        /// </summary>
        /// <param name="id">The id of the Movie.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="username">The name of the user that is rating.</param>
        /// <returns>A boolean value whether the movie was rated or not.</returns>
        public bool RateMovie(int id, int rating, string username)
        {
            return _storage.RateMovie(id, rating, username);
        }
    }
}
