using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using Newtonsoft.Json;
using WPF_Client.Exceptions;
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

        private ObjectCache _personDetailDtoCache;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Storage(IStorageStrategy strategy)
        {
            _strategy = strategy;
            //_cache = new Cache();

            _movieDtocache = MemoryCache.Default;
            _movieDetailDtocache = MemoryCache.Default;

            _personDetailDtoCache = MemoryCache.Default;

        }

        /// <summary>
        /// Retrieves the MovieDtos (dtos that a movie search returns)
        /// </summary>
        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {

            try
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
            catch (RESTserviceException e)
            {
                throw new StorageException("REST strategy unavailable.", e);
            }


            
        }

        /// <summary>
        /// Retrieves a MovieDetailDto (dto used when viewing information on a movie)
        /// </summary>
        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            try
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
                return (MovieDetailsDto)_movieDetailDtocache.Get(movieId.ToString());
            }
            catch (RESTserviceException e)
            {
               throw new StorageException();
            }
        }


        /// <summary>
        /// Creates a profile in the storage.
        /// </summary>
        public bool CreateProfile(string name, string password)
        {
            try
            {
                return _strategy.CreateProfile(name, password);
            }
            catch (RESTserviceException e)
            {
               throw new StorageException();
            }

        }

        
        public bool LoginInfo(string name, string password)
        {
            try
            {
                return _strategy.LoginInfo(name, password);
            }
            catch (RESTserviceException e)
            {
               throw new StorageException();
            }

        }

        public PersonDetailsDto PersonDetailsDto(int id)
        {
           try
           {
                if (_personDetailDtoCache.Get(id.ToString()) == null)
                {
                    Console.WriteLine("did not find in cache");

                    var storagePersonDetailDto = _strategy.PersonDetailsDto(id); // now we search in the strategy


                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.0); //10 sec

                    _personDetailDtoCache.Set(id.ToString(), storagePersonDetailDto, policy);

                    return storagePersonDetailDto;

                }
                Console.WriteLine("found in cache");
                return (PersonDetailsDto)_personDetailDtoCache.Get(id.ToString());
            }
            catch (RESTserviceException e)
            {
               throw new StorageException();
            }
        }

        public bool RateMovie(int movieId, int rating, string username)
        {
            try
            {
                var result = _strategy.RateMovie(movieId, rating, username);

                //if we have successfully rated we need to remove it from the cache
                //so that the old avg. rating is updated.
                if (result)
                {
                    _movieDetailDtocache.Remove(movieId.ToString());
                }

                return result;
            }
            catch (RESTserviceException e)
            {
                throw new StorageException();
            }








            
        }
    }
}
