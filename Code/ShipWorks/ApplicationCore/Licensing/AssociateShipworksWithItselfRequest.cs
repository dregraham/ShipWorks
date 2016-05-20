using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// A Request to associate a shipworks account with a new stamps account that
    /// stamps creates when a user creates a shipworks account.
    /// </summary>
    public class AssociateShipworksWithItselfRequest
    {
        private readonly IUspsWebClient uspsWebClient;
        private readonly ITangoWebClient tangoWebClient;

        const int NumberOfSuggestionsToShow = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociateShipworksWithItselfRequest"/> class.
        /// </summary>
        public AssociateShipworksWithItselfRequest(IUspsWebClient uspsWebClient, ITangoWebClient tangoWebClient)
        {
            this.uspsWebClient = uspsWebClient;
            this.tangoWebClient = tangoWebClient;
        }

        /// <summary>
        /// Gets or sets the card holder name.
        /// </summary>
        public string CardHolderName { get; set; }

        /// <summary>
        /// Gets or sets the type of the card.
        /// </summary>
        public CreditCardType CardType { get; set; }

        /// <summary>
        /// Gets or sets the card account number.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card CVN.
        /// </summary>
        public string CardCvn { get; set; }

        /// <summary>
        /// Gets or sets the card expiration month.
        /// </summary>
        public int CardExpirationMonth { get; set; }

        /// <summary>
        /// Gets or sets the card expiration year.
        /// </summary>
        public int CardExpirationYear { get; set; }

        /// <summary>
        /// Gets or sets the card billing address.
        /// </summary>
        public PersonAdapter CardBillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the physical address.
        /// </summary>
        public PersonAdapter PhysicalAddress { get; set; }

        /// <summary>
        /// Gets or sets the matched physical address.
        /// </summary>
        public Address MatchedPhysicalAddress { get; set; }

        /// <summary>
        /// The validated address as a person adapter.
        /// </summary>
        public PersonAdapter MatchedPersonAdapter
        {
            get
            {
                if (MatchedPhysicalAddress == null)
                {
                    return null;
                }

                return new PersonAdapter()
                {
                    Street1 = MatchedPhysicalAddress.Address1,
                    City = MatchedPhysicalAddress.City,
                    StateProvCode = MatchedPhysicalAddress.State,
                    PostalCode = MatchedPhysicalAddress.PostalCode
                };
            }
        }

        /// <summary>
        /// Gets or sets the customer key.
        /// </summary>
        public string CustomerKey { get; set; }

        /// <summary>
        /// Executes the associate ShipWorks with itself request
        /// </summary>
        public AssociateShipWorksWithItselfResponse Execute()
        {
            if (PhysicalAddress != null)
            {
                // Call AV server
                UspsAddressValidationResults uspsResult = uspsWebClient.ValidateAddress(PhysicalAddress);

                if (!uspsResult.IsSuccessfulMatch)
                {
                    string suggestionMessage = CreateSuggestionMessage(uspsResult.Candidates);
                    string message = $"{EnumHelper.GetDescription(AssociateShipWorksWithItselfResponseType.AddressValidationFailed)}{suggestionMessage}";
                    return new AssociateShipWorksWithItselfResponse(
                       AssociateShipWorksWithItselfResponseType.AddressValidationFailed,
                       message);
                }

                if (uspsResult.IsPoBox ?? false)
                {
                    return new AssociateShipWorksWithItselfResponse(
                        AssociateShipWorksWithItselfResponseType.POBoxNotAllowed,
                        EnumHelper.GetDescription(AssociateShipWorksWithItselfResponseType.POBoxNotAllowed));
                }

                MatchedPhysicalAddress = uspsResult.MatchedAddress;
            }

            return tangoWebClient.AssociateShipworksWithItself(this);
        }

        /// <summary>
        /// Creates a message of the top 3 suggested addresses. Empty string if no addresses.
        /// </summary>
        private string CreateSuggestionMessage(IEnumerable<Address> candidates)
        {
            IEnumerable<Address> addresses = candidates as IList<Address> ?? candidates?.ToList();

            if (addresses == null || !addresses.Any())
            {
                return string.Empty;
            }

            List<string> formattedAddresses = new List<string>();

            // Resharper says candidates might be null, but the check above should
            // return an empty string if it is null. Resharper don't know Roslyn...
            foreach (Address address in addresses.Take(NumberOfSuggestionsToShow))
            {
                List<string> addressElements = new List<string>
                {
                    address.Address1,
                    address.Address2,
                    address.Address3,
                    address.City,
                    address.State,
                    address.ZIPCode
                };

                string formattedAddress = string.Join(", ", addressElements.Where(s => !string.IsNullOrWhiteSpace(s)).ToList());

                formattedAddresses.Add(formattedAddress);
            }

            string addressesSeparatedWithNewLine = string.Join(Environment.NewLine, formattedAddresses);

            return $" A few examples are below.{Environment.NewLine}{Environment.NewLine}" +
                   $"{addressesSeparatedWithNewLine}";
        }
    }
}
