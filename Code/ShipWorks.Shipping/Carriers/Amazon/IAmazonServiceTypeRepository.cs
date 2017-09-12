﻿using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Amazon.Enums;

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
        List<AmazonServiceType> Get();

        /// <summary>
        /// Creates and adds a new service to the repository
        /// </summary>
        AmazonServiceType CreateNewService(string name, string description);
    }
}