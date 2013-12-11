using System;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    [Serializable]
    public class InfoDto
    {
        public string Name { get; set; }
        public string Info { get; set; }
        
    }
}