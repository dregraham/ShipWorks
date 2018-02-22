using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// Interaction logic for ShippingProfileManagerDialog.xaml
    /// </summary>
    public partial class ShippingProfileManagerDialog
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialog(IWin32Window owner, object viewModel) 
            : base(owner, viewModel, true)
        {
            InitializeComponent();            
        }
    }
}
