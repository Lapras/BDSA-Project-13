using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;

namespace WPF_Client.Storage
{
    public class Storage
    {
        private IStorageStrategy _strategy;

        public Storage(IStorageStrategy strategy)
        {
            _strategy = strategy;
            
        }

        public ObservableCollection<MovieSearchDto> MovieSearchDtos(string searchString)
        {
            return _strategy.MovieSearchDtos(searchString);
        }

        public MovieDto MovieDto(int movieId)
        {
            return _strategy.MovieDto(movieId);
        }

    }
}
