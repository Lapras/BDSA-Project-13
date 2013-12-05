using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbRestService
{
    public partial class ImdbEntities : IImdbEntities
    {
    }

    public interface IImdbEntities : IDisposable
    {
        DbSet<Movie> Movies { get; set; }
    }
}
