using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Services.Dialogs;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// Factory for creating the ShippingProfileManagerDialog
    /// </summary>
    [Component]
    public class ShippingProfileManagerDialogFactory : IShippingProfileManagerDialogFactory
    {
        private readonly IShippingProfileManagerDialogViewModel viewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogFactory(IShippingProfileManagerDialogViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Create the dialog
        /// </summary>
        public IDialog Create(IWin32Window owner)
        {
            IDialog dialog = new ShippingProfileManagerDialog();
            dialog.DataContext = viewModel;
            dialog.LoadOwner(owner);

            return dialog;
        }
    }
}
