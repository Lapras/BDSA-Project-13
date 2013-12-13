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

        /// <summary>
        /// Check if the user is already logged in
        /// </summary>
        /// <returns>True if logged in</returns>
        public bool IsLoggedIn()
        {
            return _httpSessionStateBase["User"] != null;
        }
        
        /// <summary>
        /// Create a new session for the user
        /// </summary>
        /// <param name="user">User who gets a session</param>
        public void Login(UserModel user)
        {
            _httpSessionStateBase.Add("User", user);
        }

        /// <summary>
        /// Log the current user out
        /// </summary>
        public void Logout()
        {
            _httpSessionStateBase.Remove("User");
        }

        /// <summary>
        /// Get the information of the current logged in user
        /// </summary>
        /// <returns>Model with the user information</returns>
        public UserModel GetLoggedInUser()
        {
            return _httpSessionStateBase["User"] as UserModel;
        }
    }
}