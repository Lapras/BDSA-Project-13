using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Client.Commands;
using WPF_Client.Exceptions;
using WPF_Client.Model;
using WPF_Client.Controller;
using DtoSubsystem;


namespace WPF_Client.ViewModel
{
    class MovieProfileViewModel : ViewModelBase
    {

        private MovieDetailsDto _movieDetailsDto;

        //public ICommand BackToSearchResultCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand SelectActorCommand { get; set; }

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

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MovieProfileViewModel()
        {
            MovieDetailsDto = HollywoodController.MovieDetailsDto;

            Console.WriteLine(MovieDetailsDto.Kind + " " + MovieDetailsDto.Title);
            //BackToSearchResultCommand = new BackToSearchResultCommand(this);
            BackCommand = new BackCommand();
            SelectActorCommand = new SelectActorCommand(this);
        }
    }



    /// <summary>
    /// Command bound to selecting a movie.
    /// </summary>
    class SelectActorCommand : ICommand
    {
        private MovieProfileViewModel _vm;


        public SelectActorCommand(MovieProfileViewModel vm)
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
            Console.WriteLine("clicking");


            PersonDto dto;
            dto = (PersonDto)parameter;

            //Mediator.MovieId = dto.Id;
            //HollywoodController.MovieId = dto.Id;
            try
            {
                if (!HollywoodController.GetActor(dto.Id))
                {
                    Console.WriteLine("could not find actor");
                    MessageBox.Show("Could not find actor", "No results", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (StorageException e)
            {
                MessageBox.Show("Data error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnavailableConnection e)
            {
                MessageBox.Show("There is no connection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }







}
