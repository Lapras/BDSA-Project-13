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
using WPF_Client.Model;
using WPF_Client.Controller;

namespace WPF_Client.ViewModel
{

    /// <summary>
    /// ViewModel for the SearchView.
    /// </summary>
    public class SearchViewModel : IViewModel
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

                //Console.WriteLine(value);
                //Console.WriteLine(ComboBoxSelectedIndex);

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


    }


    //COMMANDS:

    /// <summary>
    /// Command bound to the Search Button
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
            if (!SearchController.Search(_vm.TextBox, _vm.ComboBoxSelectedIndex))
            {
                MessageBox.Show("No results found for: " + _vm.TextBox, "No results");
            }
            else
            {
                ViewModelManager.Main.CurrentViewModel = new SearchResultViewModel();
            }



            /* old code
            switch (_vm.ComboBoxSelectedIndex) // We check the combobox's selected item.
            {
                case 0: // Movies
                    Console.WriteLine("We search for movies");

                    //Mediator.SearchString = _vm.TextBox;
                    //Mediator.SearchType = _vm.ComboBoxSelectedIndex;
                                       
                    


                    IModel _model = new Model.Model();
                    Mediator.SearchString = _vm.TextBox;
                    Mediator.test = _model.MovieSearchDtos(Mediator.SearchString);
                    Console.WriteLine(Mediator.SearchString);
                    var searchResultViewModel = new SearchResultViewModel(); 


                    //Console.WriteLine(Mediator.test[0].Title);
                    ViewModelManager.Main.CurrentViewModel = searchResultViewModel;

                    break;
                case 1: // Actors
                    Console.WriteLine("We search for actors");


                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            */
            
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }



}
