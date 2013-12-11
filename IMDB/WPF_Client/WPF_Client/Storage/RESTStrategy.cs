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
                    var response = httpClient.GetStringAsync("http://localhost:54321/movies/?title=" + searchString);


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
                        throw new UnavailableConnection("No connection", e);
                    }
                }
                
                throw new RESTserviceException("AggregateException response from DB.", e);

            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("There was an serializaton or deserializaton error", e);
            }

        }


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
                    var response = httpClient.GetStringAsync("http://localhost:54321/movies/?movieId=" + movieId);


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
                        throw new UnavailableConnection("No connection", e);
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

        public bool CreateProfile(string name, string password)
        {
            //example for now:
            //return true;
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
                    var response = httpClient.PostAsJsonAsync("http://localhost:54321/User/Registration", user).Result;
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
                        throw new UnavailableConnection("No connection", e);
                    }
                }

                throw new RESTserviceException("AggregateException response from DB.", e);

            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }



        }

        public bool LoginInfo(string name, string password)
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
                    var response = httpClient.PostAsJsonAsync("http://localhost:54321/User/Login", user).Result;
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
                        throw new UnavailableConnection("No connection", e);
                    }
                }

                throw new RESTserviceException("AggregateException response from DB.", e);

            }
            catch (JsonSerializationException e)
            {
                throw new RESTserviceException("there was an serializaton or deserializaton error", e);
            }
            
           

        }
        public PersonDetailsDto PersonDetailsDto(int id)
        {
            
            try
            {
                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.GetStringAsync("http://localhost:54321/person/?personId=" + id);


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
                        throw new UnavailableConnection("No connection", e);
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

        public bool RateMovie(int id, int rating, string username)
        {
            try
            {
                var user = new ReviewDto()
                {
                    MovieId = id,
                    Rating = rating,
                    Username = username
                };

                // Console.WriteLine(name + " " + password);

                using (var httpClient = new HttpClient())
                {
                    Console.WriteLine("Getting reponse from REST server");
                    var response = httpClient.PostAsJsonAsync("http://localhost:54321/movies/review", user).Result;
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
                        throw new UnavailableConnection("No connection", e);
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
