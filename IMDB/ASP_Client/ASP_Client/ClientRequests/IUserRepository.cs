using System.Net.Http;
using System.Threading.Tasks;
using ASP_Client.Models;

namespace ASP_Client.ClientRequests
{
    public interface IUserRepository
    {
        Task<HttpResponseMessage> Login(UserModel user);

        Task<HttpResponseMessage> Registration(UserModel user);
    }
}
