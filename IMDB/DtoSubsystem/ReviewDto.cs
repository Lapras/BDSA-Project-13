using System;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    [Serializable]
    public class ReviewDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int Rating { get; set; }
    }
}