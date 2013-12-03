using System;
using System.Collections.Generic;
using ASP_Client.Models;

namespace DtoSubsystem
{
    [Serializable]
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; } 
        public int? Year { get; set; }
        public List<PersonDto> Participants { get; set; }
        public int? SeasonNumber { get; set; }
        public int? EpisodeNumber { get; set; }
        public string SeriesYear { get; set; }
        public int? EpisodeOf_Id { get; set; }

    }
}