using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoSubsystem
{
    public class MovieDto : Dto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public int EpisodeNumber { get; set; }
        public string Kind { get; set; }
        public int? SeasonNumber { get; set; }
        public string SeriesYear { get; set; }
        public int? EpisodeOf_Id { get; set; }
        public string ErrorMsg { get; set; }
    }
}
