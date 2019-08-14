using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Platforms.Walmart.Warehouse
{
    /// <summary>
    /// Big Commerce warehouse store DTO
    /// </summary>
    [Obfuscation]
    public class WalmartStore : Store
    {
        public string ClientId { get; internal set; }
        public string ClientSecret { get; internal set; }
        public int DownloadNumberOfDaysBack { get; internal set; }
    }
}
