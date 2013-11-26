using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Client.ViewModel
{
    public class SearchViewModel : INotifyPropertyChanged, IViewModel
    {        

        private string _textBox; //The text input from the user from the textbox.
        private int _comboBoxSelectedIndex; //The selected index from the combobox.
        public ICommand SearchCommand { get; set; } //The command attached to the Search button.

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
                Console.WriteLine(value);

                Console.WriteLine(ComboBoxSelectedIndex);
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
        public SearchViewModel()
        {
            SearchCommand = new SearchCommand(this);
            ComboBoxSelectedIndex = 0; // This does that "Movies" is the selected from the start.
        }   


        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }


    //COMMANDS:

    /// <summary>
    /// Command attached to the Search Button
    /// </summary>
    class SearchCommand : ICommand
    {
        private SearchViewModel _vm;


        public SearchCommand(SearchViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_vm.TextBox);
        }

        public void Execute(object parameter)
        {

            switch (_vm.ComboBoxSelectedIndex) //We check the combobox's selected item.
            {
                case 0: // Movies
                    Console.WriteLine("We search for movies");
                    //Storage.SearchMovie(_vm.TextBox);

                    ViewModelLocator.Main.CurrentViewModel = new SearchResultViewModel();


                    break;
                case 1: // Actors
                    Console.WriteLine("We search for actors");

                    //Storage.SearchActor(_vm.TextBox);


                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            
            MessageBox.Show("No results found :(");
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }



}
