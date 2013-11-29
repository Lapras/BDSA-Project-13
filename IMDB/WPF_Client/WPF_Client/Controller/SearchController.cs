using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;
using WPF_Client.Model;
using WPF_Client.ViewModel;

namespace WPF_Client.Controller
{
    public static class SearchController
    {
        public static IModel _model = new Model.Model();

        public static ObservableCollection<MovieSearchDto> MovieSearchDtos { get; set; }
        public static int MoviesFound { get; set; }

        public static bool Search(string searchString, int searchType)
        {
            switch (searchType) // We check the search that should be conducted.
            {
                case 0: // Movies
                    MovieSearchDtos = _model.MovieSearchDtos(searchString);
                    MoviesFound = MovieSearchDtos.Count();

                    if (MoviesFound == 0)
                    {
                        return false;
                    }

                    ViewModelManager.Main.CurrentViewModel = new SearchResultViewModel();

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
