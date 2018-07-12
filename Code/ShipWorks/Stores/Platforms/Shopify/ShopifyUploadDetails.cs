using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Details for uploading Shopify order details
    /// </summary>
    public class ShopifyUploadDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyUploadDetails(long shopifyOrderID, string trackingNumber, string carrier, string carrierTrackingUrl, long locationID)
        {
            ShopifyOrderID = shopifyOrderID;
            TrackingNumber = trackingNumber;
            Carrier = carrier;
            CarrierTrackingUrl = carrierTrackingUrl;
            LocationID = locationID;
            ItemIDs = Enumerable.Empty<long>();
        }

        /// <summary>
        /// Order id
        /// </summary>
        public long ShopifyOrderID { get; }

        /// <summary>
        /// Tracking number
        /// </summary>
        public string TrackingNumber { get; }

        /// <summary>
        /// Carrier name
        /// </summary>
        public string Carrier { get; }

        /// <summary>
        /// Url for carrier tracking information
        /// </summary>
        public string CarrierTrackingUrl { get; }

        /// <summary>
        /// ID of the location from which this order is being fulfilled
        /// </summary>
        public long LocationID { get; }

        /// <summary>
        /// Item ids associated with this fulfillment
        /// </summary>
        public IEnumerable<long> ItemIDs { get; }
    }
}