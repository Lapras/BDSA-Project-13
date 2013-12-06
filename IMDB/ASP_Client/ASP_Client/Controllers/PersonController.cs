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
    public class PersonController : Controller
    {
        
        public async Task<ActionResult> PersonDetails(int id)
        {

            var personDetails = await GetPersonDetailsLocallyAsync(id);

            var personDetailsViewModel = new PersonDetailsViewModel();

            if (personDetails != null)
            {
                personDetailsViewModel.Id = personDetails[0].Id;
                personDetailsViewModel.Name = personDetails[0].Name;
                personDetailsViewModel.Gender = personDetails[0].Gender;
            }
            throw new NotImplementedException();

            // Can't resolve the PersonDetails view for some reason

           // return  View(personDetailsViewModel);

        }

        private async Task<List<PersonDetailsDto>> GetPersonDetailsLocallyAsync(int personId)
        {
            using (var httpClient = new HttpClient())
            {
                return JsonConvert.DeserializeObject<List<PersonDetailsDto>>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?personId=" + personId)
                );
            }
        }
    }
}