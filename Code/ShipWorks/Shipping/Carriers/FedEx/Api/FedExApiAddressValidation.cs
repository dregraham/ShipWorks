using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.AddressValidation;
using ShipWorks.Shipping.Settings;
using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// API entry point for the address validation service
    /// </summary>
    public static class FedExApiAddressValidation
    {
        private const string testingUrl = "https://wsbeta.fedex.com:443/web-services/";
        private const string liveUrl = "https://gateway.fedex.com/web-services";

        private static FedExSettings fedExSettings = new FedExSettings();

        /// <summary>
        /// Get the FedEx server URL to use
        /// </summary>
        private static string ServerUrl
        {
            get { return new FedExSettingsRepository().UseTestServer ? testingUrl : liveUrl; }
        }

        /// <summary>
        /// Create the web service instance with the appropriate URL
        /// </summary>
        private static AddressValidationService CreateWebService(string logName)
        {
            AddressValidationService webService = new AddressValidationService(new ApiLogEntry(ApiLogSource.FedEx, logName));
            webService.Url = ServerUrl;

            return webService;
        }

        /// <summary>
        /// Determines if the given shipment is being shipped to a residential address
        /// </summary>
        public static bool IsResidentialAddress(ShipmentEntity shipment)
        {
            AddressValidationRequest request = CreateRequest(shipment);

            AddressValidationReply reply = MakeRequest(request);

            return ProcessResponse(reply);
        }

        /// <summary>
        /// Processes the FedEx Address Validation response.
        /// </summary>
        /// <param name="reply">The reply.</param>
        /// <returns></returns>
        /// <exception cref="FedExApiException"></exception>
        /// <exception cref="FedExException">
        /// FedEx returned zero results for residential check.
        /// or
        /// FedEx returned zero details for residential check.
        /// or
        /// FedEx did not determine if the address is commercial or residential.
        /// or
        /// FedEx was unable to determine if the address is commercial or residential.
        /// or
        /// FedEx was unable to determine if the address is commercial or residential.
        /// </exception>
        private static bool ProcessResponse(AddressValidationReply reply)
        {
            if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiException(reply.Notifications);
            }

            if (reply.AddressResults == null || reply.AddressResults.Length == 0)
            {
                throw new FedExException("FedEx returned zero results for residential check.");
            }

            if (reply.AddressResults[0] == null)
            {
                throw new FedExException("FedEx returned zero details for residential check.");
            }

            AddressValidationResult detail = reply.AddressResults[0];

            if (!detail.ClassificationSpecified)
            {
                throw new FedExException("FedEx did not determine if the address is commercial or residential.");
            }

            switch (detail.Classification)
            {
                case FedExAddressClassificationType.BUSINESS:
                    return false;

                case FedExAddressClassificationType.RESIDENTIAL:
                    return true;

                case FedExAddressClassificationType.UNKNOWN:
                    if (detail.Attributes.None(a => a.Name == "POBox" && a.Value == "true"))
                    {
                        throw new FedExException("FedEx was unable to determine if the address is commercial or residential.");
                    }
                    return false; // It is a POBox

                default:
                    throw new FedExException("FedEx was unable to determine if the address is commercial or residential.");
            }
        }

        /// <summary>
        /// Makes the request.
        /// </summary>
        private static AddressValidationReply MakeRequest(AddressValidationRequest request)
        {
            AddressValidationReply reply;
            try
            {
                using (AddressValidationService webService = CreateWebService("ResidentialCheck"))
                {
                    reply = webService.addressValidation(request);
                }
            }
            catch (SoapException ex)
            {
                throw new FedExSoapException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
            return reply;
        }

        /// <summary>
        /// Creates the request.
        /// </summary>
        private static AddressValidationRequest CreateRequest(ShipmentEntity shipment)
        {
            FedExShipmentEntity fedex = shipment.FedEx;

            FedExAccountEntity account = FedExAccountManager.GetAccount(fedex.FedExAccountID);
            if (account == null)
            {
                throw new FedExException("No FedEx account is selected for the shipment.");
            }

            AddressValidationRequest request = new AddressValidationRequest();

            // Authentication
            request.WebAuthenticationDetail = GetWebAuthenticationDetail();

            // Client
            request.ClientDetail = GetClientDetail(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "aval",
                Major = 4,
                Intermediate = 0,
                Minor = 0
            };

            // Set the single address that we want to validate
            request.AddressesToValidate = new[]
            {
                new AddressToValidate
                {
                    Address = CreateAddress(new PersonAdapter(shipment, "Ship")),
                    Contact = new Contact
                    {
                        CompanyName = shipment.ShipCompany.Length > 0 ? shipment.ShipCompany : null
                    }
                }
            };
            return request;
        }

        /// <summary>
        /// Get the common client detail info
        /// </summary>
        private static ClientDetail GetClientDetail(FedExAccountEntity account)
        {
            ClientDetail clientDetail = new ClientDetail
            {
                AccountNumber = account.AccountNumber,
                MeterNumber = account.MeterNumber
            };

            return clientDetail;
        }

        /// <summary>
        /// Get the common WebAuthenticationDetail info
        /// </summary>
        private static WebAuthenticationDetail GetWebAuthenticationDetail()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            WebAuthenticationDetail credential = new WebAuthenticationDetail
            {
                ParentCredential = new WebAuthenticationCredential
                {
                    Key = fedExSettings.CspCredentialKey,
                    Password = fedExSettings.CspCredentialPassword
                },
                UserCredential = new WebAuthenticationCredential
                {
                    Key = settings.FedExUsername,
                    Password = SecureText.Decrypt(settings.FedExPassword, "FedEx")
                }
            };

            return credential;
        }

        /// <summary>
        /// Create a FedEx API address object from the given person
        /// </summary>
        private static Address CreateAddress(PersonAdapter person)
        {
            Address address = new Address();

            List<string> streetLines = new List<string>();
            streetLines.Add(person.Street1);

            if (!string.IsNullOrEmpty(person.Street2))
            {
                streetLines.Add(person.Street2);
            }

            if (!string.IsNullOrEmpty(person.Street3))
            {
                streetLines.Add(person.Street3);
            }

            address.StreetLines = streetLines.ToArray();
            address.City = person.City;
            address.PostalCode = person.PostalCode;
            address.StateOrProvinceCode = person.StateProvCode;
            address.CountryCode = AdjustFedExCountryCode(person.AdjustedCountryCode(ShipmentTypeCode.FedEx));

            return address;
        }

        /// <summary>
        /// Adjust the country code for what FedEx requires expects
        /// </summary>
        private static string AdjustFedExCountryCode(string code)
        {
            // FedEx wants GB
            if (code == "UK")
            {
                code = "GB";
            }

            return code;
        }
    }
}
