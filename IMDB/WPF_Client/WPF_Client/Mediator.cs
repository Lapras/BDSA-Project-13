using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;

namespace WPF_Client
{
    static public class Mediator
    {

        /* not sure if we should use this class anymore. was used to pass messages across viewmodel.
         * now we have controllers that does this job. and they also do much of the logic instead of the view models now.
         * 
        public static string SearchString { get; set; } // The string that is used for search.
        public static int SearchType { get; set; } // Int that indicates the type of search.

        public static int MovieId { get; set; } // The movieId that the SearchResultViewModel loads for the MovieDto for.

        public static ObservableCollection<MovieSearchDto> test { get; set; }
            
         */   
         //   = _model.MovieSearchDtos(Mediator.SearchString);
    }
}
