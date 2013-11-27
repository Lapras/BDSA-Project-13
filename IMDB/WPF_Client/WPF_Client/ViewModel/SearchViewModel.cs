using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF_Client.Dtos;

namespace WPF_Client.ViewModel
{
    public class SearchViewModel : INotifyPropertyChanged, IViewModel
    {

        internal Model.Model _model;
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
            _model = new Model.Model();
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

                    SearchResultViewModel searchResultViewModel = new SearchResultViewModel();


                    var result = _vm._model.MovieSearchDtos("fe");


                    searchResultViewModel._movieSearchDtos = result;



                    ViewModelLocator.Main.CurrentViewModel = searchResultViewModel;

                    //searchResultViewModel._searchString = "hey";

                    /*
                    if (searchResultViewModel.MovieSearchDtos.Count() == 0)
                    {
                        MessageBox.Show("muh");
                    }
                    else
                    {
                        ViewModelLocator.Main.CurrentViewModel = searchResultViewModel;

                    }
                    */


                    break;
                case 1: // Actors
                    Console.WriteLine("We search for actors");

                    //Storage.SearchActor(_vm.TextBox);


                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            
            
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }



}
