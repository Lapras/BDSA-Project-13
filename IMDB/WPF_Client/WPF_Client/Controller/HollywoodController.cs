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

    public static class HollywoodController
    {
        public static IModel _model = new Model.Model();

        public static PersonDetailsDto PersonDetailsDto { get; set; }
        public static MovieDetailsDto MovieDetailsDto { get; set; } // The MovieDto that the SearchResultViewModel loads for the MovieDto for.

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
    }
}
