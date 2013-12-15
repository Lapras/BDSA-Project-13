using WPF_Client.Model;
using WPF_Client.PwBoxAssistant;
using WPF_Client.ViewModel;

namespace WPF_Client.Controller
{

    /// <summary>
    /// Manages login and logout control flow.
    /// </summary>
    public class SessionController
    {
        public static IModel _model = new Model.Model();

        /// <summary>
        /// A boolean containing whethe the user is currently logged in.
        /// </summary>
        public static bool _isLoggedIn; 

        /// <summary>
        /// The current user that is logged in.
        /// </summary>
        public static string _currentUser;


        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="name">The input username.</param>
        /// <param name="password">The input password.</param>
        /// <returns>A boolean value whether the user was successfully logged in.</returns>
        public static bool Login(string name, string password)
        {

            string _pw = PasswordHash.CreateHash(password);

            if (_model.Login(name, password) && PasswordHash.ValidatePassword(password, _pw))
            {
                _currentUser = name;
                _isLoggedIn = true;

                //unit test doesnt like creating a new viewmodel and assigning it.
                if (!UnitTestDetector.IsInUnitTest)
                {
                    ViewModelManager.Main.TopViewModel = new TopSearchViewModel();
                }

                return true;
            }

            _currentUser = null;
            _isLoggedIn = false;
            return false;
            
        }

        /// <summary>
        /// Returns the name of the current user that is logged in.
        /// </summary>
        /// <returns>The name of the current user</returns>
        public static string CurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Returns whether the user is logged in.
        /// </summary>
        /// <returns>A boolean value whether the user is logged in or not</returns>
        public static bool IsLoggedIn()
        {
            return _isLoggedIn;
        }


        /// <summary>
        /// Logs out a user.
        /// </summary>
        public static void Logout()
        {
            if (!UnitTestDetector.IsInUnitTest)
            {
                ViewModelManager.Main.TopViewModel = new LoginViewModel();
            }

            _currentUser = null;
            _isLoggedIn = false;
        } 
    }
}
