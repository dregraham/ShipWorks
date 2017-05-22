using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Interface for creating BigCommerce IRestClients
    /// </summary>
    public interface IBigCommerceRestClientFactory
    {
        /// <summary>
        /// Create an IRestClient for the given store
        /// </summary>
        IRestClient Create(IBigCommerceStoreEntity store);
    }
}
