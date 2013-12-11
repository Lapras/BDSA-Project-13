using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ASP_Client.Models;
using DtoSubsystem;
using Newtonsoft.Json;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// Reponsible to Communicate with the application server holding the data
    /// </summary>
    public static class CommunicationFacade
    {
        /// <summary>
        /// Get a list of movies maching the search string
        /// </summary>
        /// <param name="searchString">Movie to look for</param>
        /// <returns>List of movies matching the search string</returns>
        public static async Task<List<MovieDto>> GetMoviesAsync(string searchString)
        {
            List<MovieDto> desiredMovies;
            CacheHelper.GetItem(searchString, out desiredMovies);

            if (desiredMovies != null) return desiredMovies;

            using (var httpClient = new HttpClient())
            {
                var receivedData = JsonConvert.DeserializeObject<List<MovieDto>>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?title=" + searchString)
                    );

                foreach (var movie in receivedData)
                {
                    CacheHelper.AddItem(movie, movie.Title);
                }
                return receivedData;
            }
        }

        /// <summary>
        /// Get detailed information about a specific movie
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        public static async Task<MovieDetailsDto> GetMovieDetailsLocallyAsync(int movieId)
        {
            MovieDetailsDto desiredMovie;
            CacheHelper.GetItem("" + movieId, out desiredMovie);

            if (desiredMovie != null) return desiredMovie;

            using (var httpClient = new HttpClient())
            {
                var receivedData = JsonConvert.DeserializeObject<MovieDetailsDto>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?movieId=" + movieId)
                    );


                CacheHelper.AddItem(receivedData, "" + receivedData.Id);


                return receivedData;
            }
        }

        /// <summary>
        /// Get the details of a specific person involved in the movie business
        /// </summary>
        /// <param name="personId">Id of the person to get the data of</param>
        /// <returns>Detailed data of the person</returns>
        public static async Task<PersonDetailsDto> GetPersonDetailsLocallyAsync(int personId)
        {
            PersonDetailsDto desiredPerson;
            CacheHelper.GetItem("" + personId, out desiredPerson);

            if (desiredPerson != null) return desiredPerson;

            using (var httpClient = new HttpClient())
            {
                var receivedData = JsonConvert.DeserializeObject<PersonDetailsDto>(
                    await httpClient.GetStringAsync("http://localhost:54321/person/?personId=" + personId)
                    );

                CacheHelper.AddItem(receivedData, "" + receivedData.Id);

                return receivedData;
            }
        }

        /// <summary>
        /// Send a Login request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        public static async Task<ReplyDto> Login(UserModel user)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync("http://localhost:54321/User/Login", user);
                var attachedMessage =
                    JsonConvert.DeserializeObject<ReplyDto>(response.Content.ReadAsStringAsync().Result);
                return attachedMessage;
            }
        }

        /// <summary>
        /// Send a Registration request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        public static async Task<ReplyDto> Registration(UserModel user)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync("http://localhost:54321/User/Registration", user);
                var attachedMessage =
                    JsonConvert.DeserializeObject<ReplyDto>(response.Content.ReadAsStringAsync().Result);
                return attachedMessage;
            }
        }

        /// <summary>
        /// Send a rating to the server
        /// </summary>
        /// <param name="review">The review of the user</param>
        /// <returns>Reply of the server</returns>
        public static async Task<ReplyDto> RateMovie(ReviewDto review)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync("http://localhost:54321/movies/review", review);
                var attachedMessage =
                    JsonConvert.DeserializeObject<ReplyDto>(response.Content.ReadAsStringAsync().Result);
                return attachedMessage;
            }
        }

        /// <summary>
        /// Get a movie without using the cache
        /// </summary>
        /// <param name="movieId">Id of the movie to get the data of</param>
        /// <returns>Detailed data of the movie</returns>
        public static async Task<MovieDetailsDto> GetMovieDetailsLocallyAsyncForce(int movieId)
        {
            using (var httpClient = new HttpClient())
            {
                var receivedData = JsonConvert.DeserializeObject<MovieDetailsDto>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?movieId=" + movieId)
                    );

                CacheHelper.RemoveItem(""+receivedData.Id);
                CacheHelper.AddItem(receivedData, "" + receivedData.Id);


                return receivedData;
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

                    desiredItem = (T) HttpContext.Current.Cache[nameOfItem];
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