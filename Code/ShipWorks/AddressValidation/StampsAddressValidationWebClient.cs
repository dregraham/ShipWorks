﻿using System.Linq;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAddressValidationWebClient()
			: this(new UspsWebClient(new UspsAccountRepository(),
	            new UspsWebServiceFactory(new LogEntryFactory()),
    	        new CertificateInspector(TangoCredentialStore.Instance.UspsCertificateVerificationData),
        	    UspsResellerType.None))
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsAddressValidationWebClient(IUspsWebClient uspsWebClient)
        {
            this.uspsWebClient = uspsWebClient;
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
                    validationResult.AddressValidationResults.Add(CreateAddressValidationResult(uspsResult.MatchedAddress, true, uspsResult));
                    
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
                    validationResult.AddressValidationResults.Add(CreateAddressValidationResult(address, false, uspsResult));
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
        /// Create an AddressValidationResult from a Stamps.com address
        /// </summary>
        private static AddressValidationResult CreateAddressValidationResult(Address address, bool isValid, UspsAddressValidationResults uspsResult)
        {
            AddressValidationResult addressValidationResult = new AddressValidationResult
            {
                Street1 = address.Address1 ?? string.Empty,
                Street2 = address.Address2 ?? string.Empty,
                Street3 = address.Address3 ?? string.Empty,
                City = address.City ?? string.Empty,
                StateProvCode = address.State ?? address.Province ?? string.Empty,
                PostalCode = GetPostalCode(address) ?? string.Empty,
                CountryCode = address.Country ?? string.Empty,
                IsValid = isValid,
                POBox = ConvertPoBox(uspsResult.IsPoBox),
                ResidentialStatus = ConvertResidentialStatus(uspsResult.ResidentialIndicator)
            };

            addressValidationResult.ParseStreet1();
            addressValidationResult.ApplyAddressCasing();

            return addressValidationResult;
        }

        /// <summary>
        /// Analyzes the uspsResult and returns the appropriate AddressType
        /// </summary>
        private static AddressType ConvertAddressType(UspsAddressValidationResults uspsResult, AddressAdapter addressAdapter)
        {
            bool isMilitary = uspsResult.StatusCodes?.Footnotes?.Any(x => (x.Value ?? string.Empty) == "Y") ?? false;
            bool isSecondaryAddressProblem = uspsResult.StatusCodes?.Footnotes?.Any(x => (x.Value ?? string.Empty) == "H" || (x.Value ?? string.Empty) == "S") ?? false;
            bool isUsTerritory = CountryList.IsUSInternationalTerritory(uspsResult.MatchedAddress?.State ?? string.Empty);

            if (!uspsResult.IsCityStateZipOk)
            {
                return AddressType.Invalid;
            }

            if (isSecondaryAddressProblem)
            {
                return AddressType.SecondaryNotFound;
            }

            if (!uspsResult.IsSuccessfulMatch)
            {
                return AddressType.PrimaryNotFound;
            }

            // successful match!

            if (isMilitary)
            {
                return AddressType.Military;
            }

            if (isUsTerritory)
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

        /// <summary>
        /// Converts the po box indicator into a ShipWorks ValidationDetailStatus
        /// </summary>
        private static ValidationDetailStatusType ConvertPoBox(bool? isPoBox)
        {
            if (!isPoBox.HasValue)
            {
                return ValidationDetailStatusType.Unknown;
            }

            return isPoBox.Value ? ValidationDetailStatusType.Yes : ValidationDetailStatusType.No;
        }

        /// <summary>
        /// Convert Stamps.com residential status into ShipWorks residential status
        /// </summary>
        private static ValidationDetailStatusType ConvertResidentialStatus(ResidentialDeliveryIndicatorType residentialStatus)
        {
            switch (residentialStatus)
            {
                case ResidentialDeliveryIndicatorType.No:
                    return ValidationDetailStatusType.No;
                case ResidentialDeliveryIndicatorType.Yes:
                    return ValidationDetailStatusType.Yes;
                default:
                    return ValidationDetailStatusType.Unknown;
            }
        }

        /// <summary>
        /// Get a full postal code from an address
        /// </summary>
        private static string GetPostalCode(Address address)
        {
            if (!string.IsNullOrEmpty(address.PostalCode))
            {
                return address.PostalCode;
            }

            if (!string.IsNullOrEmpty(address.ZIPCodeAddOn) && address.ZIPCodeAddOn != "0000")
            {
                return address.ZIPCode + "-" + address.ZIPCodeAddOn;
            }

            return address.ZIPCode;
        }
    }
}
