using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;

namespace WPF_Client.Storage
{
    class RESTStrategy : IStorageStrategy
    {
        public ObservableCollection<MovieSearchDto> MovieSearchDtos(string searchString)
        {
            //GET rest etc. etc.
            //return ...

            


            //example for now...
            var collection = new ObservableCollection<MovieSearchDto>() { 
                new MovieSearchDto() {Id = 0, Title = "predator", Year = 1987},
                new MovieSearchDto() {Id = 1, Title = "man of steel", Year = 2013},
                new MovieSearchDto() {Id = 2, Title = "spiderman", Year = 2002},
                new MovieSearchDto() {Id = 3, Title = searchString, Year = 2013} 
            };


            var result = new ObservableCollection<MovieSearchDto>(collection.Where(m => m.Title == searchString.ToLower()).ToList());
            return result;

        }


        public MovieDto MovieDto(int movieId)
        {
            //GET rest etc. etc.
            //return...



            MovieDto result;

            //just example for now....
            switch (movieId)
            {
                case 0:
                    result = new MovieDto { Title = "Predator", Year = 1987, Kind = "Sci-fi" };
                    return result;

                case 1:
                    result = new MovieDto { Title = "Man of Steel", Year = 2013, Kind = "Action" };
                    return result;

                case 2:
                    result = new MovieDto { Title = "Spiderman", Year = 2002, Kind = "Action" };
                    return result;

                case 3:
                    result = new MovieDto { Title = Mediator.SearchString, Year = 2013, Kind = "Sci-fi" };
                    return result;

                default:
                    result = new MovieDto { Title = "error", Year = 2013, Kind = "Sci-fi" };
                    return result;


                

            }


        }

    }
}
