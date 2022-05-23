using System.Reflection;

namespace ShipWorks.Stores.Platforms.ShipEngine.Apollo
{
    /// <summary>
    /// The 
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class GetOrdersDTO
    {
        /// <summary>
        /// The list of orders returned from platform
        /// </summary>
        public PaginatedPlatformServiceResponseOfOrderSourceApiSalesOrder Orders { get; set; }

        /// <summary>
        /// If there is currently an error within the refresh state in platform
        /// </summary>
        public bool Error { get; set; }
    }
}
