using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Amazon.OnlineUpdating.DTO
{
    public class AmazonBulkUploadShipmentsRequest
    {
        public string MarketplaceId { get; set; }

        public string OrderSourceId { get; set; }

        public List<AmazonUploadShipment> Shipments { get; set; }
    }

    public class AmazonUploadShipment
    {
        public string AmazonOrderId { get; set; }

        public DateTime ShipDate { get; set; }

        public string CarrierCode { get; set; }

        public string CarrierName { get; set; }

        public string Service { get; set; }

        public string TrackingNumber { get; set; }
    }
}
