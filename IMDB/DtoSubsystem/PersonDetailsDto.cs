using System;
using System.Collections.Generic;
using ASP_Client.Models;

namespace DtoSubsystem
{
    [Serializable]
    public class PersonDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
    }
}