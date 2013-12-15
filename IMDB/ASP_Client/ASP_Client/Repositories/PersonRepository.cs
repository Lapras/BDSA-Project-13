using System.Collections.Generic;
using System.Threading.Tasks;
using ASP_Client.Controllers;
using DtoSubsystem;

namespace ASP_Client.ClientRequests
{
    /// <summary>
    /// Class in charge of calling the communication facade for person related actions and
    /// returning the Http responses it gets back.
    /// Implementing the IPersonRepository interface.
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        /// <summary>
        /// Get a list of actors matching the given name
        /// </summary>
        /// <param name="personName">Name of the person to look for</param>
        /// <returns>List of the people matching the given name</returns>
        public Task<List<PersonDto>> GetPersonAsync(string personName)
        {
            return Storage.GetPersonAsync(personName);
        }

        /// <summary>
        /// Get the details of a specific person involved in the movie business
        /// </summary>
        /// <param name="personId">Id of the person to get the data of</param>
        /// <returns>Detailed data of the person</returns>
        public Task<PersonDetailsDto> GetPersonDetailsLocallyAsync(int personId)
        {
            return Storage.GetPersonDetailsAsync(personId);
        }
    }
}