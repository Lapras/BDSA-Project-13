using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;

namespace WPF_Client.Storage
{

    /// <summary>
    /// Class responsible for the data in the WPF application. It has a cache which is always searched first.
    /// It then can have different types of storage by using the Strategy pattern.
    /// </summary>
    public class Storage
    {
        private IStorageStrategy _strategy;

        private Cache _cache;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public Storage(IStorageStrategy strategy)
        {
            _strategy = strategy;
            _cache = new Cache();
            
        }

        /// <summary>
        /// Retrieves the MovieDtos (dtos that a movie search returns)
        /// </summary>
        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {
            var cachedMovieDtos = _cache.MovieDtos(searchString);

            if (cachedMovieDtos == null)
            {
                var storageMovieDtos = _strategy.MovieDtos(searchString);
                _cache.AddMovieDtos(storageMovieDtos);

                return storageMovieDtos;
            }
            else
            {
                return cachedMovieDtos;
            }

        }

        /// <summary>
        /// Retrieves a MovieDetailDto (dto used when viewing information on a movie)
        /// </summary>
        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            var catchedMovieDetailDto = _cache.MovieDetailsDto(movieId);

            if (catchedMovieDetailDto == null)
            {
                var storageMovieDetailDto = _strategy.MovieDetailsDto(movieId);
                _cache.AddMovieDetailsDto(storageMovieDetailDto);

                return storageMovieDetailDto;
            }
            else
            {
                return catchedMovieDetailDto;
            }

        }


        /// <summary>
        /// Creates a profile in the storage.
        /// </summary>
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
