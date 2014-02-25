﻿using System;
using System.Text;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Tracking;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net.Track
{
    /// <summary>
    /// OnTracTrackingRequest
    /// </summary>
    public class OnTracTrackedShipment : OnTracRequest
    {
        readonly HttpVariableRequestSubmitter onTracHttpRequestSubmitter;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracTrackedShipment(OnTracAccountEntity onTracAccount, HttpVariableRequestSubmitter onTracHttpRequestSubmitter)
            : base(onTracAccount, "OnTracTrackingRequest")
        {
            this.onTracHttpRequestSubmitter = onTracHttpRequestSubmitter;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracTrackedShipment(
            long onTracAccountNumber, string onTracPassword, HttpVariableRequestSubmitter onTracHttpRequestSubmitter, ILogEntryFactory logEntryFactory)
            : base(onTracAccountNumber, onTracPassword, logEntryFactory, ApiLogSource.OnTrac, "OnTracTrackingRequest", LogActionType.Other)
        {
            this.onTracHttpRequestSubmitter = onTracHttpRequestSubmitter;
        }

        /// <summary>
        /// Given a tracking number use the onTracShipmentRequest to get the tracking information 
        /// and transform it into a TrackingResult
        /// </summary>
        public TrackingResult GetTrackingResults(string trackingNumber)
        {
            TrackingShipment trackedShipment = GetTrackingFromOnTrac(trackingNumber);
            
            // This is the most recent event. Events are sorted in date descending
            Event latestTrackedEvent = trackedShipment.Events[0];

            TrackingResult trackingResult = new TrackingResult
            {
                Summary = GetSummary(trackedShipment, latestTrackedEvent)
            };

            foreach (Event trackedEvent in trackedShipment.Events)
            {
                var detail = new TrackingResultDetail();
                trackingResult.Details.Add(detail);

                detail.Activity = trackedEvent.Description;
                detail.Location = String.Format(
                    "{0}, {1} {2}",
                    trackedEvent.City,
                    trackedEvent.State,
                    trackedEvent.Zip);

                detail.Date = trackedEvent.EventTime.ToString("M/dd/yyy");
                detail.Time = trackedEvent.EventTime.ToString("h:mm tt");
            }

            return trackingResult;
        }

        /// <summary>
        /// Get Summary tracking informaiton.
        /// </summary>
        private static string GetSummary(TrackingShipment trackedShipment, Event latestTrackedEvent)
        {
            StringBuilder summary = new StringBuilder();

            summary.AppendFormat("<b>{0}</b>", latestTrackedEvent.Description);

            //If delivered, we show the delivered date. Else we display the projected delivery date.
            if (trackedShipment.Delivered)
            {
                summary.AppendFormat(" on {0} ", latestTrackedEvent.EventTime.ToString("M/dd/yyy h:mm tt"));
            }
            else
            {
                summary.AppendFormat(
                    "<br/><span style='color: rgb(80, 80, 80);'>Should arrive: {0}</span>",
                    trackedShipment.Exp_Del_Date.ToString("M/dd/yyy h:mm tt"));
            }

            // POD (Proof Of Deliver) is the name of the person who signed for the package
            if (!String.IsNullOrEmpty(trackedShipment.POD))
            {
                summary.AppendFormat(
                    "<br/><span style='color: rgb(80, 80, 80);'>Signed by: {0}</span>", trackedShipment.POD);
            }

            return summary.ToString();
        }

        /// <summary>
        /// Get tracking number from OnTrac
        /// </summary>
        /// <returns> Tracking information using OnTrac XSD DTO </returns>
        private TrackingShipment GetTrackingFromOnTrac(string trackingNumber)
        {
            string url = string.Format(
                "{0}{1}/shipments?pw={2}&tn={3}&requestType=track",
                BaseUrlUsedToCallOnTrac,
                AccountNumber,
                OnTracPassword,
                trackingNumber);

            onTracHttpRequestSubmitter.Uri = new Uri(url);
            onTracHttpRequestSubmitter.Verb = HttpVerb.Get;

            TrackingShipmentList trackingShipmentList = ExecuteLoggedRequest<TrackingShipmentList>(onTracHttpRequestSubmitter);

            if (trackingShipmentList.Shipments == null || trackingShipmentList.Shipments.Length == 0)
            {
                throw new OnTracException("OnTrac did not return any tracking information for the shipment.");
            }

            // We only ever request one shipment
            TrackingShipment trackedShipment = trackingShipmentList.Shipments[0];

            if (trackedShipment.Events == null || trackedShipment.Events.Length == 0)
            {
                throw new OnTracException("OnTrac did not return any tracking information for the shipment.");
            }

            return trackedShipment;
        }
    }
}