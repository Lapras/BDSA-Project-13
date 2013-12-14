using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    [Serializable]
    public class RegistrationDto : Dto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}