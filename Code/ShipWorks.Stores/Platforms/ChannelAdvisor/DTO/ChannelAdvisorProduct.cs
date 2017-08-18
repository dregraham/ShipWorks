using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Product entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorProduct
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("ProfileID")]
        public int ProfileID { get; set; }

        [JsonProperty("CreateDateUtc")]
        public DateTime CreateDateUtc { get; set; }

        [JsonProperty("UpdateDateUtc")]
        public DateTime UpdateDateUtc { get; set; }

        [JsonProperty("QuantityUpdateDateUtc")]
        public DateTime QuantityUpdateDateUtc { get; set; }

        [JsonProperty("IsAvailableInStore")]
        public bool IsAvailableInStore { get; set; }

        [JsonProperty("IsBlocked")]
        public bool IsBlocked { get; set; }

        [JsonProperty("IsExternalQuantityBlocked")]
        public bool IsExternalQuantityBlocked { get; set; }

        [JsonProperty("BlockComment")]
        public object BlockComment { get; set; }

        [JsonProperty("BlockedDateUtc")]
        public object BlockedDateUtc { get; set; }

        [JsonProperty("ReceivedDateUtc")]
        public object ReceivedDateUtc { get; set; }

        [JsonProperty("LastSaleDateUtc")]
        public object LastSaleDateUtc { get; set; }

        [JsonProperty("ASIN")]
        public string ASIN { get; set; }

        [JsonProperty("Brand")]
        public string Brand { get; set; }

        [JsonProperty("Condition")]
        public string Condition { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("EAN")]
        public string EAN { get; set; }

        [JsonProperty("FlagDescription")]
        public string FlagDescription { get; set; }

        [JsonProperty("Flag")]
        public string Flag { get; set; }

        [JsonProperty("HarmonizedCode")]
        public string HarmonizedCode { get; set; }

        [JsonProperty("ISBN")]
        public string ISBN { get; set; }

        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("MPN")]
        public string MPN { get; set; }

        [JsonProperty("ShortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("Sku")]
        public string Sku { get; set; }

        [JsonProperty("Subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("TaxProductCode")]
        public string TaxProductCode { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("UPC")]
        public string UPC { get; set; }

        [JsonProperty("WarehouseLocation")]
        public string WarehouseLocation { get; set; }

        [JsonProperty("Warranty")]
        public string Warranty { get; set; }

        [JsonProperty("Height")]
        public decimal Height { get; set; }

        [JsonProperty("Length")]
        public decimal Length { get; set; }

        [JsonProperty("Width")]
        public decimal Width { get; set; }

        [JsonProperty("Weight")]
        public decimal Weight { get; set; }

        [JsonProperty("Cost")]
        public decimal Cost { get; set; }

        [JsonProperty("Margin")]
        public decimal Margin { get; set; }

        [JsonProperty("RetailPrice")]
        public decimal RetailPrice { get; set; }

        [JsonProperty("StartingPrice")]
        public decimal StartingPrice { get; set; }

        [JsonProperty("ReservePrice")]
        public decimal ReservePrice { get; set; }

        [JsonProperty("BuyItNowPrice")]
        public double BuyItNowPrice { get; set; }

        [JsonProperty("StorePrice")]
        public double StorePrice { get; set; }

        [JsonProperty("SecondChancePrice")]
        public decimal SecondChancePrice { get; set; }

        [JsonProperty("SupplierName")]
        public string SupplierName { get; set; }

        [JsonProperty("SupplierCode")]
        public string SupplierCode { get; set; }

        [JsonProperty("SupplierPO")]
        public string SupplierPO { get; set; }

        [JsonProperty("Classification")]
        public string Classification { get; set; }

        [JsonProperty("IsDisplayInStore")]
        public bool IsDisplayInStore { get; set; }

        [JsonProperty("StoreTitle")]
        public object StoreTitle { get; set; }

        [JsonProperty("StoreDescription")]
        public object StoreDescription { get; set; }

        [JsonProperty("BundleType")]
        public string BundleType { get; set; }

        [JsonProperty("TotalAvailableQuantity")]
        public int TotalAvailableQuantity { get; set; }

        [JsonProperty("OpenAllocatedQuantity")]
        public int OpenAllocatedQuantity { get; set; }

        [JsonProperty("OpenAllocatedQuantityPooled")]
        public int OpenAllocatedQuantityPooled { get; set; }

        [JsonProperty("PendingCheckoutQuantity")]
        public int PendingCheckoutQuantity { get; set; }

        [JsonProperty("PendingCheckoutQuantityPooled")]
        public int PendingCheckoutQuantityPooled { get; set; }

        [JsonProperty("PendingPaymentQuantity")]
        public int PendingPaymentQuantity { get; set; }

        [JsonProperty("PendingPaymentQuantityPooled")]
        public int PendingPaymentQuantityPooled { get; set; }

        [JsonProperty("PendingShipmentQuantity")]
        public int PendingShipmentQuantity { get; set; }

        [JsonProperty("PendingShipmentQuantityPooled")]
        public int PendingShipmentQuantityPooled { get; set; }

        [JsonProperty("TotalQuantity")]
        public int TotalQuantity { get; set; }

        [JsonProperty("TotalQuantityPooled")]
        public int TotalQuantityPooled { get; set; }

        [JsonProperty("IsParent")]
        public bool IsParent { get; set; }

        [JsonProperty("IsInRelationship")]
        public bool IsInRelationship { get; set; }

        [JsonProperty("ParentProductID")]
        public object ParentProductID { get; set; }

        [JsonProperty("RelationshipName")]
        public object RelationshipName { get; set; }

        [JsonProperty("Attributes")]
        public IEnumerable<ChannelAdvisorProductAttribute> Attributes { get; set; }

        [JsonProperty("Images")]
        public IEnumerable<ChannelAdvisorProductImage> Images { get; set; }

        [JsonProperty("DCQuantities")]
        public IEnumerable<ChannelAdvisorProductDistributionCenterQuantity> DistributionCenterQuantities { get; set; }
    }
}