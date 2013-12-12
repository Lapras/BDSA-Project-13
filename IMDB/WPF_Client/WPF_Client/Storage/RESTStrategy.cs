using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DtoSubsystem;
using WPF_Client.Controller;
using WPF_Client.Exceptions;
using Newtonsoft.Json;
using WPF_Client.PwBoxAssistant;


namespace WPF_Client.Storage
{
    /// <summary>
    /// A storage strategy which relies on using RESTful methods on an external server.
    /// </summary>
    public class RESTStrategy : IStorageStrategy
    {
        private string _url;

        /// <summary>
        /// Contructor that takes the url of the REST service an input.
        /// </summary>
        /// <param name="url">The input REST service url.</param>
        public RESTStrategy(string url)
        {
            _url = url;
        }


        /// <summary>
        /// Retrives the MovieDtos.
        /// </summary>
        /// <param name="searchString">The input search string.</param>
        /// <returns>A collection of MovieDtos.</returns>
        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {
     
            //sample example data don't delete
            /*
            var collection = new ObservableCollection<MovieDto>() { 
                new MovieDto() {Id = 0, Title = "predator", Year = 1987},
                new MovieDto() {Id = 1, Title = "man of steel", Year = 2013},
                new MovieDto() {Id = 2, Title = "spiderman", Year = 2002},
            };

            var result = new ObservableCollection<MovieDto>(collection.Where(m => m.Title == searchString.ToLower()).ToList());
            
            return result;
            */


            try
            {
                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.GetStringAsync(_url + "/movies/?title=" + searchString);


                    Console.WriteLine("JSON string received:" + response.Result);
                    Console.WriteLine("Starting deserializing");
                    var result = JsonConvert.DeserializeObject<ObservableCollection<MovieDto>>(response.Result);
                    Console.WriteLine("deserializing done");

                    return result;
                }

            }
            catch (AggregateException e)
            {

                foreach (Exception ex in e.InnerExceptions)
                {
                    if (ex.GetType() == typeof (HttpRequestException))
                    {
                        throw new UnavailableConnectionException("No connection", e);
                    }
                }
                
                throw new RESTserviceException("AggregateException response from DB.", e);

            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("There was an serializaton or deserializaton error", e);
            }

        }

        /// <summary>
        /// Retrives the MovieDetailsDto.
        /// </summary>
        /// <param name="movieId">The input movieId</param>
        /// <returns>A MovieDetailsDto.</returns>
        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            //GET rest etc. etc.
            //return...


            

            //example data don't delete:


            /*
            MovieProfileDto result;
            switch (movieId)
            {
                case 0:
                    result = new MovieProfileDto { Title = "Predator", Year = 1987, Kind = "Sci-fi" };
                    return result;

                case 1:
                    result = new MovieProfileDto { Title = "Man of Steel", Year = 2013, Kind = "Action" };
                    return result;

                case 2:
                    result = new MovieProfileDto { Title = "Spiderman", Year = 2002, Kind = "Action" };
                    return result;

                default:
                    result = new MovieProfileDto { Title = "error", Year = 2013, Kind = "error" };
                    return result;


            }
            */

            try
            {
                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.GetStringAsync(_url + "/movies/?movieId=" + movieId);


                    Console.WriteLine("JSON string received:" + response.Result);
                    Console.WriteLine("Starting deserializing");
                    var result = JsonConvert.DeserializeObject<MovieDetailsDto>(response.Result);
                    Console.WriteLine("deserializing done");



                    return result;
                }
            }
            catch (AggregateException e)
            {
                foreach (Exception ex in e.InnerExceptions)
                {
                    if (ex.GetType() == typeof (HttpRequestException))
                    {
                        throw new UnavailableConnectionException("No connection", e);
                    }
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



        }


        /// <summary>
        /// Sends a userprofile requests.
        /// </summary>
        /// <param name="name">The requested username</param>
        /// <param name="password">The requested password.</param>
        /// <returns>A boolean value whether the user was created or not.</returns>
        public bool CreateProfile(string name, string password)
        {
            try
            {
                var user = new UserModelDto()
                {
                    Name = name,
                    Password = password
                };


                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.PostAsJsonAsync(_url + "/User/Registration", user).Result;
                    var msg = response.Content.ReadAsStringAsync();

                    Console.WriteLine("JSON string received:" + response);
                    Console.WriteLine("Starting deserializing");
                    var result = JsonConvert.DeserializeObject<ReplyDto>(msg.Result);
                    Console.WriteLine("deserializing done");

                    return result.Executed;
                }
            }
            catch (AggregateException e)
            {
                foreach (Exception ex in e.InnerExceptions)
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        throw new UnavailableConnectionException("No connection", e);
                    }
                }

                throw new RESTserviceException("AggregateException response from DB.", e);

            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
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
                var user = new UserModelDto()
                {
                    Name = name,
                    Password = password
                };
                
                // Console.WriteLine(name + " " + password);

                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.PostAsJsonAsync(_url + "/User/Login", user).Result;
                    var msg = response.Content.ReadAsStringAsync();

                    Console.WriteLine("JSON string received:" + response);
                    Console.WriteLine("Starting deserializing");
                    var result = JsonConvert.DeserializeObject<ReplyDto>(msg.Result);
                    Console.WriteLine("deserializing done");

                    return result.Executed;
                }
                
            }
                
            catch (AggregateException e)
            {
                foreach (Exception ex in e.InnerExceptions)
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        throw new UnavailableConnectionException("No connection", e);
                    }
                }

                throw new RESTserviceException("AggregateException response from DB.", e);

            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            
           

        }


        /// <summary>
        /// Retrieves a PersonDetailsDto with the supplied personId.
        /// </summary>
        /// <param name="id">The input personId.</param>
        /// <returns>The PersonDetailsDto</returns>
        public PersonDetailsDto PersonDetailsDto(int id)
        {
            
            try
            {
                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.GetStringAsync(_url + "/person/?personId=" + id);


                    Console.WriteLine("JSON string received:" + response.Result);
                    Console.WriteLine("Starting deserializing");
                    var result = JsonConvert.DeserializeObject<PersonDetailsDto>(response.Result);
                    Console.WriteLine("deserializing done");



                    return result;
                }
            }
            
            catch (AggregateException e)
            {
                foreach (Exception ex in e.InnerExceptions)
                {
                    
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        throw new UnavailableConnectionException("No connection", e);
                    }
                    
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
            Console.WriteLine("Trying to review");

            try
            {
                var user = new ReviewDto()
                {
                    MovieId = movieId,
                    Rating = rating,
                    Username = username
                };

                // Console.WriteLine(name + " " + password);

                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.PostAsJsonAsync(_url +"/movies/review", user).Result;
                    var msg = response.Content.ReadAsStringAsync();

                    Console.WriteLine("JSON string received:" + response);
                    Console.WriteLine("Starting deserializing");
                    var result = JsonConvert.DeserializeObject<ReplyDto>(msg.Result);
                    Console.WriteLine("deserializing done");

                    return result.Executed;
                }

            }

            catch (AggregateException e)
            {
                foreach (Exception ex in e.InnerExceptions)
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        throw new UnavailableConnectionException("No connection", e);
                    }
                }

                throw new RESTserviceException("AggregateException response from DB.", e);

            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
        }
    }

    
    
}
