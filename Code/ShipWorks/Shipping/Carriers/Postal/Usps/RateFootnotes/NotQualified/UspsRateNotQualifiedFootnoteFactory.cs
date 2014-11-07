﻿using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.NotQualified
{
    /// <summary>
    /// Creates USPS Rate Not Qualified footnote control.
    /// </summary>
    public class UspsRateNotQualifiedFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Construct a new UspsRateNotQualifiedFootnoteFactory object
        /// </summary>
        /// <param name="shipmentType">Type of shipment that instantiated this factory</param>
        public UspsRateNotQualifiedFootnoteFactory(ShipmentType shipmentType)
        {
            ShipmentType = shipmentType;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Create an USPS rate not qualified control
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UspsRateNotQualifiedFootnote();
        }
    }
}
