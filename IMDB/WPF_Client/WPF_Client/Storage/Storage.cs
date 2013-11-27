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
            //_strategy.getfes(searchString)
            return new ObservableCollection<MovieSearchDto>() { new MovieSearchDto() { Title = "hej", Year = 1999 } };

        }

    }
}
