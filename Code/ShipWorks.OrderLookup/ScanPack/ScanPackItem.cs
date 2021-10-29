using System;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;
using Interapptive.Shared;

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
        [NDependIgnoreTooManyParams]
        public ScanPackItem(long sortIdentifier, string name, string imageUrl, double quantity, bool isBundle, int? parentSortIdentifier, bool isBundleComplete, params string[] barcodes)
        {
            SortIdentifier = sortIdentifier;
            Name = name;
            ImageUrl = imageUrl;
            Quantity = quantity;
            IsBundle = isBundle;
            ParentSortIdentifier = parentSortIdentifier;
            IsBundleComplete = isBundleComplete;
            Barcodes = barcodes.Where(b=>!string.IsNullOrWhiteSpace(b)).ToArray();
        }

        /// <summary>
        /// Order Item ID
        /// </summary>
        [Obfuscation(Exclude = true)]
        public long SortIdentifier { get; }

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
        /// True if product is a Bundle
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsBundle { get; }
        
        /// <summary>
        /// The identifier of the parent bundle - Otherwise null
        /// </summary>
        [Obfuscation(Exclude = true)]
        public int? ParentSortIdentifier { get; }
        
        /// <summary>
        /// True if all items are in bundle otherwise false 
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsBundleComplete { get; set; }

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
        public ScanPackItem Copy() => new ScanPackItem(SortIdentifier, Name, ImageUrl, Quantity, IsBundle, ParentSortIdentifier, IsBundleComplete, Barcodes);
    }
}
