using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Details for uploading Shopify order details
    /// </summary>
    public class ShopifyUploadDetails
    {
        /// <summary>
        /// Create upload details based on another instance
        /// </summary>
        /// <param name="copyFrom"></param>
        /// <param name="items"></param>
        private ShopifyUploadDetails(ShopifyUploadDetails copyFrom, long locationID, IEnumerable<IShopifyOrderItemEntity> items)
        {
            ShopifyOrderID = copyFrom.ShopifyOrderID;
            TrackingNumber = copyFrom.TrackingNumber;
            Carrier = copyFrom.Carrier;
            CarrierTrackingUrl = copyFrom.CarrierTrackingUrl;
            LocationID = locationID;
            ItemIDs = items.Select(x => x.ShopifyOrderItemID).ToList();
        }

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

        /// <summary>
        /// Create a copy of these details with the given location and items
        /// </summary>
        public ShopifyUploadDetails WithLocation(long locationId, IEnumerable<IShopifyOrderItemEntity> items) =>
            new ShopifyUploadDetails(this, locationId, items);

        /// <summary>
        /// Get details about this object
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"ShopifyOrderID: {ShopifyOrderID}; TrackingNumber: {TrackingNumber}; Carrier: {Carrier}, CarrierTrackingUrl: {CarrierTrackingUrl}, LocationID: {LocationID}";
    }
}