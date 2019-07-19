using System.Reflection;
using GalaSoft.MvvmLight;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Represents an item to be scan and packed
    /// </summary>
    public class ScanPackItem : ViewModelBase
    {
        private string name;
        private string imageUrl;
        private double quantity;
        private string sku;

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
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// The Items Image Url
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ImageUrl
        {
            get => imageUrl;
            set => imageUrl = value;
        }

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
        public string Sku
        {
            get => sku;
            set => sku = value;
        }
    }
}
