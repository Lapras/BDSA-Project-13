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
    public static class UserProfileController
    {
        private static IModel _model = new Model.Model();

        public static bool CreateProfile(string name, string password)
        {
            var result = _model.CreateProfile(name, password);

            //if the user is successfully logged in we switch to the main menu.
            if(result)
            {
                if (!UnitTestDetector.IsInUnitTest)
                {
                    ViewModelManager.Main.CurrentViewModel = new SearchViewModel();
                }
            }


            return result;
        }

    }
}
