using System.Collections.Generic;
using System.Threading.Tasks;
using DtoSubsystem;

namespace ASP_Client.ClientRequests
{
    /// <summary>
    /// Interface for the methods concerning people in the movie business
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>
        /// Get a list of actors matching the given name
        /// </summary>
        /// <param name="personName">Name of the person to look for</param>
        /// <returns>List of the people matching the given name</returns>
        Task<List<PersonDto>> GetPersonAsync(string personName);

        /// <summary>
        /// Get the details of a specific person involved in the movie business
        /// </summary>
        /// <param name="personId">Id of the person to get the data of</param>
        /// <returns>Detailed data of the person</returns>
        Task<PersonDetailsDto> GetPersonDetailsLocallyAsync(int personId);
    }
}