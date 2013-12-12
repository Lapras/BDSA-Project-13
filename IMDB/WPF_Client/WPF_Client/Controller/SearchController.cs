using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;
using WPF_Client.Model;
using WPF_Client.ViewModel;
using WPF_Client.Storage;

namespace WPF_Client.Controller
{
    /// <summary>
    /// The SearchController is responsible for the control flow of the application whenever
    /// the client searches for actors or movies.
    /// </summary>
    public static class SearchController
    {
        public static IModel _model = new Model.Model();
        public static ObservableCollection<MovieDto> MovieDtos { get; set; }
        public static int MoviesFound { get; set; }

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

                    Console.WriteLine("Searching for: " + searchString);
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

                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            return true;
        }

    }
}
