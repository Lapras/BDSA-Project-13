using System.Net.Http;
using System.Threading.Tasks;
using ASP_Client.Models;
using DtoSubsystem;

namespace ASP_Client.ClientRequests
{
    public interface IUserRepository
    {
        Task<ReplyDto> Login(UserModel user);

        Task<ReplyDto> Registration(UserModel user);
    }
}
