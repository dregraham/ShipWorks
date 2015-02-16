using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// Class for populating the Stamps.com cred card type combo box.
    /// </summary>
    public class CreditCardTypeDropDownItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardTypeDropDownItem"/> class.
        /// </summary>
        /// <param name="cardType">Type of the card.</param>
        /// <param name="cardDisplayName">Display name of the card.</param>
        public CreditCardTypeDropDownItem(CreditCardType cardType, string cardDisplayName)
        {
            CardType = cardType;
            DisplayName = cardDisplayName;
        }

        /// <summary>
        /// Gets the type of the card.
        /// </summary>
        public CreditCardType CardType { get; private set; }

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
