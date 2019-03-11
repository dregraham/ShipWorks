using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Zentail
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    [KeyedComponent(typeof(StoreType), StoreTypeCode.Zentail)]
    [Component(RegistrationType.Self)]
    public class ZentailStoreType : GenericModuleStoreType
    {
        private readonly IShippingManager shippingManager;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ZentailStoreType(StoreEntity store, IMessageHelper messageHelper, IOrderManager orderManager,
                                IShippingManager shippingManager, IShipmentTypeManager shipmentTypeManager) :
            base(store, messageHelper, orderManager)
        {
            this.shippingManager = shippingManager;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// StoreTypeCode enum value
        /// </summary>
        public override StoreTypeCode TypeCode => StoreTypeCode.Zentail;

        /// <summary>
        /// Log request/response as Zentail
        /// </summary>
        public override ApiLogSource LogSource => ApiLogSource.Zentail;

        /// <summary>
        /// Gets the account settings help URL
        /// </summary>
        public override string AccountSettingsHelpUrl => "https://shipworks.zendesk.com/hc/en-us/articles/360022654791";

        /// <summary>
        /// Get carrier name to upload to ZenTail
        /// </summary>
        public override string GetOnlineCarrierName(ShipmentEntity shipment)
        {
            if (shipmentTypeManager.IsPostal(shipment.ShipmentTypeCode))
            {
                shippingManager.EnsureShipmentLoaded(shipment);

                if (shipmentTypeManager.IsDhl((PostalServiceType) shipment.Postal.Service))
                {
                    return "DHL GlobalMail";
                }
            }

            return base.GetOnlineCarrierName(shipment);
        }
    }
}