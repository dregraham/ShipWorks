using System.Reflection;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// Magento store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class MagentoStore : GenericModuleStore
    {
        /// <summary>
        /// Magento version
        /// </summary>
        public uint MagentoVersion { get; set; }
    }
}