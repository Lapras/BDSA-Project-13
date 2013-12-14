using System.Threading.Tasks;
using ASP_Client.Models;
using DtoSubsystem;

namespace ASP_Client.ClientRequests
{
    /// <summary>
    /// Interface for the methods concerning the user
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Method calling the CommunicationFacade's login method with a UserModel
        /// </summary>
        /// <param name="user"> Model containing the input from a user </param>
        /// <returns> Response message from the Communication facade </returns>
        Task<ReplyDto> Login(UserModel user);

        /// <summary>
        /// Method calling the CommunicationFacade's registration method with a UserModel
        /// </summary>
        /// <param name="user"> Model containing the input from a user </param>
        /// <returns> Response message from the Communication facade </returns>
        Task<ReplyDto> Registration(UserModel user);
    }
}
