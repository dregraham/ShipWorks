using Interapptive.Shared.Business;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    public class AssociateShipworksWithItselfRequest
    {
        readonly IUspsWebClient uspsWebClient;

        readonly ITangoWebClient tangoWebClient;

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
            if(!ValidatePhysicalAddressIfRequired())
            {
                return new AssociateShipWorksWithItselfResponse(
                    AssociateShipWorksWithItselfResponseType.AddressValidationFailed, 
                    EnumHelper.GetDescription(AssociateShipWorksWithItselfResponseType.AddressValidationFailed));
            }
            return tangoWebClient.AssociateShipworksWithItself(this);
        }

        /// <summary>
        /// If PhysicalAddress set, calls AddressValidation to populate MatchedAddress
        /// </summary>
        private bool ValidatePhysicalAddressIfRequired()
        {
            if (PhysicalAddress == null)
            {
                return true;
            }

            // Call AV server
            UspsAddressValidationResults uspsResult = uspsWebClient.ValidateAddress(PhysicalAddress);
            if (!uspsResult.IsSuccessfulMatch)
            {
                return false;
            }

            MatchedPhysicalAddress = uspsResult.MatchedAddress;
            return true;
        }
    }
}
