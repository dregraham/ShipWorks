using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Data structure to hold data returnd from the UPS TimeNTransit API
    /// </summary>
    public class UpsTransitTime
    {
        UpsServiceType service;
        int businessDays;
        DateTime arrivalDate;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsTransitTime(UpsServiceType service, int businessDays, DateTime arrivalDate)
        {
            this.service = service;
            this.businessDays = businessDays;
            this.arrivalDate = arrivalDate;
        }

        /// <summary>
        /// UPS service the transit time represents
        /// </summary>
        public UpsServiceType Service
        {
            get { return service; }
        }

        /// <summary>
        /// Number of business days in transit
        /// </summary>
        public int BusinessDays
        {
            get { return businessDays; }
        }

        /// <summary>
        /// Estimated arrival date for the shipment
        /// </summary>
        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
        }
    }
}
