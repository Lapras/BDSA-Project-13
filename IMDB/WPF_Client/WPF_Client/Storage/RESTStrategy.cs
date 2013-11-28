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

            return new ObservableCollection<MovieSearchDto>() { 
                new MovieSearchDto() {Id = 0, Title = searchString, Year = 2013},
                new MovieSearchDto() {Id = 1, Title = "Gravity", Year = 2013} 
            };

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
                    result = new MovieDto {Title = Mediator.SearchString, Year = 2013, Kind = "Sci-fi"};
                    return result;

                case 1:
                    result = new MovieDto { Title = "Gravity", Year = 2013, Kind = "Sci-fi" };
                    return result;

                default:
                    result = new MovieDto { Title = "Should not see this title", Year = 2013, Kind = "Sci-fi" };
                    return result;


                

            }


        }

    }
}
