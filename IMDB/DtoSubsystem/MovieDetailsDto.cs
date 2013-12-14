using System;
using System.Collections.Generic;

namespace DtoSubsystem
{
    [Serializable]
    public class MovieDetailsDto : Dto
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
        public double? AvgRating { get; set; }
        public string ErrorMsg { get; set; }
    }
}