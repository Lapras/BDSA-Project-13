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

        private MovieProfileDto _movieDto;

        public ICommand BackToSearchResultCommand { get; set; }

        /// <summary>
        /// The collection of movie results that is displayed in the view.
        /// </summary>
        public MovieProfileDto MovieDto
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

                OnPropertyChanged("MovieProfileDto");
            }
        }


        public MovieProfileViewModel()
        {
            MovieDto = HollywoodController.MovieDto;
            BackToSearchResultCommand = new BackToSearchResultCommand(this);
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
            ViewModelManager.Main.CurrentViewModel = new SearchResultViewModel();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }
}
