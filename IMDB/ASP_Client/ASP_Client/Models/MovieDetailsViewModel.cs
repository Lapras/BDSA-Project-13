using System.Collections.Generic;

namespace ASP_Client.Models
{
    public class MovieDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }

        public List<People> Participants { get; set; }
    }
}