using System.Collections.Generic;

namespace ASP_Client.Models
{
    public class MovieOverviewViewModel
    {
        public List<MovieViewModel> FoundMovies;

        public MovieOverviewViewModel()
        {
            FoundMovies = new List<MovieViewModel>();
        }
    }
}