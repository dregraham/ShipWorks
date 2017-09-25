using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.UI.Dialogs.UserConditionalNotification
{
    /// <summary>
    /// Interaction logic for UserConditionalNotification.xaml
    /// </summary>
    [Component]
    public partial class UserConditionalNotificationDialog : Window, IUserConditionalNotificationDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserConditionalNotificationDialog()
        {
            InitializeComponent();
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
            new WindowInteropHelper(this) { Owner = owner.Handle };
        }
    }
}
