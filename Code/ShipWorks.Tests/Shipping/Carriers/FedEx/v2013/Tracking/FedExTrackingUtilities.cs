﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Track;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Tracking
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
