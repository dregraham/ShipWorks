using System;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using ShipWorks.Shipping.Tracking;
using log4net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response.Manipulators
{
    /// <summary>
    /// Response manipulator responsible for transforming the TrackReply into ShipWorks tracking data.
    /// </summary>
    public class FedExTrackingResponseManipulator : IFedExTrackingResponseManipulator
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FedExTrackingResponseManipulator));
        private ShipmentEntity shipment;

        /// <summary>
        /// Manipulates a FedExShipmentEntity based on the data in the carrierResponse
        /// </summary>
        /// <param name="carrierResponse">The carrier response.</param>
        public void Manipulate(ICarrierResponse carrierResponse)
        {
            TrackReply reply = carrierResponse.NativeResponse as TrackReply;
            TrackingResult result = new TrackingResult();
            FedExTrackingResponse trackingResponse = carrierResponse as FedExTrackingResponse;
            shipment = trackingResponse.Shipment;

            if (reply.HighestSeverity == NotificationSeverityType.ERROR || reply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiCarrierException(reply.Notifications);
            }

            if (reply.CompletedTrackDetails == null || reply.CompletedTrackDetails.Length == 0 || reply.CompletedTrackDetails[0].TrackDetails == null || reply.CompletedTrackDetails[0].TrackDetails.Length == 0)
            {
                result.Summary = "Unknown";
                trackingResponse.TrackingResult = result;
                return;
            }

            // Hardcode to use the first one
            CompletedTrackDetail detail = reply.CompletedTrackDetails[0];

            if (reply.CompletedTrackDetails.Length > 1)
            {
                log.InfoFormat("Ignoring additional TrackDetails records (Total of {0})", reply.CompletedTrackDetails.Length);
            }

            TrackDetail trackDetail = detail.TrackDetails[0];

            result.Summary = GetTrackingSummary(trackDetail);

            if (trackDetail.Events != null)
            {
                foreach (TrackEvent trackEvent in trackDetail.Events)
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

            trackingResponse.TrackingResult = result;
        }

        /// <summary>
        /// Get the tracking summary for the given detail
        /// </summary>
        private static string GetTrackingSummary(TrackDetail detail)
        {
            if (detail.StatusDetail == null || string.IsNullOrWhiteSpace(detail.StatusDetail.Description))
            {
                return "No tracking information was returned.";
            }

            string status = string.Format("<b>{0}</b>", detail.StatusDetail.Description);

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
        private string GetTrackEventLocation(TrackEvent trackEvent)
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

            if (!new FedExShipmentType().IsDomestic(shipment))
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
