using System.Collections.Generic;
using DtoSubsystem;


namespace ASP_Client.Models
{
    public class PersonOverviewViewModel
    {
        public List<PersonDto> FoundPeople;
        public string ErrorMsg { get; set; }

        public PersonOverviewViewModel()
        {
            FoundPeople = new List<PersonDto>();
        }
    }
}