using System;
using System.Collections.Generic;

namespace DtoSubsystem
{
    [Serializable]
    public class PersonDetailsDto : Dto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public string CharName { get; set; }
        public List<InfoDto> Info { get; set; }    
        public String ErrorMsg { get; set; }
    }
}