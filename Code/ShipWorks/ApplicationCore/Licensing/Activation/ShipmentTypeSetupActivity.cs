using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    public class ShipmentTypeSetupActivity : IShipmentTypeSetupActivity
    {
        readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory;
        readonly IShippingSettings shippingSettings;
        readonly IShippingProfileManager shippingProfileManager;

        /// <summary>
        /// Sets up the shipment type as the default
        /// </summary>
        public ShipmentTypeSetupActivity(IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory, IShippingSettings shippingSettings,
            IShippingProfileManager shippingProfileManager)
        {
            this.shippingProfileManager = shippingProfileManager;
            this.shippingSettings = shippingSettings;
            this.shipmentTypeFactory = shipmentTypeFactory;
        }
        
        /// <summary>
        /// Sets usps as the default ShipmentType and sets up its origin address
        /// </summary>
        public void InitializeShipmentType(ShipmentTypeCode shipmentTypeCode, ShipmentOriginSource origin)
        {
            shippingSettings.SetDefaultProvider(shipmentTypeCode);

            ShippingProfileEntity uspsProfile = shipmentTypeFactory[shipmentTypeCode].GetPrimaryProfile();
            uspsProfile.OriginID = (int)origin;

            shippingProfileManager.SaveProfile(uspsProfile);
        }
    }
}
