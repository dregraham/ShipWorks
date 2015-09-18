using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// 
    /// </summary>
    public class NoneShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public NoneShipmentProcessingSynchronizer(IShippingSettings shippingSettings)
        {
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Gets a value indicating whether the shipment type [has accounts].
        /// </summary>
        public bool HasAccounts => shippingSettings.Fetch().ConfiguredTypes.Contains((int)ShipmentTypeCode.None);

        /// <summary>
        /// Saves the first account ID found to the shipment. The presumption here is that a new
        /// account was just created during the processing of a shipment, so we just want
        /// to grab the first account (the one just created) and use it to process the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            // Do nothing - there aren't any accounts for None
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is only one account available,
        /// this method will change the account to be that one account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            // Do nothing - there aren't any accounts for None
        }
    }
}
