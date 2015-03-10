using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsAccountConvertedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAccountConvertedEventArgs"/> class.
        /// </summary>
        /// <param name="account">The account.</param>
        public UspsAccountConvertedEventArgs(UspsAccountEntity account)
        {
            Account = account;
        }

        /// <summary>
        /// Gets the account that was converted.
        /// </summary>
        public UspsAccountEntity Account { get; private set; }
    }
}
