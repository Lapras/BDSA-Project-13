﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using WPF_Client.Commands;
using WPF_Client.Exceptions;
using WPF_Client.Controller;

using DtoSubsystem;

namespace WPF_Client.ViewModel
{

    /// <summary>
    /// ViewModel for the MovieSearchResultView.
    /// </summary>
    public class MovieSearchResultViewModel : ViewModelBase
    {

        private ObservableCollection<MovieDto> _movieDtos;
        private int _moviesFound;
        
        /// <summary>
        /// The command attached to clicking on a movie.
        /// </summary>
        public ICommand SelectMovieCommand { get; set; }

        /// <summary>
        /// The command attached to the back button.
        /// </summary>
        public ICommand BackCommand { get; set; }
        
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
            BackCommand = new BackCommand();
            

            MovieDtos = SearchController.MovieDtos;
            MoviesFound = SearchController.MoviesFound;

        }

    }


    //COMMANDS:

    /// <summary>
    /// Command bound to selecting a movie.
    /// </summary>
    public class SelectMovieCommand : ICommand
    {
        private MovieSearchResultViewModel _vm;


        public SelectMovieCommand(MovieSearchResultViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            
            MovieDto dto;
            dto = (MovieDto)parameter;

            try
            {
                if (!HollywoodController.GetMovie(dto.Id))
                {

                    MessageBox.Show("Could not find movie", "No results", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (StorageException e)
            {
                MessageBox.Show("Data error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnavailableConnectionException e)
            {
                MessageBox.Show("There is no connection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show("Data error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }






}
