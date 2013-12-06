using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    [Serializable]
    public class UserModelDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}