using System;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Tracking
{
    public class FedExTrackingUtilities
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
                TrackDetails = new TrackDetail[1]
                    {
                        new TrackDetail
                            {
                                TrackingNumber =  "999999999999999",
                                Events = new TrackEvent[1]
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
            };

            return trackReply;
        }
    }
}
