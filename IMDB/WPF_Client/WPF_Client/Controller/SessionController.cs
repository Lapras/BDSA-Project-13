using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Model;
using WPF_Client.PwBoxAssistant;
using WPF_Client.View;
using WPF_Client.ViewModel;

namespace WPF_Client.Controller
{

    /// <summary>
    /// Manages login and logout control flow.
    /// </summary>
    public class SessionController
    {
        public static IModel _model = new Model.Model();
        public static bool _isLoggedIn;
        public static string _currentUser;
        private static string _username;

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="name">The input username.</param>
        /// <param name="password">The input password.</param>
        /// <returns>A boolean value whether the user was successfully logged in.</returns>
        public static bool LoginIn(string name, string password)
        {
            //handle logic here checking if the user is logged in

            // maybe only handle the non encryptet PW and check it agains the enrcypted pw which is recived from the DB.
            /*
             * so that the encryption happens when the user is created and is validated in the session control
             * 
             * 
             */
            string _pw = PasswordHash.CreateHash(password);

            if (_model.Login(name, password) && PasswordHash.ValidatePassword(password, _pw))
            {
                _currentUser = name;
                _isLoggedIn = true;


                if (!UnitTestDetector.IsInUnitTest)
                {
                    ViewModelManager.Main.TopViewModel = new TopSearchViewModel();
                }

                return true;
            }
            else
            {
                _currentUser = null;
                _isLoggedIn = false;
                return false;
            }



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
