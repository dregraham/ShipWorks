using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Postal rate selection that is aware of accounts
    /// </summary>
    public class UspsPostalRateSelection : PostalRateSelection
    {
        public UspsPostalRateSelection(PostalServiceType serviceType, PostalConfirmationType confirmationType, UspsAccountEntity account)
            : base(serviceType, confirmationType)
        {
            Accounts = new List<UspsAccountEntity> { account };
        }

        /// <summary>
        /// Accounts associated with this rate
        /// </summary>
        public List<UspsAccountEntity> Accounts { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            UspsPostalRateSelection postalRateSelection = obj as UspsPostalRateSelection;

            return postalRateSelection != null &&
                postalRateSelection.Accounts.Count == Accounts.Count &&
                postalRateSelection.Accounts.Select(account => account.UspsAccountID)
                    .All(x => Accounts.Select(account => account.UspsAccountID).Contains(x)) &&
                base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() + Accounts.GetHashCode();
        }

        /// <summary>
        /// Is the rate compatible with the specified shipment
        /// </summary>
        public bool IsRateFor(ShipmentEntity shipment)
        {
            if (shipment == null || shipment.Postal == null)
            {
                throw new ArgumentNullException("shipment");
            }

            // For international flat rate envelope, customers get delivery confirmation for free.  So that they know this,
            // we display it in the service control drop down.  However, we do not receive a confirmation type in the rate 
            // from USPS.  
            // So check the available confirmation types for the shipment, and if there is only 1 and it matches the shipment
            // selected confirmation type, we have a match in this scenario.
            // We'll check this OR'd with the actual check between the rate and shipment confirmation.
            List<PostalConfirmationType> availableConfirmationTypes = new UspsShipmentType().GetAvailableConfirmationTypes(shipment.ShipCountryCode, (PostalServiceType) shipment.Postal.Service, (PostalPackagingType) shipment.Postal.PackagingType);
            bool confirmationMatches = availableConfirmationTypes.Count == 1 && 
                                       availableConfirmationTypes.First() == (PostalConfirmationType) shipment.Postal.Confirmation;

            return ServiceType == (PostalServiceType) shipment.Postal.Service && 
                (confirmationMatches || ConfirmationType == (PostalConfirmationType) shipment.Postal.Confirmation);
        }
    }
}
