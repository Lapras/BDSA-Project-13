using System;

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