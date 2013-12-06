using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;

namespace WPF_Client.Storage
{
    public class Storage
    {
        private IStorageStrategy _strategy;

        private Cache _cache;


        public Storage(IStorageStrategy strategy)
        {
            _strategy = strategy;
            _cache = new Cache();
            
        }

        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {


            return _strategy.MovieDtos(searchString);
        }


        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            return _strategy.MovieDetailsDto(movieId);
        }

        public bool CreateProfile(string name, string password)
        {
            return _strategy.CreateProfile(name, password);

        }

        public bool LoginInfo(string name, string password)
        {
            return _strategy.LoginInfo(name, password);

        }
    }
}
