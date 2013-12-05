using ASP_Client.Models;

namespace ASP_Client.Session
{
    public interface IUserSession
    {
        bool IsLoggedIn();

        void Login(UserModel user);

        void Logout();

        UserModel GetLoggedInUser();
    }
}