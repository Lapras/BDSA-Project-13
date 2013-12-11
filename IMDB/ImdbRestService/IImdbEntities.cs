using System;
using System.Data.Entity;

namespace ImdbRestService
{
    public interface IImdbEntities : IDisposable
    {
        IDbSet<Movie> Movies { get; }
        IDbSet<User> User { get; }
        IDbSet<Person> People { get; }
        IDbSet<Participate> Participates { get; }
        IDbSet<PersonInfo> PersonInfoes { get; } 
        IDbSet<InfoType> InfoTypes { get; } 
        int SaveChanges();
    }
}