using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
//using WPF_Client.Annotations;
using System.Globalization;

namespace WPF_Client.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged, IViewModel
    {
        /// <summary>
        /// The current view.
        /// </summary>
        private IViewModel _currentViewModel;


        /// <summary>
        /// The default constructor. We set the initial viewmodel to the SearchViewModel.
        /// </summary>
        public MainWindowViewModel()
        {
            CurrentViewModel = new SearchViewModel(); //We set the startup ViewModel to the SearchViewModel.
        }  

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
                OnPropertyChanged("CurrentViewModel");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }



    }


}
