using System.Collections.Generic;
using DtoSubsystem;


namespace ASP_Client.Models
{
    /// <summary>
    /// Class representing a movie in a simple way
    /// </summary>
    public class MovieOverviewViewModel
    {
        public List<MovieDto> FoundMovies;
        public string ErrorMsg { get; set; }

        public MovieOverviewViewModel()
        {
            FoundMovies = new List<MovieDto>();

        }
    }
}