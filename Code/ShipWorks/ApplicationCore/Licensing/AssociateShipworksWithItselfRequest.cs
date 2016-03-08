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
        IUspsWebClient uspsWebClient;

        ITangoWebClient tangoWebClient;

        public AssociateShipworksWithItselfRequest(IUspsWebClient uspsWebClient, ITangoWebClient tangoWebClient)
        {
            this.uspsWebClient = uspsWebClient;
            this.tangoWebClient = tangoWebClient;
        }

        public string CardHolder { get; set; }

        public CreditCardType CardType { get; set; }

        public string CardAccountNumber { get; set; }

        public string CardCvn { get; set; }

        public int CardExpirationMonth { get; set; }

        public int CardExpirationYear { get; set; }

        public PersonAdapter CardBillingAddress { get; set; }

        public PersonAdapter PhysicalAddress { get; set; }

        public Address MatchedPhysicalAddress { get; set; }

        public string CustomerKey { get; set; }

        public EnumResult<AssociateShipWorksWithItselfResponseType> Execute()
        {
            ValidatePhysicalAddress();
            return tangoWebClient.AssociateShipworksWithItself(this);            
        }

        /// <summary>
        /// If PhysicalAddress set, calls AddressValidation to populate MatchedAddress
        /// </summary>
        private void ValidatePhysicalAddress()
        {
            if (PhysicalAddress == null)
            {
                return;
            }

            // Call AV server
            UspsAddressValidationResults uspsResult = uspsWebClient.ValidateAddress(PhysicalAddress);
            if (!uspsResult.IsSuccessfulMatch)
            {
                throw new AddressValidationException("Cannot validate address.");
            }

            MatchedPhysicalAddress = uspsResult.MatchedAddress;
        }
    }
}
