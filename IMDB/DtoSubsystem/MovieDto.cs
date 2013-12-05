using System;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    [Serializable]
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
	    public int EpisodeNumber { get; set; }
		public string Kind { get; set; }
		public int? SeasonNumber { get; set; }
		public string SeriesYear { get; set; }
		public int? EpisodeOf_Id { get; set; }
    }
}