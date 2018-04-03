using System.Windows.Controls;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Archiving;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Interaction logic for ArchiveNotificationControl.xaml
    /// </summary>
    [Component]
    public partial class ArchiveNotificationControl : UserControl, IArchiveNotification
    {
        public ArchiveNotificationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Add this control to the given host control
        /// </summary>
        public void AddTo(ElementHost host, IArchiveNotificationViewModel viewModel)
        {
            DataContext = viewModel;
            host.Child = this;
        }
    }
}
