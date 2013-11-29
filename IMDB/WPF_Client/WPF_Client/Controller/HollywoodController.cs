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
    public static class HollywoodController
    {
        public static IModel _model = new Model.Model();


        public static MovieDto MovieDto { get; set; } // The MovieDto that the SearchResultViewModel loads for the MovieDto for.

        public static bool GetMovie(int movieId)
        {
            MovieDto = _model.MovieDto(movieId);

            if (MovieDto == null)
            {
                return false;
            }

            ViewModelManager.Main.CurrentViewModel = new MovieProfileViewModel();

            return true;

        }
    }
}
