using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using System.Web.Services.Protocols;
using System.Net;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// API wrapper for the 
    /// </summary>
    public static class FedExApiClose
    {
        /// <summary>
        /// Create the webserivce instance with the appropriate URL
        /// </summary>
        private static CloseService CreateWebService(string logName)
        {
            CloseService webService = new CloseService(new ApiLogEntry(ApiLogSource.FedEx, logName));
            webService.Url = FedExApiCore.ServerUrl;

            return webService;
        }

        /// <summary>
        /// Process end of day ground close for the given account.
        /// </summary>
        public static FedExEndOfDayCloseEntity ProcessGroundClose(FedExAccountEntity account)
        {
            GroundCloseRequest request = new GroundCloseRequest();

            // Authentication
            request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client 
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "clos",
                Major = 2,
                Intermediate = 0,
                Minor = 0
            };

            request.TimeUpToWhichShipmentsAreToBeClosed = DateTime.Now.AddDays(1).Date.Subtract(TimeSpan.FromSeconds(1));
            request.TimeUpToWhichShipmentsAreToBeClosedSpecified = true;

            try
            {
                using (CloseService webService = CreateWebService("GroundClose"))
                {
                    GroundCloseReply reply = webService.groundClose(request);

                    // 904 is code indicating nothing to process
                    if (reply.Notifications != null && reply.Notifications.Any(n => n.Code == "9804"))
                    {
                        return null;
                    }

                    // Check for errors
                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    FedExEndOfDayCloseEntity closeEntity = new FedExEndOfDayCloseEntity();
                    closeEntity.FedExAccountID = account.FedExAccountID;
                    closeEntity.AccountNumber = account.AccountNumber;
                    closeEntity.CloseDate = DateTime.UtcNow;

                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        adapter.SaveAndRefetch(closeEntity);

                        if (reply.CodReport != null)
                        {
                            DataResourceManager.CreateFromBytes(reply.CodReport, closeEntity.FedExEndOfDayCloseID, "COD Report");
                        }

                        if (reply.HazMatCertificate != null)
                        {
                            DataResourceManager.CreateFromBytes(reply.HazMatCertificate, closeEntity.FedExEndOfDayCloseID, "HazMat Report");
                        }

                        if (reply.MultiweightReport != null)
                        {
                            DataResourceManager.CreateFromBytes(reply.MultiweightReport, closeEntity.FedExEndOfDayCloseID, "Multiweight Report");
                        }

                        if (reply.Manifest != null)
                        {
                            DataResourceManager.CreateFromBytes(reply.Manifest.File, closeEntity.FedExEndOfDayCloseID, reply.Manifest.FileName);
                        }

                        adapter.Commit();
                    }

                    return closeEntity;
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

        /// <summary>
        /// Process end of day smartpost close for the given account.
        /// </summary>
        public static bool ProcessSmartPostClose(FedExAccountEntity account)
        {
            SmartPostCloseRequest request = new SmartPostCloseRequest();

            // Authentication
            request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client 
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "clos",
                Major = 2,
                Intermediate = 0,
                Minor = 0
            };

            request.PickUpCarrier = CarrierCodeType.FXSP;
            request.PickUpCarrierSpecified = true;

            try
            {
                using (CloseService webService = CreateWebService("SmartPostClose"))
                {
                    SmartPostCloseReply reply = webService.smartPostClose(request);

                    // 904 is code indicating nothing to process
                    if (reply.Notifications != null && reply.Notifications.Any(n => n.Code == "9804"))
                    {
                        return false;
                    }

                    // Check for errors
                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    return true;
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
