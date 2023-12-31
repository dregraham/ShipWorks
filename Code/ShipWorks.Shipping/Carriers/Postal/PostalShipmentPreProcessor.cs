﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Usps specific Shipment preprocessor
    /// </summary>
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Endicia)]
    public class PostalShipmentPreProcessor : IShipmentPreProcessor
    {
        private readonly IDefaultShipmentPreProcessor defaultPreProcessor;
        private readonly IPostalFirstClassInternationalMailFraudWarning firstClassInternationalMailFraudWarning;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public PostalShipmentPreProcessor(IDefaultShipmentPreProcessor defaultPreProcessor,
            IPostalFirstClassInternationalMailFraudWarning firstClassInternationalMailFraudWarning,
            IDateTimeProvider dateTimeProvider)
        {
            this.defaultPreProcessor = defaultPreProcessor;
            this.firstClassInternationalMailFraudWarning = firstClassInternationalMailFraudWarning;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Run the preprocessor for USPS
        /// </summary>
        public async Task<IEnumerable<ShipmentEntity>> Run(ShipmentEntity shipment, RateResult selectedRate, Action configurationCallback)
        {
            // do this check after 1/21/218 because thats when the USPS starts these new rules
            if (dateTimeProvider.Now >= new DateTime(2018, 01, 21))
            {
                firstClassInternationalMailFraudWarning.Warn(shipment);
            }

            return await defaultPreProcessor.Run(shipment, selectedRate, configurationCallback);
        }
    }
}
