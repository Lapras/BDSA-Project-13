using System;
using System.Windows.Input;
using WPF_Client.Commands;
using WPF_Client.Controller;
using DtoSubsystem;


namespace WPF_Client.ViewModel
{

    /// <summary>
    /// A ViewModel for the ActorProfileView.
    /// </summary>
    public class ActorProfileViewModel : ViewModelBase
    {

        private PersonDetailsDto _personDetailsDto;


        /// <summary>
        /// The command attached to the back button
        /// </summary>
        public ICommand BackCommand { get; set; } 
        

        /// <summary>
        /// The collection of Person results that is displayed in the view.
        /// </summary>
        public PersonDetailsDto PersonDetailsDto
        {
            get
            {
                return _personDetailsDto;
            }
            set
            {
                if (_personDetailsDto == value)
                    return;
                _personDetailsDto = value;

                OnPropertyChanged("PersonDetailsDto");
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ActorProfileViewModel()
        {
            PersonDetailsDto = HollywoodController.PersonDetailsDto;
            BackCommand = new BackCommand();
            
        }

    }

  
}
