using System;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    [Serializable]
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CharacterName { get; set; }
    }
}