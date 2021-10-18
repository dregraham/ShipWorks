using System.Collections.Generic;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;

namespace ShipWorks.Warehouse.Configuration.Stores.DTO
{
    /// <summary>
    /// Store settings downloaded from the Hub
    /// </summary>
    [Obfuscation]
    public class StoreSettings
    {
        /// <summary>
        /// A prefix to prepend to shipworks-created order numbers to make them unique. 
        /// EX: 'PREFIX-123456789-POSTFIX'
        /// </summary>
        public string ManualOrdersPrefix { get; set; }

        /// <summary>
        /// A postfix to append to shipworks-created order numbers to make them unique.
        /// EX: 'PREFIX-123456789-POSTFIX'
        /// </summary>
        public string ManualOrdersPostfix { get; set; }

        /// <summary>
        /// How shipworks should handle domestic address validation suggestions.
        /// </summary>
        public string AddressValidationDomestic { get; set; }

        /// <summary>
        /// How shipworks should handle international address validation suggestions.
        /// </summary>
        public string AddressValidationInternational { get; set; }

        /// <summary>
        /// True if the user actively ships with this store.
        /// </summary>
        public bool ActivelyShips { get; set; }
    }
}
