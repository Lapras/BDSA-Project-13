using System.Net.Http;
using System.Threading.Tasks;
using ASP_Client.Controllers;
using ASP_Client.Models;

namespace ASP_Client.ClientRequests
{
    class UserRepository : IUserRepository
    {
        public Task<HttpResponseMessage> Login(UserModel user)
        { // removed async
            return CommunicationFacade.Login(user);

            /* using (var httpClient = new HttpClient())
            {
                return await httpClient.PostAsJsonAsync("http://localhost:54321/User/Login", user);
            }*/
        }

        public Task<HttpResponseMessage> Registration(UserModel user)
        {
            return CommunicationFacade.Registration(user);
            /*       using (var httpClient = new HttpClient())
            {
                return await httpClient.PostAsJsonAsync("http://localhost:54321/User/Registration", user);
            }*/
        }
    }
}