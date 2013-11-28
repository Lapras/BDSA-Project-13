using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Dtos
{

    /// <summary>
    /// Dto used when receiving movie information on a single movie.
    /// </summary>
    public class MovieDto
    {
        //for now...
        public string Title { get; set; }
        public string Kind { get; set; }
        public Nullable<int> Year { get; set; }
    }
}
