using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.ViewModel
{
    class SearchResultViewModel : INotifyPropertyChanged, IViewModel
    {
        
        /// <summary>
        /// The current view.
        /// </summary>
        private IViewModel _currentViewModel;



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


        public SearchResultViewModel()
        {
            CurrentViewModel = this;

        }


        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
