using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Contains the results of performing an address validation
    /// </summary>
    public class StampsAddressValidationResults
    {
        /// <summary>
        /// Do the results contain a successful match
        /// </summary>
        public bool IsSuccessfulMatch { get; set; }

        /// <summary>
        /// If it's not a successful match, are the city, state, and zip ok?
        /// </summary>
        public bool IsCityStateZipOk { get; set; }

        /// <summary>
        /// Is the address a residence or business
        /// </summary>
        public ResidentialDeliveryIndicatorType ResidentialIndicator { get; set; }

        /// <summary>
        /// Is the address a PO Box
        /// </summary>
        public bool? IsPoBox { get; set; }

        /// <summary>
        /// The address that was matched
        /// </summary>
        public Address MatchedAddress { get; set; }

        /// <summary>
        /// Candidate addresses if a full match wasn't found
        /// </summary>
        public IEnumerable<Address> Candidates { get; set; }
    }
}
