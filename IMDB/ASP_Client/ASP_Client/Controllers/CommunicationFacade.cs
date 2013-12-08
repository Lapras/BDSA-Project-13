using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ASP_Client.Models;
using DtoSubsystem;
using Newtonsoft.Json;
using RestSharp;

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
            List<MovieDto> data;            
            CacheHelper.Get(searchString, out data);

            if (data != null) return data;

            using (var httpClient = new HttpClient())
            {
                var receivedData = JsonConvert.DeserializeObject<List<MovieDto>>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?title=" + searchString)
                    );

                foreach (var movieDto in receivedData)
                {
                    CacheHelper.Add(movieDto, movieDto.Title);
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
            MovieDetailsDto data;
            CacheHelper.Get(""+movieId, out data);

            if (data != null) return data;

            using (var httpClient = new HttpClient())
            {
                var receivedData = JsonConvert.DeserializeObject<MovieDetailsDto>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?movieId=" + movieId)
                    );

           
                    CacheHelper.Add(receivedData, ""+receivedData.Id);
                

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
            PersonDetailsDto data;
            CacheHelper.Get("" + personId, out data);

            if (data != null) return data;

            using (var httpClient = new HttpClient())
            {
                var receivedData = JsonConvert.DeserializeObject<PersonDetailsDto>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?personId=" + personId)
                    );

                CacheHelper.Add(receivedData, "" + receivedData.Id);

                return receivedData;
            }
        }

        /// <summary>
        /// Send a Login request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        public static async Task<HttpResponseMessage> Login(UserModel user)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.PostAsJsonAsync("http://localhost:54321/User/Login", user);
            }
        }

        /// <summary>
        /// Send a Registration request to the application server
        /// </summary>
        /// <param name="user">Data of the user to log in</param>
        /// <returns>Response from the server</returns>
        public static async Task<HttpResponseMessage> Registration(UserModel user)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.PostAsJsonAsync("http://localhost:54321/User/Registration", user);
            }
        }
    }

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
        /// <param name="o">Item to be cached</param>
        /// <param name="key">Name of item</param>
        public static void Add<T>(T o, string key)
        {
            // After 5 minutes the object is removed from the cache again
            HttpContext.Current.Cache.Insert(
                key,
                o,
                null,
                DateTime.Now.AddMinutes(5),
                System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <param name="value">Cached value. Default(T) if
        /// item doesn't exist.</param>
        /// <returns>Cached item as type</returns>
        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T) HttpContext.Current.Cache[key];
            }
            catch(Exception e)
            {
                value = default(T);
                return false;
            }

            return true;
        }
    }
}