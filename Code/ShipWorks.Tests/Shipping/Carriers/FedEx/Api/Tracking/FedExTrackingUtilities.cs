using System;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking
{
    public static class FedExTrackingUtilities
    {
        /// <summary>
        /// Build a successful track reply
        /// </summary>
        public static TrackReply BuildSuccessTrackReply()
        {
            TrackReply trackReply = new TrackReply
            {
                HighestSeverity = NotificationSeverityType.SUCCESS,
                Notifications = new Notification[]
                {
                    new Notification {
                        Code = "0",
                        Severity = NotificationSeverityType.SUCCESS
                    }
                },
                CompletedTrackDetails = new CompletedTrackDetail[]
                {
                    new CompletedTrackDetail
                        {
                            TrackDetails = new TrackDetail[]
                                {
                                    new TrackDetail
                                        {
                                            StatusDetail = new TrackStatusDetail
                                                {
                                                    Description = "SUCCESS"
                                                },
                                            TrackingNumber = "999999999999999",
                                            Events = new TrackEvent[]
                                                {
                                                    new TrackEvent
                                                        {
                                                            Timestamp = DateTime.Now.ToUniversalTime(),
                                                            EventType = "AR",
                                                            EventDescription = "Arrived at FedEx location",
                                                            Address = new Address
                                                                {
                                                                    City = "CHICAGO",
                                                                    StateOrProvinceCode = "IL",
                                                                    PostalCode = "60638",
                                                                    CountryCode = "US",
                                                                    Residential = false
                                                                }
                                                        }
                                                }

                                        }

                                }
                        }
                }
            };

            return trackReply;
        }
    }
}
