using System.Collections.Generic;

namespace ASP_Client.Models
{
    public class PersonDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public string CharName { get; set; }
        public List<InfoModel> Info { get; set; }
        public string ErrorMsg { get; set; }
    }
}