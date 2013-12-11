﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;

namespace WPF_Client.Model
{
    public interface IModel
    {
        ObservableCollection<MovieDto> MovieDtos(string searchString);
        //void MovieSearchDtos(string searchString);
        MovieDetailsDto MovieDetailsDto(int movieId);
        bool CreateProfile(string name, string password);
        bool LoginInfo(string name, string password);
        PersonDetailsDto PersonDetailsDto(int id);
    }
}
