using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;
using ASP_Client.Models;
using DtoSubsystem;
using Newtonsoft.Json;
using ASP_Client.Exceptions;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// Reponsible to Communicate with the application server holding the data
    /// </summary>
    public class RestCommunicationFacade : ICommunicationFacade
    {
        /// <summary>
        /// Get a list of movies maching the search string
        /// </summary>
        /// <param name="searchString">Movie to look for</param>
        /// <returns>List of movies matching the search string</returns>
        public async Task<List<MovieDto>> GetMoviesAsync(string searchString)
        {
            List<MovieDto> desiredMovies;
            CacheHelper.GetItem(searchString, out desiredMovies);

            if (desiredMovies != null) return desiredMovies;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var receivedData = JsonConvert.DeserializeObject<List<MovieDto>>(
                        await httpClient.GetStringAsync("http://localhost:54321/movies/?title=" + searchString)
                        );
                    if (receivedData.First().ErrorMsg.IsEmpty())
                    {
                        foreach (var movie in receivedData)
                        {
                            CacheHelper.AddItem(movie, movie.Title);
                        }
                    }
                    return receivedData;
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(ex => ex.GetType() == typeof(HttpRequestException)))
                {
                    throw new UnavailableConnectionException("No connection", e);
                }

                throw new RESTserviceException("AggregateException response from DB.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("There was an serializaton or deserializaton error", e);
            }
            catch (WebException)
            {
                return new List<MovieDto> { new MovieDto {ErrorMsg = "Application server unavailable"}};
            }
            catch (SocketException)
            {
                return new List<MovieDto> { new MovieDto {ErrorMsg = "Application server unavailable" }};
            }
            catch (HttpRequestException)
            {
                return new List<MovieDto> { new MovieDto {ErrorMsg = "Application server unavailable" }};
            }   

        }

        /// <summary>
        /// Get detailed information about a specific movie
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        public async Task<MovieDetailsDto> GetMovieDetailsLocallyAsync(int movieId)
        {
            MovieDetailsDto desiredMovie;
            CacheHelper.GetItem("" + movieId, out desiredMovie);

            if (desiredMovie != null) return desiredMovie;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var receivedData = JsonConvert.DeserializeObject<MovieDetailsDto>(
                        await httpClient.GetStringAsync("http://localhost:54321/movies/?movieId=" + movieId)
                        );

                    if (receivedData.ErrorMsg.IsEmpty())
                    {
                        CacheHelper.AddItem(receivedData, "" + receivedData.Id);
                    }
                    return receivedData;
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(ex => ex.GetType() == typeof(HttpRequestException)))
                {
                    throw new UnavailableConnectionException("No connection", e);
                }

                throw new RESTserviceException("AggregateException response from DB.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (JsonReaderException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (WebException)
            {
                return new MovieDetailsDto {ErrorMsg = "Application server unavailable"};
            }
            catch(SocketException)
            {
                return new MovieDetailsDto { ErrorMsg = "Application server unavailable" };
            }
            catch(HttpRequestException)
            {
                return new MovieDetailsDto { ErrorMsg = "Application server unavailable" };
            }   

        }

        /// <summary>
        /// Get a list of actors matching the given name
        /// </summary>
        /// <param name="personName">Name of the person to look for</param>
        /// <returns>List of the people matching the given name</returns>
        public async Task<List<PersonDto>> GetPersonAsync(string personName)
        {
            List<PersonDto> desiredPeople;
            CacheHelper.GetItem(personName, out desiredPeople);

            if (desiredPeople != null) return desiredPeople;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var receivedData = JsonConvert.DeserializeObject<List<PersonDto>>(
                        await httpClient.GetStringAsync("http://localhost:54321/person/?person=" + personName)
                        );
                    if (receivedData.First().ErrorMsg.IsEmpty())
                    {
                        foreach (var person in receivedData)
                        {
                            CacheHelper.AddItem(person, person.Name);
                        }
                    }
                    return receivedData;
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(ex => ex.GetType() == typeof(HttpRequestException)))
                {
                    throw new UnavailableConnectionException("No connection", e);
                }

                throw new RESTserviceException("AggregateException response from DB.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("There was an serializaton or deserializaton error", e);
            }
            catch (WebException)
            {
                return new List<PersonDto> { new PersonDto { ErrorMsg = "Database not available" } };
            }
            catch (SocketException)
            {
                return new List<PersonDto> { new PersonDto { ErrorMsg = "Database not available" } };
            }
            catch (HttpRequestException)
            {
                return new List<PersonDto> { new PersonDto { ErrorMsg = "Database not available" } };
            }   
        }

        /// <summary>
        /// Get the details of a specific person involved in the movie business
        /// </summary>
        /// <param name="personId">Id of the person to get the data of</param>
        /// <returns>Detailed data of the person</returns>
        public async Task<PersonDetailsDto> GetPersonDetailsLocallyAsync(int personId)
        {
            PersonDetailsDto desiredPerson;
            CacheHelper.GetItem("" + personId, out desiredPerson);

            if (desiredPerson != null) return desiredPerson;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.GetStringAsync("http://localhost:54321/person/?personId=" + personId);
                    var receivedData = JsonConvert.DeserializeObject<PersonDetailsDto>(result);

                    if (receivedData.ErrorMsg.IsEmpty())
                    {
                        CacheHelper.AddItem(receivedData, "" + receivedData.Id);
                    }
                    return receivedData;
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(ex => ex.GetType() == typeof(HttpRequestException)))
                {
                    throw new UnavailableConnectionException("No connection", e);
                }

                throw new RESTserviceException("AggregateException response from DB.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (JsonReaderException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (WebException)
            {
                return new PersonDetailsDto { ErrorMsg = "Database not available" };
            }
            catch (SocketException)
            {
                return new PersonDetailsDto { ErrorMsg = "Database not available" };
            }
            catch (HttpRequestException)
            {
                return new PersonDetailsDto { ErrorMsg = "Database not available" };
            } 

        }

        /// <summary>
        /// Send a Login request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        public async Task<ReplyDto> Login(UserModel user)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("http://localhost:54321/User/Login", user);
                    var attachedMessage =
                        JsonConvert.DeserializeObject<ReplyDto>(response.Content.ReadAsStringAsync().Result);
                    return attachedMessage;
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(ex => ex.GetType() == typeof(HttpRequestException)))
                {
                    throw new UnavailableConnectionException("No connection", e);
                }

                throw new RESTserviceException("AggregateException response from DB.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (WebException)
            {
                return new ReplyDto { Message = "Database not available" };
            }
            catch (SocketException)
            {
                return new ReplyDto { Message = "Database not available" };
            }
            catch (HttpRequestException)
            {
                return new ReplyDto { Message = "Database not available" };
            }
        }

        /// <summary>
        /// Send a Registration request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        public async Task<ReplyDto> Registration(UserModel user)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("http://localhost:54321/User/Registration", user);
                    var attachedMessage =
                        JsonConvert.DeserializeObject<ReplyDto>(response.Content.ReadAsStringAsync().Result);
                    return attachedMessage;
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(ex => ex.GetType() == typeof(HttpRequestException)))
                {
                    throw new UnavailableConnectionException("No connection", e);
                }

                throw new RESTserviceException("AggregateException response from DB.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (WebException)
            {
                return new ReplyDto { Message = "Database not available" };
            }
            catch (SocketException)
            {
                return new ReplyDto { Message = "Database not available" };
            }
            catch (HttpRequestException)
            {
                return new ReplyDto { Message = "Database not available" };
            } 
        }

        /// <summary>
        /// Send a rating to the server
        /// </summary>
        /// <param name="review">The review of the user</param>
        /// <returns>Reply of the server</returns>
        public async Task<ReplyDto> RateMovie(ReviewDto review)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync("http://localhost:54321/movies/review", review);
                    var attachedMessage =
                        JsonConvert.DeserializeObject<ReplyDto>(response.Content.ReadAsStringAsync().Result);
                    return attachedMessage;
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(ex => ex.GetType() == typeof(HttpRequestException)))
                {
                    throw new UnavailableConnectionException("No connection", e);
                }

                throw new RESTserviceException("AggregateException response from DB.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (WebException)
            {
                return new ReplyDto { Message = "Database not available" };
            }
            catch (SocketException)
            {
                return new ReplyDto { Message = "Database not available" };
            }
            catch (HttpRequestException)
            {
                return new ReplyDto { Message = "Database not available" };
            } 
        }

        /// <summary>
        /// Get a movie without using the cache
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        public async Task<MovieDetailsDto> GetMovieDetailsLocallyAsyncForce(int movieId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var receivedData = JsonConvert.DeserializeObject<MovieDetailsDto>(
                        await httpClient.GetStringAsync("http://localhost:54321/movies/?movieId=" + movieId)
                        );

                    if (receivedData.ErrorMsg.IsEmpty())
                    {
                        CacheHelper.RemoveItem("" + receivedData.Id);
                        CacheHelper.AddItem(receivedData, "" + receivedData.Id);
                    }

                    return receivedData;
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Any(ex => ex.GetType() == typeof(HttpRequestException)))
                {
                    throw new UnavailableConnectionException("No connection", e);
                }

                throw new RESTserviceException("AggregateException response from DB.", e);
            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (JsonReaderException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            catch (WebException)
            {
                return new MovieDetailsDto { ErrorMsg = "Database not available" };
            }
            catch (SocketException)
            {
                return new MovieDetailsDto { ErrorMsg = "Database not available" };
            }
            catch (HttpRequestException)
            {
                return new MovieDetailsDto { ErrorMsg = "Database not available" };
            } 
        }



        /////////////////////////////////////////////////////////////////////////////////////
        /// External Class
        /////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Found at http://johnnycoder.com/blog/2008/12/10/c-cache-helper-class/
        /// </summary>
        public static class CacheHelper
        {
            /// <summary>
            /// Insert value into the cache using
            /// appropriate name/value pairs
            /// </summary>
            /// <typeparam name="T">Type of cached item</typeparam>
            /// <param name="newItem">Item to be cached</param>
            /// <param name="keyOfNewItem">Name of item</param>
            public static void AddItem<T>(T newItem, string keyOfNewItem)
            {
                // After 5 minutes the object is removed from the cache 
                HttpContext.Current.Cache.Insert(
                    keyOfNewItem,
                    newItem,
                    null,
                    DateTime.Now.AddMinutes(5),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }

            /// <summary>
            /// Remove item from cache
            /// </summary>
            /// <param name="nameOfItem">Name of cached item</param>
            public static void RemoveItem(string nameOfItem)
            {
                HttpContext.Current.Cache.Remove(nameOfItem);
            }

            /// <summary>
            /// Check for item in cache
            /// </summary>
            /// <param name="nameOfItem">Name of cached item</param>
            /// <returns></returns>
            public static bool ItemExists(string nameOfItem)
            {
                return HttpContext.Current.Cache[nameOfItem] != null;
            }

            /// <summary>
            /// Retrieve cached item
            /// </summary>
            /// <typeparam name="T">Type of cached item</typeparam>
            /// <param name="nameOfItem">Name of cached item</param>
            /// <param name="desiredItem">Cached value. Default(T) if
            /// item doesn't exist.</param>
            /// <returns>Cached item as type</returns>
            public static bool GetItem<T>(string nameOfItem, out T desiredItem)
            {
                try
                {
                    if (!ItemExists(nameOfItem))
                    {
                        desiredItem = default(T);
                        return false;
                    }

                    desiredItem = (T)HttpContext.Current.Cache[nameOfItem];
                }
                catch (Exception)
                {
                    desiredItem = default(T);
                    return false;
                }
                return true;
            }
        }
    }
}