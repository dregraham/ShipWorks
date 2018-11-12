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
            "http://support.shipworks.com/support/solutions/articles/4000097089-adding-a-sellercloud-store";

        /// <summary>
        /// Get value entered in Carrier Name field
        /// </summary>
        public override string GetOnlineCarrierName(ShipmentEntity shipment)
        {
            if (ShipmentTypeCode.Other == shipment.ShipmentTypeCode)
            {
                return shipment.Other.Carrier.ToString();
            }

            return base.GetOnlineCarrierName(shipment);
        }
    }
}
