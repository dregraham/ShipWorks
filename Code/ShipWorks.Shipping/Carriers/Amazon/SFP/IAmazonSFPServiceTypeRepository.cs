using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Repository for Amazon shipping service types
    /// </summary>
    public interface IAmazonSFPServiceTypeRepository
    {
        /// <summary>
        /// Gets a list of service types from the repository
        /// </summary>
        /// <returns></returns>
        List<AmazonSFPServiceTypeEntity> Get();

        /// <summary>
        /// Creates and adds a new service to the repository
        /// </summary>
        void SaveNewService(string apiValue, string description);

        /// <summary>
        /// Get carrier name
        /// </summary>
        string GetCarrierName(AmazonSFPServiceTypeEntity amazonSfpServiceType);

        /// <summary>
        /// Get carrier name
        /// </summary>
        AmazonSFPServiceTypeEntity Find(string valueToSearch);

        /// <summary>
        /// Get carrier name
        /// </summary>
        string GetCarrierName(string id);
    }
}