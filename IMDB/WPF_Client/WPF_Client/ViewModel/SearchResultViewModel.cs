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

    /// <summary>
    /// ViewModel for the SearchResultView.
    /// </summary>
    public class SearchResultViewModel : IViewModel
    {
        private IModel _model;
        private ObservableCollection<MovieSearchDto> _movieSearchDtos;
        private int _moviesFound;
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
        /// The collection of movie results that is displayed in the view.
        /// </summary>
        public int MoviesFound
        {
            get
            {
                return _moviesFound;
            }
            set
            {
                if (_moviesFound == value)
                    return;
                _moviesFound = value;
                OnPropertyChanged("MoviesFound");
            }
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public SearchResultViewModel()
        {
            _model = new Model.Model();
            SelectMovieCommand = new SelectMovieCommand(this);

            Search();
            
        }

        /// <summary>
        /// Constructor which takes in an IModel
        /// </summary>
        public SearchResultViewModel(IModel model)
        {
            _model = model;
            SelectMovieCommand = new SelectMovieCommand(this);

            Search();

        }



        public void Search()
        {
            switch (Mediator.SearchType) // We check the search that should be conducted.
            {
                case 0: // Movies
                    MovieSearchDtos = _model.MovieSearchDtos(Mediator.SearchString);
                    MoviesFound = MovieSearchDtos.Count();
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

            Mediator.MovieId = dto.Id;
            var movieProfileViewModel = new MovieProfileViewModel();

            ViewModelManager.Main.CurrentViewModel = movieProfileViewModel;

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }

}
