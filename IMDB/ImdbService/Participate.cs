//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImdbService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Participate
    {
        public int ParticipateId { get; set; }
        public Nullable<int> NrOrder { get; set; }
        public string CharName { get; set; }
        public string Role { get; set; }
        public string Note { get; set; }
        public Nullable<int> Movie_Id { get; set; }
        public Nullable<int> Person_Id { get; set; }
    
        public virtual Movies Movies { get; set; }
        public virtual People People { get; set; }
    }
}
