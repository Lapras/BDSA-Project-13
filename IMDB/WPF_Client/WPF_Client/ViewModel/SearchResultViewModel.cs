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

namespace WPF_Client.ViewModel
{

    /// <summary>
    /// ViewModel for the SearchResultView.
    /// </summary>
    class SearchResultViewModel : IViewModel
    {
        private Model.Model _model;
        private ObservableCollection<MovieSearchDto> _movieSearchDtos;
        public string SearchString { get; set; } //The string that will be searched with.
        public int SearchType { get; set; } //The type of search that should be conducted.

        public ICommand SelectMovieCommand { get; set; } //The command attached to clicking on a movie.

        /// <summary>
        /// The collection of movie results that is displayed in the view.
        /// </summary>
        public ObservableCollection<MovieSearchDto> MovieSearchDtos
        {
            get
            {
                return _movieSearchDtos;
            }
            set
            {
                if (_movieSearchDtos == value)
                    return;
                _movieSearchDtos = value;                
                OnPropertyChanged("MovieSearchDtos");
            }
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public SearchResultViewModel()
        {
            _model = new Model.Model();
            _movieSearchDtos = new ObservableCollection<MovieSearchDto>();

            SelectMovieCommand = new SelectMovieCommand(this);

            switch (Mediator.SearchType) //We check the search that should be conducted.
            {
                case 0: // Movies

                    Console.WriteLine(Mediator.SearchString);
                    MovieSearchDtos = _model.MovieSearchDtos(Mediator.SearchString);

                    //Console.WriteLine(MovieSearchDtos.Count());

                    break;

                case 1: // Actors

                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            
        }

    }


    //COMMANDS:

    /// <summary>
    /// Command bound to selecting a movie.
    /// </summary>
    class SelectMovieCommand : ICommand
    {
        private SearchResultViewModel _vm;


        public SelectMovieCommand(SearchResultViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            //return !string.IsNullOrEmpty(_vm.TextBox);
            return true;
        }

        public void Execute(object parameter)
        {
            
            MovieSearchDto dto;
            dto = (MovieSearchDto)parameter;

            Mediator._movieId = dto.Id;
            var movieProfileViewModel = new MovieProfileViewModel();

            ViewModelLocator.Main.CurrentViewModel = movieProfileViewModel;

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }

}
