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
    }
}