using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;

namespace WPF_Client.Storage
{
    class Cache
    {
        ObservableCollection<MovieDto> _movieDtos;



        public Cache()
        {
            _movieDtos = new ObservableCollection<MovieDto>();
        }




        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {
            ObservableCollection<MovieDto> result;


            var resultList = (from m in _movieDtos
                             where m.Title.Contains(searchString)
                             select new MovieDto
                             {
                                 Id = m.Id,
                                 Title = m.Title,
                                 Year = m.Year
                             }).ToList();


            result = new ObservableCollection<MovieDto>(resultList);

            return result;
        }


        //public MovieDetailsDto MovieDetailsDto(int movieId)
        //{
        //    return _strategy.MovieDetailsDto(movieId);
        //}
    }
}
