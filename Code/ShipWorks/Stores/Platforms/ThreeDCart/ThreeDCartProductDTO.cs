namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Helper class for caching 3D Cart product data
    /// </summary>
    public class ThreeDCartProductDTO
    {
        /// <summary>
        /// The 3D Cart item name
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// The 3D Cart option name
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// The 3D Cart option description
        /// </summary>
        public string OptionDescription { get; set; }

        /// <summary>
        /// The 3D Cart image url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The 3D Cart image thumbnail url
        /// </summary>
        public string ImageThumbnail { get; set; }

        /// <summary>
        /// The 3D Cart option price
        /// </summary>
        public decimal OptionPrice { get; set; }

        /// <summary>
        /// The 3D Cart warehouse bin
        /// </summary>
        public string WarehouseBin { get; set; }
    }
}
