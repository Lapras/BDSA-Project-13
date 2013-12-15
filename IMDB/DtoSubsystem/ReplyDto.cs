using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    /// <summary>
    /// Dto used for reply messages by the server 
    /// </summary>
    [Serializable]
    public class ReplyDto : Dto
    {
        public string Message { get; set; }
        public bool Executed { get; set; }
    }
}