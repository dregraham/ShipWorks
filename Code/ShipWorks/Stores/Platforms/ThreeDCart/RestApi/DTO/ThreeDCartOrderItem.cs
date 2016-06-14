using System;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ThreeDCartOrderItem
    {
        public int CatalogID { get; set; }

        public int ItemIndexID { get; set; }

        public string ItemID { get; set; }

        public int ItemShipmentID { get; set; }

        public double ItemQuantity { get; set; }

        public int ItemWarehouseID { get; set; }

        public string ItemDescription { get; set; }

        public decimal ItemUnitPrice { get; set; }

        public double ItemWeight { get; set; }

        public decimal ItemOptionPrice { get; set; }

        public string ItemAdditionalField1 { get; set; }

        public string ItemAdditionalField2 { get; set; }

        public string ItemAdditionalField3 { get; set; }

        public string ItemPageAdded { get; set; }

        public DateTime ItemDateAdded { get; set; }

        public decimal ItemUnitCost { get; set; }

        public int ItemUnitStock { get; set; }

        public string ItemOptions { get; set; }

        public string ItemCatalogIDOptions { get; set; }
    }
}