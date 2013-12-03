using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_Client.Controller;
using WPF_Client.ViewModel;

namespace WPF_Client.Commands
{
    /// <summary>
    /// Command bound ment for the back button.
    /// </summary>
    class BackCommand : ICommand
    {

        public BackCommand()
        {

        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter){

            ViewModelManager.Main.CurrentViewModel = ViewModelManager.PreviousViewModelsStack.Pop();
            ViewModelManager.PreviousViewModelsStack.Pop(); // We do not want the previousviewmodels stack to save the viewmodel that the user presses back from.
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }
}
