﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace DtoSubsystem
{
    [Serializable]
    public class ReplyDto : Dto
    {
        public string Message { get; set; }
        public bool Executed { get; set; }
    }
}