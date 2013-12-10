using System.Collections.Generic;
using System.Web.Mvc;

namespace ASP_Client.Models
{
    public class MovieDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public int Rating { get; set; }
        public List<ActorViewModel> Participants { get; set; }
    }
}