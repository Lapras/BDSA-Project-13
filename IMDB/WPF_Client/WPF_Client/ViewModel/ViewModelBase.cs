using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_Client.ViewModel
{

    /// <summary>
    /// Abstract class which all ViewModels extend inorder to provide basic functionality and avoid code duplication.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
