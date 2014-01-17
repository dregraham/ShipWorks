﻿using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// An implementation of the IRateFootnoteFactory that creates instances of a 
    /// BrokerExceptionsRateFootnoteControl.
    /// </summary>
    public class BrokerExceptionsRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly IEnumerable<BrokerException> brokerExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerExceptionsRateFootnoteFactory" /> class.
        /// </summary>
        /// <param name="shipmentType">The shipment type that the factory is being create for.</param>
        /// <param name="brokerExceptions">The broker exceptions.</param>
        /// <exception cref="System.InvalidOperationException">One or more broker exceptions must be provided.</exception>
        public BrokerExceptionsRateFootnoteFactory(ShipmentType shipmentType, IEnumerable<BrokerException> brokerExceptions)
        {
            IEnumerable<BrokerException> exceptions = brokerExceptions as IList<BrokerException> ?? brokerExceptions.ToList();

            if (!exceptions.Any())
            {
                throw new InvalidOperationException("One or more broker exceptions must be provided.");
            }

            ShipmentType = shipmentType;
            this.brokerExceptions = exceptions;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <returns>A BrokerExceptionsRateFootnoteControl object.</returns>
        public RateFootnoteControl CreateFootnote()
        {
            return new BrokerExceptionsRateFootnoteControl(brokerExceptions);
        }
    }
}
