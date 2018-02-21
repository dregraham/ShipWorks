using System.Collections.Generic;
using System.Windows.Input;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// Interface that represents the Shipping Profile Manager Dialog view model
    /// </summary>
    interface IShippingProfileManagerDialogViewModel
    {
        /// <summary>
        /// Add a profile
        /// </summary>
        ICommand AddCommand { get; }

        /// <summary>
        /// Edit the selected profile
        /// </summary>
        ICommand EditCommand { get; }

        /// <summary>
        /// Delete the selected profile
        /// </summary>
        ICommand DeleteCommand { get; }

        /// <summary>
        /// Shipping Profiles loaded in the dialog
        /// </summary>
        IEnumerable<ShippingProfileEntity> ShippingProfiles { get; }

        /// <summary>
        /// Selected Shipping Profile
        /// </summary>
        ShippingProfileEntity SelectedShippingProfile { get; }
    }
}
