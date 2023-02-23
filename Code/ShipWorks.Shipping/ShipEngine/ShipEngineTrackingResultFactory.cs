using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.Tracking;
using System;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory for creating ShipWorks TrackingResult from the ShipEngine TrackingInformation
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipEngineTrackingResultFactory : IShipEngineTrackingResultFactory
    {
        /// <summary>
        /// Creates ShipWorks TrackingResult from the ShipEngine TrackingInformation
        /// </summary>
        public TrackingResult Create(DTOs.TrackingInformation shipEngineTrackingInfo)
        {
            TrackingResult trackingResult = new TrackingResult();
            trackingResult.Summary = GetTrackingSummary(shipEngineTrackingInfo);

            if (shipEngineTrackingInfo.Events != null)
            {
                foreach (DTOs.TrackEvent trackEvent in shipEngineTrackingInfo.Events)
                {
                    TrackingResultDetail trackingResultDetail = new TrackingResultDetail();
                    trackingResultDetail.Date = (trackEvent.CarrierOccurredAt ?? trackEvent.OccurredAt)?.ToString("M/dd/yyy") ?? "";
                    trackingResultDetail.Time = (trackEvent.CarrierOccurredAt ?? trackEvent.OccurredAt)?.ToString("h:mm tt") ?? "";
                    trackingResultDetail.Location = CreateTrackingLocation(trackEvent);
                    trackingResultDetail.Activity = trackEvent.Description;

                    trackingResult.Details.Add(trackingResultDetail);
                }
            }

            return trackingResult;
        }

        /// <summary>
        /// Get tracking summary
        /// </summary>
        private string GetTrackingSummary(TrackingInformation detail)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(detail.StatusDescription))
            {
                return "No tracking information was returned";
            }

            sb.Append($"<b>{detail.StatusDescription}</b>");

            if(detail.ActualDeliveryDate.HasValue)
            {
                sb.Append($" on {detail.ActualDeliveryDate.Value}");
            }

            if(detail.EstimatedDeliveryDate.HasValue)
            {
                sb.Append($"<br/><span style='color: rgb(80, 80, 80);'>Should arrive: {detail.EstimatedDeliveryDate.Value}</span>");
            }

            var signer = detail.Events.FirstOrDefault()?.Signer;
            if (signer.HasValue())
            {
                sb.Append($"<br/><span style='color: rgb(80, 80, 80);'>Signed by: {AddressCasing.Apply(signer)}</span>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Create a tracking location string from the track event location members
        /// </summary>
        private string CreateTrackingLocation(DTOs.TrackEvent trackEvent)
        {
            string[] locationMembers = 
            {
                trackEvent.CityLocality,
                trackEvent.StateProvince,
                $"{trackEvent.CountryCode} {trackEvent.PostalCode}"
            };

            return string.Join(", ", locationMembers.Where(m => !string.IsNullOrWhiteSpace(m)));
        }
    }
}
