using System.Collections.Generic;
using WPF_Client.ViewModel;

namespace WPF_Client
{

    /// <summary>
    /// Class which maintains a public reference to the MainWindowViewModel, so whenever the application should change view the MainWindowViewModel's CurrentViewModel should be changed to another ViewModel.
    /// </summary>
    public static class ViewModelManager
    {
        private static MainWindowViewModel _main;

        private static Stack<ViewModelBase> _previousViewModelsStack = new Stack<ViewModelBase>();

        /// <summary>
        /// A property for containing a stack of previous viewmodels that the user has viewed. 
        /// </summary>
        public static Stack<ViewModelBase> PreviousViewModelsStack
        {
            get
            {
                return _previousViewModelsStack;
            }
        }

        /// <summary>
        /// Retrives the MainWindowViewModel
        /// </summary>
        public static MainWindowViewModel Main
        {
            get
            {
                return _main;
            }
            set
            {
                _main = value;                
            }
        }
    }

    
}
