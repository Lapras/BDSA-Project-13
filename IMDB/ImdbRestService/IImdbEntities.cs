using System;
using System.Data.Entity;

namespace ImdbRestService
{
    public interface IImdbEntities : IDisposable
    {
        IDbSet<Movie> Movies { get; }
    }
}