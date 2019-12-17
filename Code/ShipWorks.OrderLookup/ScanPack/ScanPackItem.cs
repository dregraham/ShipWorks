using System;
using System.Linq;
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
        public ScanPackItem(long orderItemID, string name, string imageUrl, double quantity, params string[] barcodes)
        {
            OrderItemID = orderItemID;
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
            Barcodes = barcodes;
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
        /// A collection of barcodes that can be matched on
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string[] Barcodes { get; }

        /// <summary>
        /// Does the barcode match the item
        /// </summary>
        public bool IsMatch(string barcodeText) => Barcodes.Any(b => b.Equals(barcodeText, StringComparison.InvariantCultureIgnoreCase));

        /// <summary>
        /// Creates a copy of this ScanPackItem
        /// </summary>
        public ScanPackItem Copy() => new ScanPackItem(OrderItemID, Name, ImageUrl, Quantity, Barcodes);
    }
}
