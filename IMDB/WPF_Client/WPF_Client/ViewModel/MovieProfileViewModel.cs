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


namespace WPF_Client.ViewModel
{
    class MovieProfileViewModel : IViewModel
    {
        private IModel _model;
        

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

                //Console.WriteLine(value.Title + " " + value.Year);

                OnPropertyChanged("MovieDto");
            }
        }


        public MovieProfileViewModel()
        {
            Console.WriteLine("MovieProfileViewModel created");
            _model = new Model.Model();
            _movieDto = _model.MovieDto(Mediator.MovieId);
        }
    }
}
