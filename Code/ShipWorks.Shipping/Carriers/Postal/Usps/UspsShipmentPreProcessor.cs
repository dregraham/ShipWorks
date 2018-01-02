﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Usps specific Shipment preprocessor
    /// </summary>
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.Usps)]
    public class UspsShipmentPreProcessor : IShipmentPreProcessor
    {
        private readonly IDefaultShipmentPreProcessor defaultPreProcessor;
        private readonly IUspsFirstClassInternationalShipmentValidator firstClassInternationalShipmentValidator;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShipmentPreProcessor(IDefaultShipmentPreProcessor defaultPreProcessor, 
            IUspsFirstClassInternationalShipmentValidator firstClassInternationalShipmentValidator, 
            IDateTimeProvider dateTimeProvider)
        {
            this.defaultPreProcessor = defaultPreProcessor;
            this.firstClassInternationalShipmentValidator = firstClassInternationalShipmentValidator;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Run the preprocessor for USPS
        /// </summary>
        public IEnumerable<ShipmentEntity> Run(ShipmentEntity shipment, RateResult selectedRate, Action configurationCallback)
        {
            // do this check after 1/21/218 because thats when the USPS starts these new rules
            if (dateTimeProvider.Now >= new DateTime(2018, 01, 21))
            {
                firstClassInternationalShipmentValidator.ValidateShipment(shipment);
            }

            return defaultPreProcessor.Run(shipment, selectedRate, configurationCallback);
        }
    }
}
