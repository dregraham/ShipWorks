﻿using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Settings;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Manipulate the date of the given shipment
    /// </summary>
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.Express1Endicia)]
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.PostalWebTools)]
    [KeyedComponent(typeof(IShipmentDateManipulator), ShipmentTypeCode.Express1Usps)]
    public class PostalShipmentDateManipulator : IShipmentDateManipulator
    {
        private readonly IShippingSettings shippingSettings;
        private readonly DefaultShipmentDateManipulator defaultShipmentDateManipulator;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalShipmentDateManipulator(IShippingSettings shippingSettings, IDateTimeProvider dateTimeProvider,
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
            var shipDateTime = shipment.ShipDate.ToLocalTime();

            if (shipDateTime.Date < now.Date)
            {
                shipment.ShipDate = now;
            }

            if (now.TimeOfDay >= shipDateCutoff && now.Date == shipDateTime.Date ||
                shipDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                shipment.ShipDate = shipment.ShipDate.AddDays(1);
                shipDateTime = shipDateTime.AddDays(1);

                if (shipDateTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    shipment.ShipDate = shipment.ShipDate.AddDays(1);
                }             
            }
        }
    }
}
