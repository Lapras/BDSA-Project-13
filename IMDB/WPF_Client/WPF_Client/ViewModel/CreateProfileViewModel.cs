using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Client.Commands;
using WPF_Client.Controller;
using WPF_Client.Exceptions;

namespace WPF_Client.ViewModel
{

    /// <summary>
    /// ViewModel for the CreateProfileView.
    /// </summary>
    class CreateProfileViewModel : ViewModelBase
    {
        private string _usernameTextBox; //The text input from the user from the username-textbox.
        private string _passwordTextBox; //The text input from the user from the password-textbox.
        public ICommand CreateCommand { get; set; } //The command attached to the create button.
        public ICommand BackCommand { get; set; } //The command attached to the create button.

        /// <summary>
        /// The TextBox property. Which is the text input from the user from the textbox.
        /// </summary>
        public string UsernameTextBox
        {
            get { return _usernameTextBox; }
            set
            {
                if (_usernameTextBox == value)
                    return;
                _usernameTextBox = value;

                //Console.WriteLine(value);
                //Console.WriteLine(ComboBoxSelectedIndex);

                OnPropertyChanged("UsernameTextBox");
            }
        }

        /// <summary>
        /// The TextBox property. Which is the text input from the user from the textbox.
        /// </summary>
        public string PasswordTextBox
        {
            get { return _passwordTextBox; }
            set
            {
                if (_passwordTextBox == value)
                    return;
                _passwordTextBox = value;

                OnPropertyChanged("PasswordTextBox");
            }
        }


        public CreateProfileViewModel()
        {
            CreateCommand = new CreateCommand(this);
            BackCommand = new BackCommand();
        }


    }


    //COMMANDS:

    /// <summary>
    /// Command bound to the Search Button
    /// </summary>
    class CreateCommand : ICommand
    {
        private CreateProfileViewModel _vm;


        public CreateCommand(CreateProfileViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_vm.UsernameTextBox) && !string.IsNullOrEmpty(_vm.PasswordTextBox);
        }

        public void Execute(object parameter)
        {
            try
            {
                if (UserProfileController.CreateProfile(_vm.UsernameTextBox, _vm.PasswordTextBox))
                {
                    MessageBox.Show(_vm.UsernameTextBox + " successfully created!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(_vm.UsernameTextBox + " already taken :(.", "Username already taken.", MessageBoxButton.OK, MessageBoxImage.Information);
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
