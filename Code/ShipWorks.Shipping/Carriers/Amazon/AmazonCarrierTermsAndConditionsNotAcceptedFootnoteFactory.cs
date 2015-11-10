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
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(ShipmentTypeCode shipmentType, IEnumerable<string> carrierNames)
        {
            MethodConditions.EnsureArgumentIsNotNull(carrierNames, nameof(carrierNames));

            ShipmentTypeCode = shipmentType;
            CarrierNames = carrierNames;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl(CarrierNames);
        }

        /// <summary>
        /// Notes that this factory should or should not be used in BestRate
        /// For example, when using BestRate, we do not want Usps promo footnotes to display, so this will be set to false.
        /// </summary>
        public bool AllowedForBestRate => false;

        /// <summary>
        /// Carrier names for Terms and Conditions
        /// </summary>
        public IEnumerable<string> CarrierNames { get; }
    }
}
