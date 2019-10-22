using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Error codes that can be generated from the domain
    /// </summary>
    [Obfuscation(StripAfterObfuscation = false)]
    public enum HubErrorCode
    {
        /// <summary>
        /// Unknown error
        /// </summary>
        [Description("Unknown error")]
        Unknown = 0,

        /// <summary>
        /// There are no warehouses available to start the routing process
        /// </summary>
        [Description("There are no warehouses available to start the routing process")]
        NoWarehousesAvailableForRouting = 1,

        /// <summary>
        /// There are no warehouses left after the routing process has finished
        /// </summary>
        [Description("There are no warehouses left after the routing process has finished")]
        NoWarehousesLeftForRouting = 2,

        /// <summary>
        /// Items that are trying to be rerouted are not currently routed to the origin warehouse
        /// </summary>
        [Description("Items that are trying to be rerouted are not currently routed to the origin warehouse")]
        ItemsAreNotRoutedToWarehouse = 3,

        /// <summary>
        /// The customer only has a single warehouse
        /// </summary> 
        [Description("The customer only has a single warehouse")]
        CustomerHasSingleWarehouse = 4,
    }
}