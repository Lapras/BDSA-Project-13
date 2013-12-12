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
        public int UserRating { get; set; }
        public List<PersonViewModel> Participants { get; set; }
        public string ErrorMsg { get; set; }
    }
}