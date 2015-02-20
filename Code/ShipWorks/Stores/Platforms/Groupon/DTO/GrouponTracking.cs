using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Groupon.DTO
{
    class GrouponTracking
    {
        public GrouponTracking(string Carrier, Int64 CILineItemID, string Tracking)
        {
            this.carrier = Carrier;
            this.ci_lineitem_id = CILineItemID;
            this.tracking = Tracking;

        }

        public string carrier { get; set; }
        public Int64 ci_lineitem_id { get; set; }
        public string tracking { get; set; }

    }
}
