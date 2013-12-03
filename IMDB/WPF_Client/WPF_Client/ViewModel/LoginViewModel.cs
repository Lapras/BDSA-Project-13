using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Client.Commands;

namespace WPF_Client.ViewModel
{
    class LoginViewModel : IViewModel
    {
        private string _usernameTextBox; //The username inputtet by the user
        //private string _passwordTextBox; //The selected index from the combobox.
        public ICommand LoginCommand { get; set; } //The command attached to the Search button.
        public ICommand RegisterCommand { get; set; } //The command attached to the Search button.
        public ICommand BackCommand { get; set; }



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

        /* public string PasswordTextBox
         {
             get { return _passwordTextBox; }
             set
             {
                 if (_passwordTextBox == value)
                     return;
                 _passwordTextBox = value;

                 //Console.WriteLine(value);
                 //Console.WriteLine(ComboBoxSelectedIndex);

                 OnPropertyChanged("PasswordTextBox");
             }
         }*/

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
            return !string.IsNullOrEmpty(_vm.UsernameTextBox);
        }

        public void Execute(object parameter)
        {
            if (_vm.UsernameTextBox == "Simon")
            {
                MessageBox.Show("Wrong Username or Password");
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
