using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;

namespace WPF_Client.Storage
{
    /// <summary>
    /// Interface that the strategies of the storage must implement.
    /// </summary>
    public interface IStorageStrategy
    {
        ObservableCollection<MovieDto> MovieDtos(string searchString);
        MovieDetailsDto MovieDetailsDto(int movieId);
        bool CreateProfile(string name, string password);
        bool Login(string name, string password);
        PersonDetailsDto PersonDetailsDto(int id);
        bool RateMovie(int id, int rating, string username);
    }
}
