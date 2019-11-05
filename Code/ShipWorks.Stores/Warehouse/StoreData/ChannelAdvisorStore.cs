using System.Collections.Generic;
using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// Channel Advisor store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ChannelAdvisorStore : Store
    {
        /// <summary>
        /// ChannelAdvisor OAuth Refresh token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// The number of days back to download orders
        /// </summary>
        public int DaysBack { get; set; }

        /// <summary>
        /// A list of item attribute names to be imported
        /// </summary>
        public IEnumerable<string> ItemAttributesToImport { get; set; }

        /// <summary>
        /// Country code for this store
        /// </summary>
        public string CountryCode { get; set; }
    }
}