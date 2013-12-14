using System;
using System.Collections.ObjectModel;
using DtoSubsystem;
using WPF_Client.Model;
using WPF_Client.ViewModel;

namespace WPF_Client.Controller
{
    /// <summary>
    /// The SearchController is responsible for the control flow of the application whenever
    /// the client searches for actors or movies.
    /// </summary>
    public static class SearchController
    {
        public static IModel _model = new Model.Model();

        /// <summary>
        /// The MovieDtos that the MovieSearchResultViewModel loads.
        /// </summary>
        public static ObservableCollection<MovieDto> MovieDtos { get; set; }

        /// <summary>
        /// The PersonDtos that the ActorSearchResultViewModel loads.
        /// </summary>
        public static ObservableCollection<PersonDto> PersonDtos { get; set; }

        /// <summary>
        /// The count of the MovieDtos.
        /// </summary>
        public static int MoviesFound { get; set; }

        /// <summary>
        /// The count of the PersonDtos.
        /// </summary>
        public static int PersonsFound { get; set; }

        /// <summary>
        /// Searches for a movie.
        /// </summary>
        /// <param name="searchString">The input search string.</param>
        /// <param name="searchType">The input search type.</param>
        /// <returns>A boolean value whether the search was successfull.</returns>
        public static bool Search(string searchString, int searchType)
        {
            switch (searchType) // We check the search that should be conducted.
            {

                case 0: // Movies

                    MovieDtos = _model.MovieDtos(searchString);
                    MoviesFound = MovieDtos.Count;
                    
                    if (MoviesFound == 0)
                    {
                        return false;
                    }

                    //unit test doesnt like creating a new viewmodel and assigning it.
                    if (!UnitTestDetector.IsInUnitTest)
                    {
                        ViewModelManager.Main.CurrentViewModel = new MovieSearchResultViewModel();
                    }
                    

                    break;

                case 1: // Actors

                    PersonDtos = _model.PersonDtos(searchString);
                    PersonsFound = PersonDtos.Count;

                    if (PersonDtos.Count == 0)
                    {
                        return false;
                    }

                    //unit test doesnt like creating a new viewmodel and assigning it.
                    if (!UnitTestDetector.IsInUnitTest)
                    {
                        ViewModelManager.Main.CurrentViewModel = new ActorSearchResultViewModel();
                    }
                    

                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            return true;
        }

    }
}
