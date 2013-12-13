

namespace WPF_Client.ViewModel
{

    /// <summary>
    /// The MainWindowViewModel is a ViewModel for the MainWindow containing the 
    /// CurrentViewModel and the TopViewModel that change and updates UI.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// The current view.
        /// </summary>
        private ViewModelBase _currentViewModel;


        /// <summary>
        /// The toolbar.
        /// </summary>
        private ViewModelBase _topViewModel;

        /// <summary>
        /// The default constructor. We set the initial viewmodel to the SearchViewModel.
        /// </summary>
        public MainWindowViewModel()
        {
            TopViewModel = new LoginViewModel();
            CurrentViewModel = new SearchViewModel(); //We set the startup ViewModel to the SearchViewModel.
        }

        /// <summary>
        /// The CurrentView property. When the View is changed,
        /// we need to raise a property changed event.
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                ViewModelManager.PreviousViewModelsStack.Push(_currentViewModel);
                _currentViewModel = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }

        /// <summary>
        /// The TopViewModel property which is the "toolbar" in the UI.
        /// </summary>
        public ViewModelBase TopViewModel
        {
            get
            {
                return _topViewModel;
            }
            set
            {
                if (_topViewModel == value)
                    return;
                _topViewModel = value;
                OnPropertyChanged("TopViewModel");
            }

        }

    }

}
