using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Client.Dtos
{
    public class MovieSearchDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }   
    }
}
