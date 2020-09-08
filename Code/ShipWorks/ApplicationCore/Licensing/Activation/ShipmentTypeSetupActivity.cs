using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    [Component]
    public class ShipmentTypeSetupActivity : IShipmentTypeSetupActivity
    {
        readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory;
        readonly IShippingSettings shippingSettings;
        readonly IShippingProfileManager shippingProfileManager;

        /// <summary>
        /// Sets up the shipment type as the default
        /// </summary>
        public ShipmentTypeSetupActivity(IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory,
            IShippingSettings shippingSettings, IShippingProfileManager shippingProfileManager)
        {
            this.shippingProfileManager = shippingProfileManager;
            this.shippingSettings = shippingSettings;
            this.shipmentTypeFactory = shipmentTypeFactory;
        }

        /// <summary>
        /// Initializes a shipment type, creating a default profile and optionally setting it to default 
        /// </summary>
        public void InitializeShipmentType(ShipmentTypeCode shipmentTypeCode,
            ShipmentOriginSource origin,
            bool forceSetDefault = true,
            ThermalLanguage? requestedLabelFormat = null)
        {
            var settings = shippingSettings.FetchReadOnly();

            if (forceSetDefault || settings.DefaultShipmentTypeCode == ShipmentTypeCode.None)
            {
                shippingSettings.SetDefaultProvider(shipmentTypeCode);
            }

            ShipmentType shipmentType = shipmentTypeFactory[shipmentTypeCode];

            ShippingProfileEntity profile = shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);
            profile.OriginID = (int) origin;

            if (requestedLabelFormat != null)
            {
                profile.RequestedLabelFormat = (int) requestedLabelFormat;
            }

            shippingProfileManager.SaveProfile(profile);
        }
    }
}
