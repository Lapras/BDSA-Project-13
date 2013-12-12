using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;
using WPF_Client.Storage;

namespace WPF_Client.Model
{
    /// <summary>
    /// The Model class represents the main business data of the application.
    /// </summary>
    public class Model : IModel
    {
        private Storage.Storage _storage;

        public Model()
        {
            _storage = new Storage.Storage(new RESTStrategy("http://localhost:54321"));
        }

        
        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {
            return _storage.MovieDtos(searchString);
        }

        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            return _storage.MovieDetailsDto(movieId);
        }

        public bool CreateProfile(string name, string password)
        {
            return _storage.CreateProfile(name, password);
        }

        public bool Login(string name, string password)
        {
            return _storage.Login(name, password);
        }

        public PersonDetailsDto PersonDetailsDto(int id)
        {
            return _storage.PersonDetailsDto(id);
        }

        public bool RateMovie(int id, int rating, string username)
        {
            return _storage.RateMovie(id, rating, username);
        }
    }
}
