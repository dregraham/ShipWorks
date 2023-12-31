﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.UI.Profiles
{
    /// <summary>
    /// Interface that represents the Shipping Profile Manager Dialog view model
    /// </summary>
    public interface IShippingProfileManagerDialogViewModel
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
        ObservableCollection<IEditableShippingProfile> ShippingProfiles { get; }

        /// <summary>
        /// Selected Shipping Profile
        /// </summary>
        IEditableShippingProfile SelectedShippingProfile { get; set; }
    }
}
