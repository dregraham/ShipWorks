using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Repository for Amazon shipping service types
    /// </summary>
    public interface IAmazonServiceTypeRepository
    {
        /// <summary>
        /// Gets a list of service types from the repository
        /// </summary>
        /// <returns></returns>
        List<AmazonServiceTypeEntity> Get();

        /// <summary>
        /// Creates and adds a new service to the repository
        /// </summary>
        void SaveNewService(string apiValue, string description);
    }
}