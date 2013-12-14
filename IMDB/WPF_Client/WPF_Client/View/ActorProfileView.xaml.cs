using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Client.ViewModel;

namespace WPF_Client.View
{
    /// <summary>
    /// Code behind for the view. There should be no code in here other than the basic initialization!
    /// </summary>
    public partial class ActorProfileView : UserControl
    {
        public ActorProfileView()
        {
            InitializeComponent();
            DataContext = new ActorProfileViewModel();
        }

        
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
    }
}
