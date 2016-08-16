namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Helper class for caching 3dcart product data
    /// </summary>
    public class ThreeDCartProductDTO
    {
        /// <summary>
        /// The 3dcart item name
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// The 3dcart option name
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// The 3dcart option description
        /// </summary>
        public string OptionDescription { get; set; }

        /// <summary>
        /// The 3dcart image url
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The 3dcart image thumbnail url
        /// </summary>
        public string ImageThumbnail { get; set; }

        /// <summary>
        /// The 3dcart option price
        /// </summary>
        public decimal OptionPrice { get; set; }

        /// <summary>
        /// The 3dcart warehouse bin
        /// </summary>
        public string WarehouseBin { get; set; }
    }
}
