using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.AddressValidation;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using System.Web.Services.Protocols;
using System.Net;
using ShipWorks.Data;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// API entry point for the address validation service
    /// </summary>
    public static class FedExApiAddressValidation
    {        
        /// <summary>
        /// Create the webserivce instance with the appropriate URL
        /// </summary>
        private static AddressValidationService CreateWebService(string logName)
        {
            AddressValidationService webService = new AddressValidationService(new ApiLogEntry(ApiLogSource.FedEx, logName));
            webService.Url = FedExApiCore.ServerUrl;

            return webService;
        }

        /// <summary>
        /// Determines if the given shipment is being shipped to a residential address
        /// </summary>
        public static bool IsResidentialAddress(ShipmentEntity shipment)
        {
            FedExShipmentEntity fedex = shipment.FedEx;

            FedExAccountEntity account = FedExAccountManager.GetAccount(fedex.FedExAccountID);
            if (account == null)
            {
                throw new FedExException("No FedEx account is selected for the shipment.");
            }

            AddressValidationRequest request = new AddressValidationRequest();

            // Authentication
            request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "aval",
                Major = 2,
                Intermediate = 0,
                Minor = 0
            };

            // Set the single address that we want to validate
            request.AddressesToValidate = new AddressToValidate[] 
            { 
                new AddressToValidate()
                {
                    Address = FedExApiCore.CreateAddress<Address>(new PersonAdapter(shipment, "Ship")),
                    CompanyName = shipment.ShipCompany.Length > 0 ? shipment.ShipCompany : null
                }
            };

            // We just want to check residential status
            request.Options = new AddressValidationOptions();
            request.Options.CheckResidentialStatus = true;
            request.Options.CheckResidentialStatusSpecified = true;

            try
            {
                using (AddressValidationService webService = CreateWebService("ResidentialCheck"))
                {
                    AddressValidationReply reply = webService.addressValidation(request);

                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    if (reply.AddressResults == null || reply.AddressResults.Length == 0)
                    {
                        throw new FedExException("FedEx returned zero results for residential check.");
                    }

                    if (reply.AddressResults[0] == null || reply.AddressResults[0].ProposedAddressDetails.Length == 0)
                    {
                        throw new FedExException("FedEx returned zero details for residential check.");
                    }

                    ProposedAddressDetail detail = reply.AddressResults[0].ProposedAddressDetails[0];

                    if (!detail.ResidentialStatusSpecified)
                    {
                        throw new FedExException("FedEx did not determine if the address is commercial or residential.");
                    }

                    if (detail.ResidentialStatus == ResidentialStatusType.NOT_APPLICABLE_TO_COUNTRY)
                    {
                        throw new FedExException("Residential status is not available for the destination country.");
                    }

                    if (detail.ResidentialStatus == ResidentialStatusType.INSUFFICIENT_DATA ||
                        detail.ResidentialStatus == ResidentialStatusType.UNAVAILABLE ||
                        detail.ResidentialStatus == ResidentialStatusType.UNDETERMINED)
                    {
                        throw new FedExException("FedEx was unable to determine if the address is commercial or residential.");
                    }

                    return detail.ResidentialStatus == ResidentialStatusType.RESIDENTIAL;
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
        }
    }
}
