using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// An IShipmentProcessingSynchronizer implementation to handle the PreProcessing 
    /// of an Other shipment 
    /// </summary>
    public class OtherShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        /// <summary>
        /// Gets a value indicating whether the shipment type [has accounts].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts
        {
            get 
            {
                // Since there aren't any actual accounts for "Other", we'll just check
                // whether the shipment type has been configured
                ShippingSettingsEntity settings = ShippingSettings.Fetch();
                return settings.ConfiguredTypes.Contains((int)ShipmentTypeCode.Other);
            }
        }

        /// <summary>
        /// Saves the first account ID found to the shipment. The presumption here is that a new
        /// account was just created during the processing of a shipment, so we just want
        /// to grab the first account (the one just created) and use it to process the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            // Do nothing - there aren't any accounts for Other
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is only one account available,
        /// this method will change the account to be that one account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            // Do nothing - there aren't any accounts for Other
        }
    }
}
