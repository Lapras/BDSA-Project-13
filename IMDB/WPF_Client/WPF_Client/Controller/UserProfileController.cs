using WPF_Client.Model;

namespace WPF_Client.Controller
{

    /// <summary>
    /// Manages application control flow whenever users wants to create or view his/her
    ///  userprofile, including managing his/her movie preference lists.
    /// </summary>
    public static class UserProfileController
    {

        public static IModel _model = new Model.Model();

        /// <summary>
        /// Creates a profile with the supplied username and password.
        /// </summary>
        /// <param name="name">The requested username.</param>
        /// <param name="password">The requested password.</param>
        /// <returns>A boolean value whether the user creation was successfull or not.</returns>
        public static bool CreateProfile(string name, string password)
        {
            var result = _model.CreateProfile(name, password);

            //unit test doesnt like creating a new viewmodel and assigning it.
            if (!UnitTestDetector.IsInUnitTest)
            {
                ViewModelManager.Main.CurrentViewModel = ViewModelManager.PreviousViewModelsStack.Pop();
            }

            return result;
        }

    }
}
