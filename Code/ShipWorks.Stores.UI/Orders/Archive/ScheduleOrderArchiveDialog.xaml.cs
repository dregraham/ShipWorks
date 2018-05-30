using System.Diagnostics.CodeAnalysis;
using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Stores.Orders.Archive;

namespace ShipWorks.Stores.UI.Orders.Archive
{
    /// <summary>
    /// Interaction logic for ScheduleOrderArchiveDialog.xaml
    /// </summary>
    [Component]
    public partial class ScheduleOrderArchiveDialog : IScheduleOrderArchiveDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScheduleOrderArchiveDialog()
        {
            InitializeComponent();

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Set the owner of this window
        /// </summary>
        [SuppressMessage("SonarQube", "S1848:Objects should not be created to be dropped immediately without being used",
            Justification = "The interop helper is only used temporarily to set this window's owner")]
        [SuppressMessage("Recommendations", "RECS0026:Objects should not be created to be dropped immediately without being used",
            Justification = "The interop helper is only used temporarily to set this window's owner")]
        public void LoadOwner(System.Windows.Forms.IWin32Window owner)
        {
            new WindowInteropHelper(this)
            {
                Owner = owner.Handle
            };
        }
    }
}
