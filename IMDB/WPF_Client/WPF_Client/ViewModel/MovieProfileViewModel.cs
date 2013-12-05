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
using WPF_Client.Commands;
using WPF_Client.Dtos;
using WPF_Client.Model;
using WPF_Client.Controller;
using DtoSubsystem;


namespace WPF_Client.ViewModel
{
    class MovieProfileViewModel : IViewModel
    {

        private MovieDetailsDto _movieDetailsDto;

        //public ICommand BackToSearchResultCommand { get; set; }
        public ICommand BackCommand { get; set; }

        /// <summary>
        /// The collection of movie results that is displayed in the view.
        /// </summary>
        public MovieDetailsDto MovieDetailsDto
        {
            get
            {
                return _movieDetailsDto;
            }
            set
            {
                if (_movieDetailsDto == value)
                    return;
                _movieDetailsDto = value;

                OnPropertyChanged("MovieDetailsDto");
            }
        }


        public MovieProfileViewModel()
        {
            MovieDetailsDto = HollywoodController.MovieDetailsDto;

            Console.WriteLine(MovieDetailsDto.Kind + " " + MovieDetailsDto.Title);
            //BackToSearchResultCommand = new BackToSearchResultCommand(this);
            BackCommand = new BackCommand();
        }
    }





    // COMMANDS:

    /// <summary>
    /// Command bound to the Search Button
    /// </summary>
    class BackToSearchResultCommand : ICommand
    {
        private MovieProfileViewModel _vm;


        public BackToSearchResultCommand(MovieProfileViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModelManager.Main.CurrentViewModel = new MovieSearchResultViewModel();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }
}
