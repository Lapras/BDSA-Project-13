using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;

namespace WPF_Client.ViewModel
{
    class SearchResultViewModel : INotifyPropertyChanged, IViewModel
    {
        internal Model.Model _model;
        public ObservableCollection<MovieSearchDto> _movieSearchDtos;
        public string _searchtest;



        public ObservableCollection<MovieSearchDto> MovieSearchDtos
        {
            get
            {
                return _movieSearchDtos;
            }
            set
            {
                if (_movieSearchDtos == value)
                    return;
                _movieSearchDtos = value;

                //Console.WriteLine(value[0].Year + " " + value[0].Title);
                
                OnPropertyChanged("MovieSearchDtos");
                _movieSearchDtos.CollectionChanged += (sender, args) => OnPropertyChanged("MovieSearchDtos");
            }
        }




        /// <summary>
        /// Default constructor.
        /// </summary>
        public SearchResultViewModel()
        {
 

            MovieSearchDtos = _movieSearchDtos;
            //_model = new Model.Model();
            //MovieSearchDtos = _model.MovieSearchDtos(_searchtest);

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
