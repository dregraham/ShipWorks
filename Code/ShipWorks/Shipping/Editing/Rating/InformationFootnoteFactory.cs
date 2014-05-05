using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Factory to crate a RateNotSupportedFootnote
    /// </summary>
    public class InformationFootnoteFactory : IRateFootnoteFactory
    {
        private readonly string informationText;

        public InformationFootnoteFactory(string informationText)
        {
            this.informationText = informationText;
        }

        public ShipmentType ShipmentType { get; private set; }

        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new InformationFootnoteControl(informationText);
        }
    }
}
