using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Settings;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Manipulate the date of the given shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.UpsWorldShip)]
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.FedEx)]
    public class WeekdaysOnlyShipmentDateManipulator : IShipmentDateManipulator
    {
        private readonly IShippingSettings shippingSettings;
        private readonly DefaultShipmentDateManipulator defaultShipmentDateManipulator;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public WeekdaysOnlyShipmentDateManipulator(IShippingSettings shippingSettings, IDateTimeProvider dateTimeProvider,
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
            ShipmentDateCutoff cutoff = shippingSettingsEntity.GetShipmentDateCutoff(shipment.ShipmentTypeCode);

            if (!cutoff.Enabled)
            {
                defaultShipmentDateManipulator.Manipulate(shipment);
                return;
            }

            TimeSpan shipDateCutoff = cutoff.CutoffTime;
            DateTime now = dateTimeProvider.Now;

            // Bring the past up to now
            if (shipment.ShipDate.Date < now.Date)
            {
                shipment.ShipDate = now;
            }

            if (now.TimeOfDay >= shipDateCutoff && now.Date == shipment.ShipDate.Date ||
                shipment.ShipDate.DayOfWeek == DayOfWeek.Saturday ||
                shipment.ShipDate.DayOfWeek == DayOfWeek.Sunday)
            {
                shipment.ShipDate = shipment.ShipDate.AddDays(1);

                if (shipment.ShipDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    shipment.ShipDate = shipment.ShipDate.AddDays(1);
                }

                if (shipment.ShipDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    shipment.ShipDate = shipment.ShipDate.AddDays(1);
                }
            }
        }
    }
}
