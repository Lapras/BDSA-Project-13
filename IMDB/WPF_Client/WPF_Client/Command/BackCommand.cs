using System;
using System.Windows.Input;

namespace WPF_Client.Commands
{
    /// <summary>
    /// A Command for the back button.
    /// </summary>
    class BackCommand : ICommand
    {
        
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
