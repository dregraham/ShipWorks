using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Warehouse
{
    /// <summary>
    /// ThreeD Cart Store DTO
    /// </summary>
    public class ThreeDCartStore : Store
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartStore()
        {
        }

        /// <summary>
        /// Url of the store
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// API Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// How far back should we download
        /// </summary>
        public int DownloadNumberOfDaysBack { get; set; }
    }
}