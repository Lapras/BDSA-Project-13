using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
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
        /// <summary>
        /// Method creating a list of movies based on a search string and puts them in a MovieOverviewViewModel which
        /// is given to the IndexView.
        /// The method first searches in the local database. If nothing is found, it searches in IMDb's database. If
        /// movies are found there they are added to the local database.
        /// </summary>
        /// <param name="searchString"> search criteria for movies </param>
        /// <returns> A Task containing an ActionResult to be handled </returns>        
        public async Task<ActionResult> SearchPerson(string searchString)
        {
            //if (Session["User"] == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}

            var foundPeople = await Storage.GetPersonAsync(searchString);

            var personOverviewViewModel = new PersonOverviewViewModel();

            if (foundPeople.First().ErrorMsg.IsEmpty())
            {

                if (foundPeople.Count != 0)
                {
                    personOverviewViewModel.FoundPeople = foundPeople;
                }
            }
            else
            {
                personOverviewViewModel.ErrorMsg = foundPeople.First().ErrorMsg;
            }

            return View(personOverviewViewModel);
        }

        /// <summary>
        /// Get the information of a person matching the id and return it to a view
        /// </summary>
        /// <param name="id">Id of the person to search for</param>
        /// <returns>The View provided with the new information model</returns>
        public async Task<ActionResult> PersonDetails(int id)
        {
            var personDetails = await Storage.GetPersonDetailsLocallyAsync(id);

            var personDetailsViewModel = new PersonDetailsViewModel();

            if (personDetails.ErrorMsg.IsEmpty())
            {

                if (personDetails != null)
                {
                    personDetailsViewModel.Id = personDetails.Id;
                    personDetailsViewModel.Name = personDetails.Name;
                    personDetailsViewModel.Gender = personDetails.Gender;
                    personDetailsViewModel.Role = personDetails.Role;

                    var temp = personDetails.Info.Select(detail => new InfoModel()
                    {
                        Name = detail.Name,
                        Info = detail.Info
                    }).ToList();

                    personDetailsViewModel.Info = temp;
                }
            }
            else
            {
                personDetailsViewModel.ErrorMsg = personDetails.ErrorMsg;
            }

            return  View(personDetailsViewModel);
        }
    }
}
