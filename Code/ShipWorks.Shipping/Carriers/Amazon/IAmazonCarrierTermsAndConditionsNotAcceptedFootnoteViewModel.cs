using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Interface for the Amazon carrier terms and conditions footnote view model so that the implementation
    /// can live in a different assembly
    /// </summary>
    public interface IAmazonCarrierTermsAndConditionsNotAcceptedFootnoteViewModel
    {
        /// <summary>
        /// Names of the carriers to show in the dialog box
        /// </summary>
        IEnumerable<string> CarrierNames { get; set; }
    }
}