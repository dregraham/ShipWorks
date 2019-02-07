using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Help;

namespace ShipWorks.UI.Dialogs.AboutShipWorks
{
    /// <summary>
    /// About ShipWorks Dialog
    /// </summary>
    public partial class AboutShipWorksDialog : IAboutShipWorksDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AboutShipWorksDialog(AboutShipWorksViewModel viewModel, IWin32Window owner) :
            base(owner, viewModel, false)
        {
            InitializeComponent();
        }
    }
}
