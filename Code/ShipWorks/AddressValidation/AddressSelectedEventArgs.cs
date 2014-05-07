using System;
using Interapptive.Shared.Business;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Event args for when an address is selected from the AddressSelector
    /// </summary>
    public class AddressSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddressSelectedEventArgs(AddressAdapter selectedAddress)
        {
            SelectedAddress = selectedAddress;
        }

        /// <summary>
        /// Address that was selected
        /// </summary>
        public AddressAdapter SelectedAddress { get; private set; }
    }
}