﻿using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Tracking;
using System.Linq;

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
            trackingResult.Summary = shipEngineTrackingInfo.StatusDescription;

            if (shipEngineTrackingInfo.Events != null)
            {
                foreach (DTOs.TrackEvent trackEvent in shipEngineTrackingInfo.Events)
                {
                    TrackingResultDetail trackingResultDetail = new TrackingResultDetail();
                    trackingResultDetail.Date = trackEvent.OccurredAt?.ToString("M/dd/yyy") ?? "";
                    trackingResultDetail.Time = trackEvent.OccurredAt?.ToString("h:mm tt") ?? "";
                    trackingResultDetail.Location = CreateTrackingLocation(trackEvent);
                    trackingResultDetail.Activity = trackEvent.Description;

                    trackingResult.Details.Add(trackingResultDetail);
                }
            }

            return trackingResult;
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
