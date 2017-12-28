using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Net;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Carriers.Postal;
using System;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Address validator that uses Stamps.com for lookups
    /// </summary>
    public class StampsAddressValidationWebClient : IAddressValidationWebClient
    {
        private readonly IUspsWebClient uspsWebClient;
        private readonly IAddressValidationResultFactory addressValidationResultFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAddressValidationWebClient()
			: this(new UspsWebClient(new UspsAccountRepository(),
	            new UspsWebServiceFactory(new LogEntryFactory()),
    	        new CertificateInspector(TangoCredentialStore.Instance.UspsCertificateVerificationData),
        	    UspsResellerType.None), new StampsAddressValidationResultFactory())
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAddressValidationWebClient(IUspsWebClient uspsWebClient, IAddressValidationResultFactory addressValidationResultFactory)
        {
            this.uspsWebClient = uspsWebClient;
            this.addressValidationResultFactory = addressValidationResultFactory;
        }

        /// <summary>
        /// Validate the address
        /// </summary>
        public async Task<AddressValidationWebClientValidateAddressResult> ValidateAddressAsync(AddressAdapter addressAdapter)
        {
            // The underlying web client expects a PersonAdapter, so convert the address adapter to a person adapter
            PersonAdapter personAdapter = new PersonAdapter()
            {
                FirstName = "Sample",
                LastName = "Name",
                Street1 = addressAdapter.Street1,
                Street2 = addressAdapter.Street2,
                City = addressAdapter.City,
                StateProvCode = addressAdapter.StateProvCode,
                PostalCode = addressAdapter.PostalCode,
                CountryCode = addressAdapter.CountryCode
            };

            AddressValidationWebClientValidateAddressResult validationResult = new AddressValidationWebClientValidateAddressResult();

            UspsCounterRateAccountRepository accountRepo = new UspsCounterRateAccountRepository(TangoCredentialStore.Instance);
            try
            {
                UspsAddressValidationResults uspsResult = await uspsWebClient.ValidateAddressAsync(personAdapter, accountRepo.DefaultProfileAccount).ConfigureAwait(false);
                validationResult.AddressType = ConvertAddressType(uspsResult, addressAdapter);

                if (uspsResult.IsSuccessfulMatch)
                {
                    validationResult.AddressValidationResults.Add(addressValidationResultFactory.CreateAddressValidationResult(uspsResult.MatchedAddress, true, uspsResult, (int) validationResult.AddressType));
                    
                    if (validationResult.AddressType == AddressType.InternationalAmbiguous)
                    {
                        validationResult.AddressValidationError = TranslateValidationResultMessage(uspsResult);
                    }
                }
                else
                {
                    validationResult.AddressValidationError = uspsResult.BadAddressMessage;
                }

                foreach (Address address in uspsResult.Candidates)
                {
                    validationResult.AddressValidationResults.Add(addressValidationResultFactory.CreateAddressValidationResult(address, false, uspsResult, (int) validationResult.AddressType));
                }
            }
            catch (UspsException ex)
            {
                validationResult.AddressValidationError = ex.Message;
            }

            return validationResult;
        }

        /// <summary>
        /// Translate the error from stamps to a ShipWorks customer friendly error
        /// </summary>
        private string TranslateValidationResultMessage(UspsAddressValidationResults uspsResult)
        {
            string originalMessage = uspsResult?.AddressCleansingResult ?? string.Empty;

            if (originalMessage == "Province and Postal Code are valid, but City and Street could not be verified.")
            {
                return "The address has been verified to the State level, which is the highest level possible for the destination country.";
            }

            if (originalMessage == "City, Province, and Postal Code are valid, but the Street could not be verified.")
            {
                return "The address has been verified to the City level, which is the highest level possible for the destination country.";
            }

            if (originalMessage == "Street, City, Province, and Postal Code are valid, but the Street Number could not be verified.")
            {
                return "The address has been verified to the Street level, which is the highest level possible for the destination country.";
            }

            return originalMessage;
        }

        /// <summary>
        /// Analyzes the uspsResult and returns the appropriate AddressType
        /// </summary>
        private static AddressType ConvertAddressType(UspsAddressValidationResults uspsResult, AddressAdapter addressAdapter)
        {
            if (!uspsResult.IsCityStateZipOk)
            {
                return AddressType.Invalid;
            }
            
            if (IsSecondaryAddressProblem(uspsResult))
            {
                return AddressType.SecondaryNotFound;
            }

            if (!uspsResult.IsSuccessfulMatch)
            {
                return AddressType.PrimaryNotFound;
            }

            // successful match!
            if (IsMilitary(uspsResult))
            {
                return AddressType.Military;
            }
            
            if (IsUsTerritory(uspsResult))
            {
                return AddressType.UsTerritory;
            }

            if (uspsResult.IsPoBox ?? false)
            {
                return AddressType.PoBox;
            }

            switch (uspsResult.ResidentialIndicator)
            {
                case ResidentialDeliveryIndicatorType.Yes:
                    return AddressType.Residential;
                case ResidentialDeliveryIndicatorType.No:
                    return AddressType.Commercial;
                default:
                    if (addressAdapter.IsDomesticCountry())
                    {
                        return AddressType.Valid;
                    }
                    return DetermineInternationalCorectness(uspsResult);
            }
        }

        /// <summary>
        /// Does the Address Validation Result have a secondary address problem 
        /// </summary>
        private static bool IsSecondaryAddressProblem(UspsAddressValidationResults uspsResult)
        {
            return uspsResult.StatusCodes?.Footnotes?.Any(x => (x.Value ?? string.Empty) == "H" || (x.Value ?? string.Empty) == "S") ?? false;
        }

        /// <summary>
        /// Is the Address Validation Result for a US Territory
        /// </summary>
        private static bool IsUsTerritory(UspsAddressValidationResults uspsResult)
        {
            return CountryList.IsUSInternationalTerritory(uspsResult.MatchedAddress?.State ?? string.Empty);
        }

        /// <summary>
        /// Is the Address Validation Result for a military address
        /// </summary>
        private static bool IsMilitary(UspsAddressValidationResults uspsResult)
        {
            return uspsResult.StatusCodes?.Footnotes?.Any(x => (x.Value ?? string.Empty) == "Y") ?? false;
        }

        /// <summary>
        /// Check to see if an international address has been verified but still ambiguous
        /// </summary>
        private static AddressType DetermineInternationalCorectness(UspsAddressValidationResults uspsResult)
        {
            if (uspsResult.VerificationLevel == AddressVerificationLevel.Maximum && 
                !string.IsNullOrWhiteSpace(uspsResult.AddressCleansingResult) &&
                uspsResult.AddressCleansingResult != "Full Address Verified.")
            {
                return AddressType.InternationalAmbiguous;
            }

            return AddressType.Valid;
        }
    }
}
