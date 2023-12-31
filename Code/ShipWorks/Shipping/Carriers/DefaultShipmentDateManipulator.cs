﻿using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Default manipulator of shipment date
    /// </summary>
    [Component(RegistrationType.Self)]
    public class DefaultShipmentDateManipulator : IShipmentDateManipulator
    {
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor for unit tests
        /// </summary>
        protected DefaultShipmentDateManipulator()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DefaultShipmentDateManipulator(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Manipulate the date of the given shipment
        /// </summary>
        public virtual void Manipulate(ShipmentEntity shipment)
        {
            var now = dateTimeProvider.Now;
            var timeZoneInfo = dateTimeProvider.TimeZoneInfo;

            if (!shipment.Processed && shipment.ShipDate.Date < now.Date)
            {
                shipment.ShipDate = now.Date.ToUniversalTime(timeZoneInfo);
            }
        }
    }
}
