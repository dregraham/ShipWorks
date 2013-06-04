using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Data.Model.EntityClasses;
using System.Web.Services.Protocols;
using System.Net;
using ShipWorks.ApplicationCore;
using log4net;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Wrapper for the FedEx trackign API
    /// </summary>
    public static class FedExApiTrack
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FedExApiTrack));

        static int testNumberIndex = 0;

        /// <summary>
        /// Create the webserivce instance with the appropriate URL
        /// </summary>
        private static TrackService CreateTrackService()
        {
            TrackService webService = new TrackService(new ApiLogEntry(ApiLogSource.FedEx, "Track"));
            webService.Url = FedExApiCore.ServerUrl;

            return webService;
        }

        /// <summary>
        /// Track the given FedEx tracking number
        /// </summary>
        public static TrackingResult TrackShipment(string trackingNumber)
        {
            if (InterapptiveOnly.MagicKeysDown)
            {
                trackingNumber = (testNumberIndex++ % 2) == 0 ? "369844911227763" : "828007940070030";
            }

            TrackRequest request = new TrackRequest();

            FedExAccountEntity account = FedExAccountManager.Accounts.FirstOrDefault();
            if (account == null)
            {
                throw new FedExException("ShipWorks cannot track a FedEx shipment without a configured FedEx account.");
            }
            else if (account.Is2xMigrationPending)
            {
                throw new FedExException("The FedEx account selected for the shipment was migrated from ShipWorks 2, and has not yet been configured for ShipWorks 3.");
            }

            // Authentication
            request.WebAuthenticationDetail = FedExApiCore.GetWebAuthenticationDetail<WebAuthenticationDetail>();

            // Client
            request.ClientDetail = FedExApiCore.GetClientDetail<ClientDetail>(account);

            // Version
            request.Version = new VersionId
            {
                ServiceId = "trck",
                Major = 4,
                Intermediate = 0,
                Minor = 0
            };

            request.IncludeDetailedScans = true;
            request.IncludeDetailedScansSpecified = true;

            request.PackageIdentifier = new TrackPackageIdentifier
             {
                Type = TrackIdentifierType.TRACKING_NUMBER_OR_DOORTAG,
                Value = trackingNumber
             };

            try
            {
                using (TrackService webService = CreateTrackService())
                {
                    TrackReply reply = webService.track(request);
                    TrackingResult result = new TrackingResult();

                    if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
                    {
                        throw new FedExApiException(reply.Notifications);
                    }

                    if (reply.TrackDetails == null || reply.TrackDetails.Length == 0)
                    {
                        result.Summary = "Unknown";
                        return result;
                    }

                    // Hardcode to use the first one
                    TrackDetail detail = reply.TrackDetails[0];

                    if (reply.TrackDetails.Length > 1)
                    {
                        log.InfoFormat("Ignoring additional TrackDetails records (Total of {0})", reply.TrackDetails.Length);
                    }

                    result.Summary = GetTrackingSummary(detail);

                    if (detail.Events != null)
                    {
                        foreach (TrackEvent trackEvent in detail.Events)
                        {
                            TrackingResultDetail resultDetail = new TrackingResultDetail();
                            resultDetail.Activity = GetTrackEventActivity(trackEvent);
                            resultDetail.Location = GetTrackEventLocation(trackEvent);

                            if (trackEvent.TimestampSpecified)
                            {
                                resultDetail.Date = trackEvent.Timestamp.ToString("M/dd/yyy");
                                resultDetail.Time = trackEvent.Timestamp.ToString("h:mm tt");
                            }

                            result.Details.Add(resultDetail);
                        }
                    }

                    return result;
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
        /// Get the tracking summary for the given detail
        /// </summary>
        private static string GetTrackingSummary(TrackDetail detail)
        {
            if (string.IsNullOrWhiteSpace(detail.StatusDescription))
            {
                return "No tracking information was returned.";
            }

            string status = string.Format("<b>{0}</b>", detail.StatusDescription);

            if (detail.ActualDeliveryTimestampSpecified)
            {
                DateTime delivered = detail.ActualDeliveryTimestamp;
                status += string.Format(" on {0:M/dd/yyyy h:mm tt}", delivered);
            }

            if (detail.EstimatedDeliveryTimestampSpecified)
            {
                DateTime estimate = detail.EstimatedDeliveryTimestamp;
                status += string.Format("<br/><span style='color: rgb(80, 80, 80);'>Should arrive: {0:M/dd/yyyy}</span>", estimate);
            }

            if (!string.IsNullOrEmpty(detail.DeliverySignatureName))
            {
                status += string.Format("<br/><span style='color: rgb(80, 80, 80);'>Signed by: {0}</span>", AddressCasing.Apply(detail.DeliverySignatureName));
            }

            return status;
        }

        /// <summary>
        /// Get the activity information for the given track event
        /// </summary>
        private static string GetTrackEventActivity(TrackEvent trackEvent)
        {
            string activity = trackEvent.EventDescription;

            if (!string.IsNullOrEmpty(trackEvent.StatusExceptionDescription))
            {
                activity += "<br/>" + trackEvent.StatusExceptionDescription;
            }

            return activity;
        }

        /// <summary>
        /// Get descriptive location text for the given track event
        /// </summary>
        private static string GetTrackEventLocation(TrackEvent trackEvent)
        {
            string location = AddressCasing.Apply(trackEvent.Address.City) ?? string.Empty;

            if (!string.IsNullOrEmpty(trackEvent.Address.StateOrProvinceCode))
            {
                if (location.Length > 0)
                {
                    location += ", ";
                }

                location += trackEvent.Address.StateOrProvinceCode;
            }

            if (trackEvent.Address.CountryCode != "US")
            {
                if (location.Length > 0)
                {
                    location += ", ";
                }

                location += Geography.GetCountryName(trackEvent.Address.CountryCode);
            }

            return location;
        }
    }
}
