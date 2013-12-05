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

        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }



}
