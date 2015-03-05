﻿using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    /// <summary>
    /// Class for populating the Stamps.com usage type combo box.
    /// </summary>
    public class StampsAccountUsageDropdownItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StampsAccountUsageDropdownItem"/> class.
        /// </summary>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="displayName">The display name.</param>
        public StampsAccountUsageDropdownItem(AccountType accountType, string displayName)
        {
            AccountType = accountType;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets the type of the account.
        /// </summary>
        public AccountType AccountType { get; private set; }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return DisplayName;
        }
    }
}