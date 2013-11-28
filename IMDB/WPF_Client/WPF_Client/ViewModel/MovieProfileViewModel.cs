using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_Client.Dtos;
using WPF_Client.Model;
using WPF_Client.Controller;


namespace WPF_Client.ViewModel
{
    class MovieProfileViewModel : IViewModel
    {

        private MovieDto _movieDto;

        /// <summary>
        /// The collection of movie results that is displayed in the view.
        /// </summary>
        public MovieDto MovieDto
        {
            get
            {
                return _movieDto;
            }
            set
            {
                if (_movieDto == value)
                    return;
                _movieDto = value;

                OnPropertyChanged("MovieDto");
            }
        }


        public MovieProfileViewModel()
        {
            MovieDto = HollywoodController.MovieDto;
        }
    }
}
