using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// An IShipmentProcessingSynchronizer implementation to handle the PreProcessing of a
    /// Amazon shipment 
    /// </summary>
    public class AmazonShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public AmazonShipmentProcessingSynchronizer(IShippingSettings shippingSettings)
        {
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Gets a value indicating whether the shipment type [has accounts].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts =>
            // No accounts for Amazon, return true if Amazon has been configured
            shippingSettings.GetConfiguredTypes().Contains(ShipmentTypeCode.Amazon);
   
        

        /// <summary>
        /// Saves the first account ID found to the shipment. The presumption here is that a new 
        /// account was just created during the processing of a shipment, so we just want 
        /// to grab the first account (the one just created) and use it to process the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            // Do nothing - there aren't any accounts for Amazon
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is an account available,
        /// this method will change the account to be the first account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            // Do nothing - there aren't any accounts for Amazon
        }
    }
}
