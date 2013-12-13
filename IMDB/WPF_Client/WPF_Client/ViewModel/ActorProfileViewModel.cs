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

    /// <summary>
    /// A ViewModel for the ActorProfileView.
    /// </summary>
    public class ActorProfileViewModel : ViewModelBase
    {

        private PersonDetailsDto _personDetailsDto;
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

            Console.WriteLine(PersonDetailsDto.Name + " " + PersonDetailsDto.Gender);
            BackCommand = new BackCommand();
            
        }



    }

    
  
}
