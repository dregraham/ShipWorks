using System.Collections.Generic;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Contains the results of performing an address validation
    /// </summary>
    public class UspsAddressValidationResults
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

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string BadAddressMessage { get; set; }

        /// <summary>
        /// Gets or sets the status codes.
        /// </summary>
        public StatusCodes StatusCodes { get; set; }
    }
}
