using ASP_Client.Models;

namespace ASP_Client.Session
{
    public interface IUserSession
    {
        /// <summary>
        /// Check if the user is already logged in
        /// </summary>
        /// <returns>True if logged in</returns>
        bool IsLoggedIn();

        /// <summary>
        /// Create a new session for the user
        /// </summary>
        /// <param name="user">User who gets a session</param>
        void Login(UserModel user);

        /// <summary>
        /// Log the current user out
        /// </summary>
        void Logout();
        
        /// <summary>
        /// Get the information of the current logged in user
        /// </summary>
        /// <returns>Model with the user information</returns>
        UserModel GetLoggedInUser();
    }
}