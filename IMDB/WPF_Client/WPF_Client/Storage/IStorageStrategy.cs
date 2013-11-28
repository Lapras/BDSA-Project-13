using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;

namespace WPF_Client.Storage
{
    public interface IStorageStrategy
    {
        ObservableCollection<MovieSearchDto> MovieSearchDtos(string searchString);
        MovieDto MovieDto(int movieId);
    }
}
