﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Dtos;
using DtoSubsystem;

namespace WPF_Client.Storage
{
    public interface IStorageStrategy
    {
        ObservableCollection<MovieDto> MovieDtos(string searchString);
        //void MovieSearchDtos(string searchString);
        MovieDetailsDto MovieDetailsDto(int movieId);
        bool CreateProfile(string name, string password);
        bool LoginInfo(string name, string password);
    }
}
