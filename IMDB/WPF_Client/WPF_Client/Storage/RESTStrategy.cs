using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using DtoSubsystem;
using WPF_Client.Exceptions;
using Newtonsoft.Json;

namespace WPF_Client.Storage
{
    /// <summary>
    /// A storage strategy which relies on using RESTful methods on an external server.
    /// </summary>
    public class RESTStrategy : IStorageStrategy
    {

        /// <summary>
        /// The url that this RESTstrategy communicates with.
        /// </summary>
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
            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.Timeout = new TimeSpan(0, 0, 0, 10);

                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.GetStringAsync(_url + "/movies/?title=" + searchString);


                    Console.WriteLine("JSON string received:" + response.Result);
                    Console.WriteLine("Starting deserializing");
                    var result = JsonConvert.DeserializeObject<ObservableCollection<MovieDto>>(response.Result);
                    Console.WriteLine("deserializing done");


                    if (result != null)
                    {
                        if (result[0].ErrorMsg != null)
                        {
                            if (result[0].ErrorMsg.Equals("Movie could not be found"))
                            {
                                result.Remove(result[0]);
                            }
                        }

                        
                    }

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
            catch (JsonReaderException e)
            {
                throw new RESTserviceException("There was an error reading the json", e);
            }

        }


        /// <summary>
        /// Retrives the PersonDtos.
        /// </summary>
        /// <param name="searchString">The input search string.</param>
        /// <returns>A collection of PersonDtos.</returns>
        public ObservableCollection<PersonDto> PersonDtos(string searchString)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.Timeout = new TimeSpan(0, 0, 0, 10);

                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.GetStringAsync(_url + "/person/?person=" + searchString);


                    Console.WriteLine("JSON string received:" + response.Result);
                    Console.WriteLine("Starting deserializing");
                    var result = JsonConvert.DeserializeObject<ObservableCollection<PersonDto>>(response.Result);
                    Console.WriteLine("deserializing done");

                    if (result != null)
                    {
                        if (result[0].ErrorMsg != null)
                        {
                            if (result[0].ErrorMsg.Equals("Movie could not be found"))
                            {
                                result.Remove(result[0]);
                            }
                        }

                    }
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
                throw new RESTserviceException("There was an serializaton or deserializaton error", e);
            }
            catch (JsonReaderException e)
            {
                throw new RESTserviceException("There was an error reading the json", e);
            }

        }

        /// <summary>
        /// Retrives the MovieDetailsDto.
        /// </summary>
        /// <param name="movieId">The input movieId</param>
        /// <returns>A MovieDetailsDto.</returns>
        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 10);

                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.GetStringAsync(_url + "/movies/" + movieId);


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
                throw new RESTserviceException("There was an error reading the json", e);
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
                var user = new RegistrationDto()
                {
                    Name = name,
                    Password = password
                };


                using (var httpClient = new HttpClient())
                {

                    httpClient.Timeout = new TimeSpan(0, 0, 0, 10);

                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.PostAsJsonAsync(_url + "/user/registration", user).Result;
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
            catch (JsonReaderException e)
            {
                throw new RESTserviceException("There was an error reading the json", e);
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
                var user = new LoginDto()
                {
                    Name = name,
                    Password = password
                };
                
                // Console.WriteLine(name + " " + password);

                using (var httpClient = new HttpClient())
                {

                    httpClient.Timeout = new TimeSpan(0, 0, 0, 10);


                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.PostAsJsonAsync(_url + "/user/login", user).Result;
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
            catch (JsonReaderException e)
            {
                throw new RESTserviceException("There was an error reading the json", e);
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

                    httpClient.Timeout = new TimeSpan(0, 0, 0, 10);

                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.GetStringAsync(_url + "/person/" + id);


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
                throw new RESTserviceException("There was an error reading the json", e);
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
                var user = new RatingDto()
                {
                    MovieId = movieId,
                    Rating = rating,
                    Username = username
                };

                // Console.WriteLine(name + " " + password);

                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 10);

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
            catch (JsonReaderException e)
            {
                throw new RESTserviceException("There was an error reading the json", e);
            }
        }
    }

    
    
}
