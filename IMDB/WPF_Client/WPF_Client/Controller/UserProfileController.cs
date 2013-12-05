using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Client.Model;
using WPF_Client.View;

namespace WPF_Client.Controller
{
    public static class UserProfileController
    {
        private static IModel _model = new Model.Model();

        public static bool CreateProfile(string name, string password)
        {
            //handle logic here like checking the user's input characters and length etc. before sending createprofile request?

            return _model.CreateProfile(name, password);
        }

    }
}
