using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Interface for a ServiceBuilder - Returns service number and string based on shipments
    /// </summary>
    public interface IShipmentServicesBuilder
    {
        /// <summary>
        /// Builds the service type dictionary.
        /// </summary>
        Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments);
    }
}