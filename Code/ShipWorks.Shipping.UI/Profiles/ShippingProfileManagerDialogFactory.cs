using System;
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
        private readonly Func<IShippingProfileManagerDialogViewModel> viewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingProfileManagerDialogFactory(Func<IShippingProfileManagerDialogViewModel> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
        }

        /// <summary>
        /// Create the dialog
        /// </summary>
        public IDialog Create(IWin32Window owner) => new ShippingProfileManagerDialog(owner, viewModelFactory());        
    }
}
