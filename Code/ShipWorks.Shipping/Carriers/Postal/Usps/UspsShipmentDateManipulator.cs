using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Manipulate the date of the given shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.Usps)]
    public class UspsShipmentDateManipulator : IShipmentDateManipulator
    {
        private readonly IShippingSettings shippingSettings;
        private readonly DefaultShipmentDateManipulator defaultShipmentDateManipulator;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShipmentDateManipulator(IShippingSettings shippingSettings, IDateTimeProvider dateTimeProvider, 
            DefaultShipmentDateManipulator defaultShipmentDateManipulator)
        {
            this.defaultShipmentDateManipulator = defaultShipmentDateManipulator;
            this.dateTimeProvider = dateTimeProvider;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Manipulate the date of the given shipment
        /// </summary>
        public void Manipulate(ShipmentEntity shipment)
        {
            if (shipment.Processed)
            {
                return;
            }

            IShippingSettingsEntity shippingSettingsEntity = shippingSettings.FetchReadOnly();

            if (!shippingSettingsEntity.UspsShippingDateCutoffEnabled)
            {
                defaultShipmentDateManipulator.Manipulate(shipment);
                return;
            }

            TimeSpan shipDateCutoff = shippingSettingsEntity.UspsShippingDateCutoffTime;
            DateTime now = dateTimeProvider.Now;

            // Bring the past up to now
            if (shipment.ShipDate.Date < now.Date)
            {
                shipment.ShipDate = now;
            }

            if (now.TimeOfDay >= shipDateCutoff && now.Date == shipment.ShipDate.Date)
            {
                shipment.ShipDate = shipment.ShipDate.AddDays(1);
                
                if (shipment.ShipDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    shipment.ShipDate = shipment.ShipDate.AddDays(1);
                }
            }
        }
    }
}
