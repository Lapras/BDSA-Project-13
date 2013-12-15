using System;
using System.Collections.ObjectModel;
using System.Runtime.Caching;
using WPF_Client.Exceptions;
using DtoSubsystem;

namespace WPF_Client.Storage
{

    /// <summary>
    /// Class responsible for the data in the WPF application. It has a cache which is always searched first.
    /// It can have different types of storage by using the Strategy pattern e.g. databases or communicate with
    /// external databases using REST.
    /// </summary>
    public class Storage
    {
        private IStorageStrategy _strategy;

        /// <summary>
        /// Cache for MovieDtos.
        /// </summary>
        private ObjectCache _movieDtoCache;

        /// <summary>
        /// Cache for PersonDtos.
        /// </summary>
        private ObjectCache _personDtoCache;

        /// <summary>
        /// Cache for MovieDetailDtos.
        /// </summary>
        private ObjectCache _movieDetailDtoCache;

        /// <summary>
        /// Cache for PersonDetailDtos
        /// </summary>
        private ObjectCache _personDetailDtoCache;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public Storage(IStorageStrategy strategy)
        {
            _strategy = strategy;

            _movieDtoCache = new MemoryCache("movieDtoCache");
            _movieDetailDtoCache = new MemoryCache("movieDetailDtoCache");

            _personDtoCache = new MemoryCache("personDtoCache");
            _personDetailDtoCache = new MemoryCache("personDetailDtoCache");

        }

        /// <summary>
        /// Sets a new strategy.
        /// </summary>
        /// <param name="strategy">The input strategy.</param>
        public void SetStrategy(IStorageStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Retrieves the MovieDtos of the supplied searchstring. It always looks in its cache first.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns>The MovieDtos.</returns>
        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {

            try
            {
                if (_movieDtoCache.Get(searchString) == null)
                {

                    var storageMovieDtos = _strategy.MovieDtos(searchString); // now we search in the strategy


                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(900.0); //15 min

                    _movieDtoCache.Set(searchString, storageMovieDtos, policy);

                    return storageMovieDtos;

                }

                


                return (ObservableCollection<MovieDto>) _movieDtoCache.Get(searchString);

            }
            catch (RESTserviceException e)
            {
                throw new StorageException("REST strategy unavailable.", e);
            }

            
        }


        /// <summary>
        /// Retrieves the PersonDtos of the supplied searchstring. It always looks in its cache first.
        /// </summary>
        /// <param name="searchString">The search string.</param>
        /// <returns>The PersonDtos.</returns>
        public ObservableCollection<PersonDto> PersonDtos(string searchString)
        {

            try
            {
                if (_personDtoCache.Get(searchString) == null)
                {

                    var storagePersonDtos = _strategy.PersonDtos(searchString); // now we search in the strategy


                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(900.0); //15 min

                    _personDtoCache.Set(searchString, storagePersonDtos, policy);

                    return storagePersonDtos;

                }

                return (ObservableCollection<PersonDto>)_personDtoCache.Get(searchString);

            }
            catch (RESTserviceException e)
            {
                throw new StorageException("REST strategy unavailable.", e);
            }


        }


        /// <summary>
        /// Retrives the MovieDetailsDto of a movie. It always looks in its cache first.
        /// </summary>
        /// <param name="movieId">The movieId</param>
        /// <returns>A MovieDetailsDto.</returns>
        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            try
            {
                if (_movieDetailDtoCache.Get(movieId.ToString()) == null)
                {

                    var storageMovieDetailDto = _strategy.MovieDetailsDto(movieId); // now we search in the strategy


                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(5.0); //5 sec

                    _movieDetailDtoCache.Set(movieId.ToString(), storageMovieDetailDto, policy);

                    return storageMovieDetailDto;

                }

                return (MovieDetailsDto)_movieDetailDtoCache.Get(movieId.ToString());
            }
            catch (RESTserviceException e)
            {
               throw new StorageException();
            }
        }


        /// <summary>
        /// Creates a profile with the supplied name and password.
        /// </summary>
        /// <param name="name">The requested name.</param>
        /// <param name="password">The requested password.</param>
        /// <returns>A boolean value whether the account was created or not.</returns>
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

        /// <summary>
        /// Logs in the user with the supplied username and password.
        /// </summary>
        /// <param name="name">The input username.</param>
        /// <param name="password">The input password.</param>
        /// <returns>A boolean value whether the login was successfull or not.</returns>
        public bool Login(string name, string password)
        {
            try
            {
                return _strategy.Login(name, password);
            }
            catch (RESTserviceException e)
            {
               throw new StorageException();
            }

        }


        /// <summary>
        /// Retrieves a PersonDetailsDto with the supplied personId. It always looks in the cache first.
        /// </summary>
        /// <param name="id">The input personId.</param>
        /// <returns>The PersonDetailsDto</returns>
        public PersonDetailsDto PersonDetailsDto(int id)
        {
           try
           {
                if (_personDetailDtoCache.Get(id.ToString()) == null)
                {

                    var storagePersonDetailDto = _strategy.PersonDetailsDto(id); // now we search in the strategy


                    CacheItemPolicy policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10.0); //10 sec

                    _personDetailDtoCache.Set(id.ToString(), storagePersonDetailDto, policy);

                    return storagePersonDetailDto;

                }

                return (PersonDetailsDto)_personDetailDtoCache.Get(id.ToString());
            }
            catch (RESTserviceException e)
            {
               throw new StorageException();
            }
        }

        /// <summary>
        /// Rates a movie.
        /// </summary>
        /// <param name="id">The movieId.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="username">The username that rates the movie.</param>
        /// <returns></returns>
        public bool RateMovie(int movieId, int rating, string username)
        {
            try
            {
                var result = _strategy.RateMovie(movieId, rating, username);

                //if we have successfully rated we need to remove it from the cache
                //so that the old avg. rating is updated.
                if (result)
                {
                    _movieDetailDtoCache.Remove(movieId.ToString());
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
