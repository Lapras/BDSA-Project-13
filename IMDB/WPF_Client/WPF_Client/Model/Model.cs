using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;
using DtoSubsystem;
using WPF_Client.Storage;

namespace WPF_Client.Model
{
    public class Model : IModel
    {
        private Storage.Storage _storage;

        public Model()
        {
            _storage = new Storage.Storage(new RESTStrategy());
        }


        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {
            return _storage.MovieDtos(searchString);
        }

        public MovieProfileDto MovieProfileDto(int movieId)
        {
            return _storage.MovieDto(movieId);
        }
        

    }
}
