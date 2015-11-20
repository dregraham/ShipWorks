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

        /// <summary>
        /// Initializes a new instance of the <see cref="InformationFootnoteFactory"/> class.
        /// </summary>
        /// <param name="informationText">The information text.</param>
        public InformationFootnoteFactory(string informationText)
        {
            this.informationText = informationText;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Notes that this factory should be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return true; }
        }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <param name="parameters">Parameters that allow footnotes to interact with the rates grid</param>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new InformationFootnoteControl(informationText);
        }
    }
}
