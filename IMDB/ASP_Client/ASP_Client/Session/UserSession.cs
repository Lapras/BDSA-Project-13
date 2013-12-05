using System.Web;
using ASP_Client.Models;

namespace ASP_Client.Session
{
    public class UserSession : IUserSession
    {
        private readonly HttpSessionStateBase _httpSessionStateBase;

        public UserSession(HttpSessionStateBase httpSessionStateBase)
        {
            _httpSessionStateBase = httpSessionStateBase;
        }

        public bool IsLoggedIn()
        {
            return _httpSessionStateBase["User"] != null;
        }

        public void Login(UserModel user)
        {
            _httpSessionStateBase.Add("User", user);
        }

        public void Logout()
        {
            _httpSessionStateBase.Remove("User");
        }

        public UserModel GetLoggedInUser()
        {
            return _httpSessionStateBase["User"] as UserModel;
        }
    }
}