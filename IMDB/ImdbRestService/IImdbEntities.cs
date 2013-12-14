using System;
using System.Data.Entity;

namespace ImdbRestService
{
    /// <summary>
    /// Interface for every entity class in our database
    /// </summary>
    public interface IImdbEntities : IDisposable
    {
        IDbSet<Movie> Movies { get; }
        IDbSet<User> User { get; }
        IDbSet<Rating> Rating { get; }
        IDbSet<Person> People { get; }
        IDbSet<Participate> Participates { get; }
        IDbSet<PersonInfo> PersonInfoes { get; } 
        IDbSet<InfoType> InfoTypes { get; } 
        int SaveChanges();
    }
}