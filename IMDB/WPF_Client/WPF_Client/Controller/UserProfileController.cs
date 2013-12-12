using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Model;
using WPF_Client.View;
using WPF_Client.ViewModel;

namespace WPF_Client.Controller
{

    /// <summary>
    /// Manages application control flow whenever users wants to create or view his/her
    ///  userprofile, including managing his/her movie preference lists.
    /// </summary>
    public static class UserProfileController
    {

        private static IModel _model = new Model.Model();

        /// <summary>
        /// Creates a profile with the supplied username and password.
        /// </summary>
        /// <param name="name">The requested username.</param>
        /// <param name="password">The requested password.</param>
        /// <returns>A boolean value whether the user creation was successfull or not.</returns>
        public static bool CreateProfile(string name, string password)
        {
            var result = _model.CreateProfile(name, password);

            return result;
        }

    }
}
