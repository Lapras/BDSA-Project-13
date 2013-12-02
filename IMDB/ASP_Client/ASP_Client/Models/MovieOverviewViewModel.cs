using System.Collections.Generic;
using DtoSubsystem;


namespace ASP_Client.Models
{
    public class MovieOverviewViewModel
    {
        public List<MovieDto> FoundMovies;

        public MovieOverviewViewModel()
        {
            FoundMovies = new List<MovieDto>();
        }
    }
}