using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
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

        private ObjectCache _movieDtocache;
        private ObjectCache _movieDetailDtocache;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Storage(IStorageStrategy strategy)
        {
            _strategy = strategy;
            //_cache = new Cache();

            _movieDtocache = MemoryCache.Default;
            _movieDetailDtocache = MemoryCache.Default;

        }

        /// <summary>
        /// Retrieves the MovieDtos (dtos that a movie search returns)
        /// </summary>
        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {

            if (_movieDtocache.Get(searchString) == null)
            {
                Console.WriteLine("did not find in cache");

                var storageMovieDtos = _strategy.MovieDtos(searchString); // now we search in the strategy


                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(900.0); //15 min

                _movieDtocache.Set(searchString, storageMovieDtos, policy);

                return storageMovieDtos;

            }
            
            
            Console.WriteLine("found in cache");
            return (ObservableCollection<MovieDto>) _movieDtocache.Get(searchString);
            
        }

        /// <summary>
        /// Retrieves a MovieDetailDto (dto used when viewing information on a movie)
        /// </summary>
        public MovieDetailsDto MovieDetailsDto(int movieId)
        {

            if (_movieDetailDtocache.Get(movieId.ToString()) == null)
            {
                Console.WriteLine("did not find in cache");

                var storageMovieDetailDto = _strategy.MovieDetailsDto(movieId); // now we search in the strategy


                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.0); //10 sec

                _movieDetailDtocache.Set(movieId.ToString(), storageMovieDetailDto, policy);

                return storageMovieDetailDto;

            }


            Console.WriteLine("found in cache");
            return (MovieDetailsDto) _movieDetailDtocache.Get(movieId.ToString());

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
