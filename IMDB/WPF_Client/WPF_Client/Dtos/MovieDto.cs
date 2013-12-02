using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WPF_Client.Dtos
{

    //should not be used should use shared dtosubsystem

    /// <summary>
    /// Dto used when receiving movie search results.
    /// </summary>
    [Serializable]
    public class MovieDto2
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
    }
}
