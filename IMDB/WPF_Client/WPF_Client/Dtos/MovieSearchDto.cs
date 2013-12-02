using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Dtos
{

    /// <summary>
    /// Dto used when receiving movie search results.
    /// </summary>
    public class MovieSearchDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
    }
}
