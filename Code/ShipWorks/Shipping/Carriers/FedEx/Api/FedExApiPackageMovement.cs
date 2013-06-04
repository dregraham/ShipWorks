using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using System.Web.Services.Protocols;
using System.Net;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Wrapper for calling package movement API
    /// </summary>
    public static class FedExApiPackageMovement
    {        
        /// <summary>
        /// Create the webserivce instance with the appropriate URL
        /// </summary>
        private static PackageMovementInformationService CreateWebService(string logName)
        {
            PackageMovementInformationService webService = new PackageMovementInformationService(new ApiLogEntry(ApiLogSource.FedEx, logName));
            webService.Url = FedExApiCore.ServerUrl;

            return webService;
        }

        /// <summary>
        /// Get the FedEx location ID for the given fedex account
        /// </summary>
        public static string GetLocationID(FedExAccountEntity account)
        {
            PostalCodeInquiryRequest request = new PostalCodeInquiryRequest();

            // Authentication
            request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "pmis",
                Major = 4,
                Intermediate = 0,
                Minor = 0
            };

            request.PostalCode = account.PostalCode;
            request.CountryCode = account.CountryCode;

            try
            {
                using (PackageMovementInformationService webService = CreateWebService("PostalCodeInquiry"))
                {
                    PostalCodeInquiryReply reply = webService.postalCodeInquiry(request);

                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    return reply.ExpressDescription == null ? string.Empty : reply.ExpressDescription.LocationId;
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
