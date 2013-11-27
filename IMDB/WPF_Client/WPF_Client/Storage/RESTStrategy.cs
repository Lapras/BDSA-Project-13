using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;

namespace WPF_Client.Storage
{
    class RESTStrategy : IStorageStrategy
    {
        public ObservableCollection<MovieSearchDto> MovieSearchDtos(string searchString)
        {
            throw new NotImplementedException();
        }
    }
}
