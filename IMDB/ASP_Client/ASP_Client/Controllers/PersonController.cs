﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ASP_Client.Models;
using DtoSubsystem;
using Newtonsoft.Json;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// PersonController class is the controller for every view concerning a representation of a single person.
    /// From here the views get the required models to show.
    /// </summary>
    public class PersonController : BaseController
    {
        
        public async Task<ActionResult> PersonDetails(int id)
        {

            var personDetails = await CommunicationFacade.GetPersonDetailsLocallyAsync(id);

            var personDetailsViewModel = new PersonDetailsViewModel();

            if (personDetails != null)
            {
                personDetailsViewModel.Id = personDetails.Id;
                personDetailsViewModel.Name = personDetails.Name;
                personDetailsViewModel.Gender = personDetails.Gender;
            }
        //    throw new NotImplementedException();

            // Can't resolve the PersonDetails view for some reason 

            return  View(personDetailsViewModel);
        }
    }
}
