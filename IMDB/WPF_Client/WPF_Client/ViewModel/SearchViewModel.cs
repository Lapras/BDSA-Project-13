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
        /// <summary>
        /// The current view.
        /// </summary>
        //private IViewModel _currentViewModel;


        private string _textBox;
        private int _comboBoxSelectedIndex;


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


        public ICommand SearchCommand { get; set; }


        /*
        /// <summary>
        /// The CurrentView property. When the View is changed,
        /// we need to raise a property changed event.
        /// </summary>
        public IViewModel CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;

                Console.WriteLine(value);

                OnPropertyChanged("CurrentViewModel");
            }
        }
        */

        
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
        /// Default constructor.  We set the initial view-model to 'FirstViewModel'.
        /// We also associate the commands with their execution actions.
        /// </summary>
        public SearchViewModel()
        {
            //CurrentViewModel = this;

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

            switch (_vm.ComboBoxSelectedIndex) //we check the dropdown box's selected item. (make field)
            {
                case 0: // Movies
                    Console.WriteLine("We search for movies");

                    //Test.main.CurrentViewModel = new SearchResultViewModel();

                    ViewModelLocator.Main.CurrentViewModel = new SearchResultViewModel();
                    //Storage.SearchMovie(_vm.TextBox);

                    //_vm.CurrentViewModel = new MainViewModel();

                    break;
                case 1: // Actors
                    Console.WriteLine("We search for actors");

                    //Storage.SearchActor(_vm.TextBox);


                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            


            //MessageBox.Show("muh");
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }








}
