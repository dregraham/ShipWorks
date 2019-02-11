using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.SellerCloud
{
    /// <summary>
    /// SellerCloud generic module store implementation
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.SellerCloud, ExternallyOwned = true)]
    public class SellerCloudStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SellerCloudStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {
        }

        /// <summary>
        /// StoreTypeCode enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.SellerCloud;

        /// <summary>
        /// Log request/responses as SellerCloud
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.SellerCloud;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl =>
            "https://shipworks.zendesk.com/hc/en-us/articles/360022654091";

        /// <summary>
        /// Get value entered in Carrier Name field
        /// </summary>
        public override string GetOnlineCarrierName(ShipmentEntity shipment)
        {
            if (shipment.ShipmentTypeCode == ShipmentTypeCode.Other)
            {
                return shipment.Other.Carrier.ToString();
            }

            return base.GetOnlineCarrierName(shipment);
        }
    }
}
