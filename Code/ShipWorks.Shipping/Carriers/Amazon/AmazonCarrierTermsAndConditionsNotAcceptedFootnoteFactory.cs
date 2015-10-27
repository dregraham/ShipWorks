using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing.Rating;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Footnote factory for creating Amazon carrier terms and conditions footnote
    /// </summary>
    public class AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory : IRateFootnoteFactory
    {
        private List<string> carrierNames;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(ShipmentType shipmentType, List<string> carrierNames)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentType, nameof(shipmentType));
            MethodConditions.EnsureArgumentIsNotNull(carrierNames, nameof(carrierNames));

            this.ShipmentType = shipmentType;
            this.carrierNames = carrierNames;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl(carrierNames);
        }

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// For example, when using BestRate, we do not want Usps promo footnotes to display, so this will be set to false.
        /// </summary>
        public bool AllowedForBestRate => false;
    }
}
