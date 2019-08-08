using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Platforms.BigCommerce.Warehouse
{
    /// <summary>
    /// Big Commerce warehouse store DTO
    /// </summary>
    [Obfuscation]
    public class BigCommerceStore : Store
    {
        public string ClientId { get; internal set; }
        public string AccessToken { get; internal set; }
        public string Url { get; internal set; }
        public int DownloadNumberOfDaysBack { get; internal set; }
        public int? InitialDownloadDays { get; internal set; }
    }
}
