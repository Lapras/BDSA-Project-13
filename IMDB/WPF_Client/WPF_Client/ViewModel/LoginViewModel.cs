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
    /// ViewModel for the LoginView
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private string _usernameTextBox; //The username inputtet by the user
        private string _passwordBox; //The password from the user.
        public ICommand LoginCommand { get; set; } //The command attached to the Search button.
        public ICommand RegisterCommand { get; set; } //The command attached to the Search button.
        public ICommand BackCommand { get; set; }//The command attached to the Back button.



        /// <summary>
        /// Property for the username textbox.
        /// </summary>
        public string UsernameTextBox
        {
            get { return _usernameTextBox; }
            set
            {
                if (_usernameTextBox == value)
                    return;
                _usernameTextBox = value;
                OnPropertyChanged("UsernameTextBox");
            }
        }

        /// <summary>
        /// Property for the password textbox.
        /// </summary>
        public string PasswordBox
        {
             get { return _passwordBox; }
             set
             {
                 if (_passwordBox == value)
                     return;
                 _passwordBox = value;
                 OnPropertyChanged("PasswordTextBox");
             }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LoginViewModel()
        {
            LoginCommand = new LoginCommand(this);
            RegisterCommand = new RegisterCommand(this);
            BackCommand = new BackCommand();
        }

    }


    class LoginCommand : ICommand
    {
        private LoginViewModel _vm;


        public LoginCommand(LoginViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_vm.UsernameTextBox) && !string.IsNullOrEmpty(_vm.PasswordBox);
        }

        public void Execute(object parameter)
        {
            try
            {
                if (SessionController.LoginInfo(_vm.UsernameTextBox, _vm.PasswordBox))
                {
                    MessageBox.Show("Login succes: You are now logged in to the system :).", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    MessageBox.Show("Login failed: You entered the wrong username or password.","Wrong information",MessageBoxButton.OK, MessageBoxImage.Information);
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

    class RegisterCommand : ICommand
    {
        private LoginViewModel _vm;


        public RegisterCommand(LoginViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModelManager.Main.CurrentViewModel = new CreateProfileViewModel();

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }
}
