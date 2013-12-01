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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPF_Client.Dtos;
using WPF_Client.Model;
using WPF_Client.Controller;

using DtoSubsystem;

namespace WPF_Client.ViewModel
{

    /// <summary>
    /// ViewModel for the MovieSearchResultView.
    /// </summary>
    public class MovieSearchResultViewModel : IViewModel
    {
        private ObservableCollection<MovieDto> _movieDtos;
        private int _moviesFound;
        
        public ICommand SelectMovieCommand { get; set; } //The command attached to clicking on a movie.
        public ICommand BackToSearchCommand { get; set; }
        /// <summary>
        /// The collection of movie results that is displayed in the view.
        /// </summary>
        public ObservableCollection<MovieDto> MovieDtos
        {
            get
            {
                return _movieDtos;
            }
            set
            {
                if (_movieDtos == value)
                    return;
                _movieDtos = value;                
                OnPropertyChanged("MovieDtos");
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
        public MovieSearchResultViewModel()
        {
            SelectMovieCommand = new SelectMovieCommand(this);
            BackToSearchCommand = new BackToSearchCommand(this);

            MovieDtos = SearchController.MovieDtos;
            MoviesFound = SearchController.MoviesFound;

        }

    }


    //COMMANDS:

    /// <summary>
    /// Command bound to selecting a movie.
    /// </summary>
    class SelectMovieCommand : ICommand
    {
        private MovieSearchResultViewModel _vm;


        public SelectMovieCommand(MovieSearchResultViewModel vm)
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
            
            MovieDto dto;
            dto = (MovieDto)parameter;

            //Mediator.MovieId = dto.Id;
            //HollywoodController.MovieId = dto.Id;

            if (!HollywoodController.GetMovie(dto.Id))
            {
                MessageBox.Show("Could not find movie");
            }

            

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }


    /// <summary>
    /// Command bound to the Search Button
    /// </summary>
    class BackToSearchCommand : ICommand
    {
        private MovieSearchResultViewModel _vm;


        public BackToSearchCommand(MovieSearchResultViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModelManager.Main.CurrentViewModel = new SearchViewModel();


        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }

}
