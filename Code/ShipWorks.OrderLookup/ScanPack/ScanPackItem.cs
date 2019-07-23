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
        public ScanPackItem(string name, string imageUrl, double quantity, string sku)
        {
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
            Sku = sku;
        }

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
        /// The Item's SKU
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Sku { get; set; }
    }
}
