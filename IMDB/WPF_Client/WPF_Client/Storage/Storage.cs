using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;
using DtoSubsystem;

namespace WPF_Client.Storage
{
    public class Storage
    {
        private IStorageStrategy _strategy;

        public Storage(IStorageStrategy strategy)
        {
            _strategy = strategy;
            
        }

        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {
            return _strategy.MovieDtos(searchString);
        }

        public MovieProfileDto MovieDto(int movieId)
        {
            return _strategy.MovieProfileDto(movieId);
        }

        public bool CreateProfile(string name, string password)
        {
            return _strategy.CreateProfile(name, password);

        }
    }
}
