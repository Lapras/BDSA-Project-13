﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImdbRestService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ImdbEntities : DbContext, IImdbEntities
    {
        public ImdbEntities()
            : base("name=ImdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public IDbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public IDbSet<InfoType> InfoTypes { get; set; }
        public IDbSet<MovieInfo> MovieInfoes { get; set; }
        public IDbSet<Movie> Movies { get; set; }
        public IDbSet<Participate> Participates { get; set; }
        public IDbSet<Person> People { get; set; }
        public IDbSet<PersonInfo> PersonInfoes { get; set; }
        public IDbSet<User> User { get; set; }
    }
}
