using System.Reflection;
using GalaSoft.MvvmLight;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Represents an item to be scan and packed
    /// </summary>
    public class ScanPackItem : ViewModelBase
    {
        private double quantity;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackItem(long orderItemID, string name, string imageUrl, double quantity, string itemUpc, string itemCode, string productUpc, string sku)
        {
            OrderItemID = orderItemID;
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
            ItemUpc = itemUpc;
            ItemCode = itemCode;
            ProductUpc = productUpc;
            Sku = sku;
        }

        /// <summary>
        /// Order Item ID
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long OrderItemID { get; }

        /// <summary>
        /// The Items Name
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Name { get; set; }

        /// <summary>
        /// The Items Image Url
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// The Items Quantity
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double Quantity
        {
            get => quantity;
            set => Set(ref quantity, value);
        }

        /// <summary>
        /// The Item's UPC
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ItemUpc { get; set; }

        /// <summary>
        /// The Item's code
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ItemCode { get; }

        /// <summary>
        /// The Product's UPC
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ProductUpc { get; }

        /// <summary>
        /// The Item's SKU
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Sku { get; set; }
    }
}
