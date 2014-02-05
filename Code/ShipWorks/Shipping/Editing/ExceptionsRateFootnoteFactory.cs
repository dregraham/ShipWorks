using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Editing
{
    public class ExceptionsRateFootnoteFactory : IRateFootnoteFactory
    {
        public ShipmentType ShipmentType { get; private set; }
        private readonly string errorMessage;

        public ExceptionsRateFootnoteFactory(ShipmentType shipmentType, string errorMessage)
        {
            ShipmentType = shipmentType;
            this.errorMessage = errorMessage;
        }

        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new ExceptionsRateFootNoteControl(errorMessage);
        }
    }
}
