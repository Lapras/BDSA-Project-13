using System.Net.Http;
using System.Threading.Tasks;
using ASP_Client.Controllers;
using ASP_Client.Models;
using DtoSubsystem;

namespace ASP_Client.ClientRequests
{
    /// <summary>
    /// Class in charge of calling the communication facade for user related actions and
    /// returning the Http responses it gets back.
    /// Implementing the IUserRepository interface.
    /// </summary>
    class UserRepository : IUserRepository
    {
        /// <summary>
        /// Method calling the CommunicationFacade's login method with a UserModel
        /// </summary>
        /// <param name="user"> Model containing the input from a user </param>
        /// <returns> Response message from the Communication facade </returns>
        public Task<ReplyDto> Login(UserModel user)
        { 
            return CommunicationFacade.Login(user);         
        }

        /// <summary>
        /// Method calling the CommunicationFacade's registration method with a UserModel
        /// </summary>
        /// <param name="user"> Model containing the input from a user </param>
        /// <returns> Response message from the Communication facade </returns>
        public Task<ReplyDto> Registration(UserModel user)
        {
            return CommunicationFacade.Registration(user);
        }
    }
}