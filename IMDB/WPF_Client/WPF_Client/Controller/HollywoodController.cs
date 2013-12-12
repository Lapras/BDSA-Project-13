using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Model;
using WPF_Client.ViewModel;
using DtoSubsystem;

namespace WPF_Client.Controller
{
    /// <summary>
    /// Controller for managing application control flow whenever clients want
    /// to view a movie or actor. Also controls control flow whenever the client
    /// rates and writes reviews on movies.
    /// </summary>
    public static class HollywoodController
    {
        public static IModel _model = new Model.Model();

        public static PersonDetailsDto PersonDetailsDto { get; set; }
        public static MovieDetailsDto MovieDetailsDto { get; set; } // The MovieDto that the SearchResultViewModel loads for the MovieDto for.

        /// <summary>
        /// Gets a movie with the supplied id.
        /// </summary>
        /// <param name="movieId">The input movie id</param>
        /// <returns>A boolean value whether the movie was successfully found.</returns>
        public static bool GetMovie(int movieId)
        {
            MovieDetailsDto = _model.MovieDetailsDto(movieId);


            if (MovieDetailsDto == null)
            {
                return false;
            }

            ViewModelManager.Main.CurrentViewModel = new MovieProfileViewModel();

            return true;

        }

        /// <summary>
        /// Gets an actor with the supplied id.
        /// </summary>
        /// <param name="id">The input actor id.</param>
        /// <returns>A boolean value whether the actor was successfully found.</returns>
        public static bool GetActor(int id)
        {
            PersonDetailsDto = _model.PersonDetailsDto(id);


            if (PersonDetailsDto == null)
            {
                return false;
            }

            ViewModelManager.Main.CurrentViewModel = new ActorProfileViewModel();

            return true;

        }

        /// <summary>
        /// Rates a movie.
        /// </summary>
        /// <param name="movieId">The input movieId.</param>
        /// <param name="rating">The input rating.</param>
        /// <returns>A boolean whether the movie was successfully rated.</returns>
        public static bool RateMovie(int movieId, int rating)
        {
            var result = _model.RateMovie(movieId, rating, SessionController._currentUser);
            
            if (result)
            {
                Console.WriteLine("Should update");
                GetMovie(movieId);
            }

            return result;
        }
    }
}
