//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IMDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class PersonInfo
    {
        public int PersonInfoId { get; set; }
        public string Info { get; set; }
        public string Note { get; set; }
        public int Person_Id { get; set; }
        public int Type_Id { get; set; }
    
        public virtual InfoType InfoType { get; set; }
        public virtual People People { get; set; }
    }
}
