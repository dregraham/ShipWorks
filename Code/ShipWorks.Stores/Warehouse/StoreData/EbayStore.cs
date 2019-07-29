using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// Ebay store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class EbayStore : Store
    {
        /// <summary>
        /// The date to start downloading orders from
        /// </summary>
        public ulong DownloadStartDate { get; set; }

        /// <summary>
        /// The auth token for this store
        /// </summary>
        public string EbayToken { get; set; }

        /// <summary>
        /// Whether or not to use the eBay sandbox
        /// </summary>
        public bool UseSandbox { get; set; }
    }
}
