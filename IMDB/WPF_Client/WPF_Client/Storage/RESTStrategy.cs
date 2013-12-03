using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;
using WPF_Client.Dtos;
using WPF_Client.Controller;
using Newtonsoft.Json;



namespace WPF_Client.Storage
{
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


        public MovieProfileDto MovieProfileDto(int movieId)
        {
            //GET rest etc. etc.
            //return...


            MovieProfileDto result;

            //just example for now....
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


        }

        public bool CreateProfile(string name, string password)
        {
            //example for now:


            // POST /createprofile/
            //etc. etc.

            return true;

        }
      


        }


    
}
