using System.Windows.Controls;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Orders.Archive;

namespace ShipWorks.Stores.UI.Orders.Archive
{
    /// <summary>
    /// Interaction logic for ArchiveNotification.xaml
    /// </summary>
    [Component]
    public partial class ArchiveNotification : UserControl, IArchiveNotification
    {
        public ArchiveNotification()
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
