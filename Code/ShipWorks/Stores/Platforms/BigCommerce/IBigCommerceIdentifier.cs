using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Allows interaction with the BigCommerce identifier
    /// </summary>
    public interface IBigCommerceIdentifier
    {
        /// <summary>
        /// Get the identifier from the given store
        /// </summary>
        string Get(IBigCommerceStoreEntity typedStore);

        /// <summary>
        /// Set the identifier on the given store
        /// </summary>
        BigCommerceStoreEntity Set(BigCommerceStoreEntity store);
    }
}
