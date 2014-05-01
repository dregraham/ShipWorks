using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Factory to crate a RateNotSupportedFootnote
    /// </summary>
    public class RatesNotSupportedFootnoteFactory : IRateFootnoteFactory
    {
        private readonly Action<ShipmentTypeCode> shipmentTypeSelected;

        public RatesNotSupportedFootnoteFactory(Action<ShipmentTypeCode> shipmentTypeSelected)
        {
            this.shipmentTypeSelected = shipmentTypeSelected;
        }

        public ShipmentType ShipmentType { get; private set; }

        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new RatesNotSupportedFootnoteControl(shipmentTypeSelected);
        }
    }
}
