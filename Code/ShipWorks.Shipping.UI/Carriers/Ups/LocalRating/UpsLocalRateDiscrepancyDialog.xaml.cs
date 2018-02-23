using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.UI.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Interaction logic for UpsLocalRateDiscrepancyDialog.xaml
    /// </summary>
    [NamedComponent("UpsLocalRateDiscrepancyDialog", typeof(IDialog))]
    public partial class UpsLocalRateDiscrepancyDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalRateDiscrepancyDialog(IWin32Window owner) 
            : base(owner, false)
        {
            InitializeComponent();
        }
    }
}
