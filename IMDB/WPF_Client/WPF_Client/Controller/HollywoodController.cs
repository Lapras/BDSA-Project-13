using System;
using WPF_Client.Model;
using WPF_Client.ViewModel;
using DtoSubsystem;

namespace WPF_Client.Controller
{
    /// <summary>
    /// Controller for managing application control flow whenever clients want
    /// to view a movie or actor. Also controls control flow whenever the client
    /// rates movies.
    /// </summary>
    public static class HollywoodController
    {

        public static IModel _model = new Model.Model();

        /// <summary>
        /// The PersonDetailsDto that the ActorProfileViewModel loads.
        /// </summary>
        public static PersonDetailsDto PersonDetailsDto { get; set; }// The 

        /// <summary>
        /// The MovieDto that the SearchResultViewModel loads.
        /// </summary>
        public static MovieDetailsDto MovieDetailsDto { get; set; }



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

            //unit test doesnt like creating a new viewmodel and assigning it.
            if (!UnitTestDetector.IsInUnitTest)
            {
                ViewModelManager.Main.CurrentViewModel = new MovieProfileViewModel();
            }

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

            //unit test doesnt like creating a new viewmodel and assigning it.
            if (!UnitTestDetector.IsInUnitTest)
            {
                ViewModelManager.Main.CurrentViewModel = new ActorProfileViewModel();
            }

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
            var result = _model.RateMovie(movieId, rating, SessionController.CurrentUser());
            
            if (result)
            {
                MovieDetailsDto = _model.MovieDetailsDto(movieId);
                if (MovieDetailsDto == null)
                {
                    return false;
                }
            }

            return result;
        }
    }
}
