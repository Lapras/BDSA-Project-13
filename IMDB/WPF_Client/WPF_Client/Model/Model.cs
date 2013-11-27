using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;
using WPF_Client.Storage;

namespace WPF_Client.Model
{
    class Model
    {
        private Storage.Storage _storage;

        public Model()
        {
            _storage = new Storage.Storage(new RESTStrategy());
        }


        public ObservableCollection<MovieSearchDto> MovieSearchDtos(string searchString)
        {
            return _storage.MovieSearchDtos(searchString);

        }


        /*
        public MovieDto MovieDtos(int id)
        {
            //storage.getfes(id)

        }
        */

    }
}
