using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.OpenSky
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.OpenSky)]
    [Component(RegistrationType.Self)]
    public class OpenSkyStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenSkyStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager) :
            base(store, messageHelper, orderManager)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.OpenSky;

        /// <summary>
        /// Log request/responses as OpenSky
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.OpenSky;

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl => "http://support.shipworks.com/support/solutions/articles/4000062791";

        /// <summary>
        /// Gets the online store's carrier name
        /// </summary>
        public override string GetOnlineCarrierName(ShipmentEntity shipment)
        {
            if (ShipmentTypeManager.IsPostal(shipment.ShipmentTypeCode))
            {
                ShippingManager.EnsureShipmentLoaded(shipment);

                if (ShipmentTypeManager.IsDhl((PostalServiceType) shipment.Postal.Service))
                {
                    return "DHL ECOMMERCE";
                }
            }

            return base.GetOnlineCarrierName(shipment);
        }
    }
}
