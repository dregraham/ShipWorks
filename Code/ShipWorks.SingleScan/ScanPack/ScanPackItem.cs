using System.Reflection;

namespace ShipWorks.SingleScan.ScanPack
{
    /// <summary>
    /// Represents an item to be scan and packed
    /// </summary>
    public class ScanPackItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackItem(string name, string imageUrl, double quantity)
        {
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
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
        public double Quantity { get; set; }
    }
}
