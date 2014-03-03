﻿using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Rate group that contains the reason for invalid results
    /// </summary>
    public class InvalidRateGroup : RateGroup
    {
        private readonly Exception exception;

        /// <summary>
        /// Create a new instance of an InvalidRateGroup
        /// </summary>
        /// <param name="shipmentType">Type of shipment associated with the rates</param>
        /// <param name="exception">Exception that caused the invalid rates</param>
        public InvalidRateGroup(ShipmentType shipmentType, Exception exception)
            : base(new List<RateResult>())
        {
            this.exception = exception;

            base.Rates.Add(new RateResult("No rates are available for this shipment.", string.Empty)
            {
                ShipmentType = shipmentType.ShipmentTypeCode
            });

            base.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(shipmentType, exception.Message));
        }

        /// <summary>
        /// Get the shipping exception that caused the invalid rates
        /// </summary>
        public ShippingException ShippingException
        {
            get
            {
                return exception as ShippingException ?? new ShippingException(exception.Message, exception);
            }
        }
    }
}
