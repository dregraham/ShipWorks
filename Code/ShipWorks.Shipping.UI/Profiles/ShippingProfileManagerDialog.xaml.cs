using System.Windows.Forms;

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
        public ShippingProfileManagerDialog(IWin32Window owner, object viewModel) : base(owner, viewModel)
        {
            InitializeComponent();

            // Need to set topmost to false because this window opens a winforms window
            // which will appear behind this one if topmost is true
            Topmost = false;
            ResizeMode = System.Windows.ResizeMode.CanResizeWithGrip;
        }
    }
}
