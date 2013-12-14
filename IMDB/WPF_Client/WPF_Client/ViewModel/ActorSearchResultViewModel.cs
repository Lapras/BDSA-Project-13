using System;
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
    /// ViewModel for the ActorSearchResultView.
    /// </summary>
    public class ActorSearchResultViewModel : ViewModelBase
    {

        private ObservableCollection<PersonDto> _personDtos;
        private int _personsFound;
        
        /// <summary>
        /// The command attached to clicking on a person.
        /// </summary>
        public ICommand SelectActorInSearchResultViewCommand { get; set; }

        /// <summary>
        /// The command attached to the back button.
        /// </summary>
        public ICommand BackCommand { get; set; }
        
        /// <summary>
        /// The collection of person results that is displayed in the view.
        /// </summary>
        public ObservableCollection<PersonDto> PersonDtos
        {
            get
            {
                return _personDtos;
            }
            set
            {
                if (_personDtos == value)
                    return;
                _personDtos = value;                
                OnPropertyChanged("PersonDtos");
            }
        }

        /// <summary>
        /// The collection of person results that is displayed in the view.
        /// </summary>
        public int PersonsFound
        {
            get
            {
                return _personsFound;
            }
            set
            {
                if (_personsFound == value)
                    return;
                _personsFound = value;
                OnPropertyChanged("PersonsFound");
            }
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public ActorSearchResultViewModel()
        {
            SelectActorInSearchResultViewCommand = new SelectActorInSearchResultViewCommand(this);
            BackCommand = new BackCommand();
            

            PersonDtos = SearchController.PersonDtos;
            PersonsFound = SearchController.PersonsFound;

        }

    }


    //COMMANDS:

    /// <summary>
    /// Command bound to selecting an actor.
    /// </summary>
    public class SelectActorInSearchResultViewCommand : ICommand
    {
        private ActorSearchResultViewModel _vm;


        public SelectActorInSearchResultViewCommand(ActorSearchResultViewModel vm)
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

                    MessageBox.Show("Could not find actor/actress", "No results", MessageBoxButton.OK,
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
