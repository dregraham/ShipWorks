using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// Rakuten store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class RakutenStore : Store
    {
        /// <summary>
        /// The date to start downloading orders from
        /// </summary>
        public int DownloadStartDate { get; set; }

        /// <summary>
        /// The auth token for this store
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// The shop url for this store
        /// </summary>
        public string ShopUrl { get; set; }

        /// <summary>
        /// The market place id for this store
        /// </summary>
        public string MarketplaceID { get; set; }
    }
}
