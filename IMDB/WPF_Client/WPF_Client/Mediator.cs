using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client
{
    static public class Mediator
    {
        public static string SearchString { get; set; } // The string that is used for search.
        public static int SearchType { get; set; } // Int that indicates the type of search.

        public static int _movieId; //The movieId that this ViewModel loads for the MovieDto.
    }
}
