using System;
using System.Windows;
using System.Windows.Input;
using WPF_Client.Controller;

namespace WPF_Client.ViewModel
{

    /// <summary>
    /// The TopSearchViewModel is a ViewModel for the TopSearchView. Containing the logout function together with search bar.
    /// </summary>
    internal class TopSearchViewModel : ViewModelBase
    {
        private string _textBox; //The text input from the user from the textbox.
        private int _comboBoxSelectedIndex; //The selected index from the combobox.

        /// <summary>
        /// The command attached to the Search button.
        /// </summary>
        public ICommand TopSearchCommand { get; set; }

        /// <summary>
        /// The command attached to the Logout button.
        /// </summary>
        public ICommand LogoutCommand { get; set; }


        /// <summary>
        /// The TextBox property. Which is the text input from the user from the textbox.
        /// </summary>
        public string TextBox
        {
            get { return _textBox; }
            set
            {
                if (_textBox == value)
                    return;
                _textBox = value;

                OnPropertyChanged("TextBox");
            }
        }

        /// <summary>
        /// The ComboBoxSelectedIndex property. Which is the selected index from the combobox.
        /// </summary>
        public int ComboBoxSelectedIndex
        {
            get
            {
                return _comboBoxSelectedIndex;
            }
            set
            {
                if (_comboBoxSelectedIndex == value)
                    return;
                _comboBoxSelectedIndex = value;
                OnPropertyChanged("ComboBoxSelectedIndex");
            }
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public TopSearchViewModel()
        {
            
            TopSearchCommand = new TopSearchCommand(this);
            LogoutCommand = new LogoutCommand(this);
            ComboBoxSelectedIndex = 0; // This does that "Movies" is the selected from the start.
        }

    }

    //COMMANDS:

    /// <summary>
    /// Command bound to the Search Button
    /// </summary>
    class TopSearchCommand : ICommand
    {
        private TopSearchViewModel _vm;


        public TopSearchCommand(TopSearchViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_vm.TextBox);
        }

        public void Execute(object parameter)
        {
            if (!SearchController.Search(_vm.TextBox, _vm.ComboBoxSelectedIndex))
            {
                MessageBox.Show("No results found for: " + _vm.TextBox, "No results");
            }

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }
    /// <summary>
    /// The command bound to the logout button, to log out the user.
    /// </summary>
    class LogoutCommand : ICommand
    {
        private TopSearchViewModel _vm;


        public LogoutCommand(TopSearchViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return (SessionController._isLoggedIn);
        }

        public void Execute(object parameter)
        {
            if (SessionController._isLoggedIn)
            {
                SessionController.Logout();
                MessageBox.Show("You have been logged out","Logged out");
            }
            else
            {
                MessageBox.Show("You have to be logged in before you can log out","Mismatch" );
            }
            
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }
    
}
