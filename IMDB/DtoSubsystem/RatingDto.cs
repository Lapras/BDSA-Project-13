using System;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    [Serializable]
    public class RatingDto : Dto
    {
        public string Username { get; set; }
        public int MovieId { get; set; }
        public int Rating { get; set; }
    }
}