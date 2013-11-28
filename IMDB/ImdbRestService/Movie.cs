//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImdbRestService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Movie
    {
        public Movie()
        {
            this.MovieInfoes = new HashSet<MovieInfo>();
            this.Movies1 = new HashSet<Movie>();
            this.Participates = new HashSet<Participate>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Kind { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> SeasonNumber { get; set; }
        public Nullable<int> EpisodeNumber { get; set; }
        public string SeriesYear { get; set; }
        public Nullable<int> EpisodeOf_Id { get; set; }
    
        public virtual ICollection<MovieInfo> MovieInfoes { get; set; }
        public virtual ICollection<Movie> Movies1 { get; set; }
        public virtual Movie Movie1 { get; set; }
        public virtual ICollection<Participate> Participates { get; set; }
    }
}