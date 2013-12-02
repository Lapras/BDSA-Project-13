using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using WPF_Client.ViewModel;

namespace WPF_Client
{

    /// <summary>
    /// Class which maintains a public reference to the MainWindowViewModel, so whenever the application should change view the MainWindowViewModel's CurrentViewModel should be changed to another ViewModel.
    /// </summary>
    public static class ViewModelManager
    {
        private static MainWindowViewModel _main;

        public static MainWindowViewModel Main
        {
            get
            {
                return _main;
            }
            set
            {
                _main = value;                
            }
        }
    }

    
}
