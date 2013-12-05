using System.Net.Http;
using System.Threading.Tasks;
using ASP_Client.Models;

namespace ASP_Client.ClientRequests
{
    class UserRepository : IUserRepository
    {
        public async Task<HttpResponseMessage> Login(UserModel user)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.PostAsJsonAsync("http://localhost:54321/User/Login", user);
            }
        }

        public async Task<HttpResponseMessage> Registration(UserModel user)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.PostAsJsonAsync("http://localhost:54321/User/Registration", user);
            }
        }
    }
}