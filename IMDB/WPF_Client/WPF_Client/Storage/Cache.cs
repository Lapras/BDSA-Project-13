using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;

namespace WPF_Client.Storage
{
    /// <summary>
    /// A in-memory cache that is always searched first before trying to use the main storage.
    /// </summary>
    class Cache
    {
        ObservableCollection<MovieDto> _movieDtos;
        IDictionary<int,MovieDetailsDto> _movieDetailsDtos;

        IDictionary<string, bool> _hasBeenSearchedBefore;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Cache()
        {
            _movieDtos = new ObservableCollection<MovieDto>();
            _movieDetailsDtos = new Dictionary<int,MovieDetailsDto>();
            _hasBeenSearchedBefore = new Dictionary<string, bool>();
        }



        /// <summary>
        /// Retrieves the MovieDtos (dtos that a movie search returns)
        /// </summary>
        public ObservableCollection<MovieDto> MovieDtos(string searchString)
        {

            bool hasBeenSearchedBefore;
            _hasBeenSearchedBefore.TryGetValue(searchString, out hasBeenSearchedBefore);

            if (!hasBeenSearchedBefore)
            {
                _hasBeenSearchedBefore.Add(searchString, true);
                
                return null;
            }
            else
            {                
                ObservableCollection<MovieDto> result;
                
                var resultList = (from m in _movieDtos
                                  where m.Title.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0
                                  select new MovieDto
                                  {
                                      Id = m.Id,
                                      Title = m.Title,
                                      Year = m.Year
                                  }).ToList();


                result = new ObservableCollection<MovieDto>(resultList);

                Console.WriteLine(result.Count);

                return result;
            }
            
        }

        /// <summary>
        /// Retrieves a MovieDetailDto (dto used when viewing information on a movie)
        /// </summary>
        public MovieDetailsDto MovieDetailsDto(int movieId)
        {
            MovieDetailsDto result = new MovieDetailsDto();

            if (_movieDetailsDtos.ContainsKey(movieId))
	        {
                result = _movieDetailsDtos[movieId];
	        }


            if (!_movieDetailsDtos.ContainsKey(movieId))
	        {
                result = null;
	        }

            return result;
        }



        /// <summary>
        /// Adds the supplied MovieDtos list to the in-memory cache.
        /// </summary>
        public void AddMovieDtos(IList<MovieDto> movieDtoList)
        {
            foreach (MovieDto m in movieDtoList)
            {
                _movieDtos.Add(m);
                Console.WriteLine(m.Title + " added...");
            }
        }

        /// <summary>
        /// Adds the supplied MovieDetailDto to the in-memory cache.
        /// </summary>
        public void AddMovieDetailsDto(MovieDetailsDto dto)
        {
            _movieDetailsDtos.Add(dto.Id, dto);
        }

        /// <summary>
        /// Removes the supplied MovieDetailDto to the in-memory cache.
        /// </summary>
        public void RemoveMovieDetailsDto(MovieDetailsDto dto)
        {
            _movieDetailsDtos.Remove(dto.Id);
        }
    }
}
