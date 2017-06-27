using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

namespace ShipWorks.UI.Dialogs.UserConditionalNotification
{
    /// <summary>
    /// Interaction logic for UserConditionalNotification.xaml
    /// </summary>
    [Component]
    public partial class UserConditionalNotification : Window, IUserConditionalNotification
    {
        public UserConditionalNotification()
        {
            InitializeComponent();
        }

        public void LoadOwner(IWin32Window owner)
        {
            throw new NotImplementedException();
        }

        public void ShowInformation(IWin32Window owner, string message, UserConditionalNotificationType notificationType)
        {
            throw new NotImplementedException();
        }
    }
}
