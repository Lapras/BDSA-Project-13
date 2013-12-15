using System;
using System.Windows;
using System.Windows.Input;
using WPF_Client.Commands;
using WPF_Client.Exceptions;
using WPF_Client.Controller;
using DtoSubsystem;


namespace WPF_Client.ViewModel
{
    /// <summary>
    /// ViewModel for the MovieProfileView.
    /// </summary>
    public class MovieProfileViewModel : ViewModelBase
    {

        private MovieDetailsDto _movieDetailsDto;

        /// <summary>
        /// The command attached to the back button.
        /// </summary>
        public ICommand BackCommand { get; set; }

        /// <summary>
        /// The command attached when clicking on an actor.
        /// </summary>
        public ICommand SelectActorCommand { get; set; }

        /// <summary>
        /// The command attached to the rate button.
        /// </summary>
        public ICommand RateCommand { get; set; }



        private int _selectedRating;

        /// <summary>
        /// The collection of movie results that is displayed in the view.
        /// </summary>
        public int SelectedRating
        {
            get
            {
                return _selectedRating;
            }
            set
            {
                if (_selectedRating == value)
                    return;
                _selectedRating = value;

                OnPropertyChanged("SelectedRating");
            }
        }


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

            BackCommand = new BackCommand();
            SelectActorCommand = new SelectActorCommand(this);
            RateCommand = new RateCommand(this);
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
            return true;
        }

        public void Execute(object parameter)
        {
            PersonDto dto;
            dto = (PersonDto)parameter;

            try
            {
                if (!HollywoodController.GetActor(dto.Id))
                {

                    MessageBox.Show("Could not find actor", "No results", MessageBoxButton.OK,
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



    /// <summary>
    /// Command bound to selecting a movie.
    /// </summary>
    class RateCommand : ICommand
    {
        private MovieProfileViewModel _vm;


        public RateCommand(MovieProfileViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return SessionController.IsLoggedIn() && _vm.SelectedRating != 0;

        }

        public void Execute(object parameter)
        {
            try
            {
                if (HollywoodController.RateMovie(_vm.MovieDetailsDto.Id, _vm.SelectedRating))
                {
                    MessageBox.Show("Successfully rated.", "Success!");
                    _vm.MovieDetailsDto = HollywoodController.MovieDetailsDto;
                }
                else
                {
                    MessageBox.Show("There was an error rating.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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



        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }



}
