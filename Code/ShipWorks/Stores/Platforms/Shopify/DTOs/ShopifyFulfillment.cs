﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Details for uploading Shopify order details
    /// </summary>
    [Obfuscation]
    [JsonObject("fulfillment")]
    public class ShopifyFulfillment
    {
        /// <summary>
        /// Create upload details based on another instance
        /// </summary>
        private ShopifyFulfillment(ShopifyFulfillment copyFrom, long locationID)
        {
            TrackingNumber = copyFrom.TrackingNumber;
            Carrier = copyFrom.Carrier;
            CarrierTrackingUrl = copyFrom.CarrierTrackingUrl;
            NotifyCustomer = copyFrom.NotifyCustomer;
            LineItems = copyFrom.LineItems;
            
            LocationID = locationID;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyFulfillment(string trackingNumber, string carrier, string carrierTrackingUrl, long locationID, IShopifyStoreEntity store, IEnumerable<IShopifyOrderItemEntity> items)
        {
            TrackingNumber = trackingNumber;
            Carrier = carrier;
            CarrierTrackingUrl = carrierTrackingUrl;
            LocationID = locationID;
            NotifyCustomer = store.ShopifyNotifyCustomer;
            LineItems = items.Select(x => new ShopifyFulfillmentLineItem(x.ShopifyOrderItemID)).ToList();
        }

        /// <summary>
        /// Tracking number
        /// </summary>
        [JsonProperty("tracking_number")]
        public string TrackingNumber { get; }

        /// <summary>
        /// Carrier name
        /// </summary>
        [JsonProperty("tracking_company")]
        public string Carrier { get; }

        /// <summary>
        /// Url for carrier tracking information
        /// </summary>
        [JsonProperty("tracking_url")]
        public string CarrierTrackingUrl { get; }

        /// <summary>
        /// ID of the location from which this order is being fulfilled
        /// </summary>
        [JsonProperty("location_id")]
        public long LocationID { get; }

        /// <summary>
        /// Should Shopify notify the customer
        /// </summary>
        [JsonProperty("notify_customer")]
        public bool NotifyCustomer { get; }

        /// <summary>
        /// Item ids associated with this fulfillment
        /// </summary>
        [JsonProperty("line_items", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<ShopifyFulfillmentLineItem> LineItems { get; }

        /// <summary>
        /// Create a copy of these details with the given location and items
        /// </summary>
        public ShopifyFulfillment WithLocation(long locationId) =>
            new ShopifyFulfillment(this, locationId);

        /// <summary>
        /// Get details about this object
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            $"TrackingNumber: {TrackingNumber}; Carrier: {Carrier}, CarrierTrackingUrl: {CarrierTrackingUrl}, LocationID: {LocationID}";
    }
}