using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Interface for creating generic store web clients
    /// </summary>
    public interface IGenericStoreWebClientFactory
    {
        /// <summary>
        /// Create a web client for the given store
        /// </summary>
        IGenericStoreWebClient CreateWebClient(long storeID);
    }
}
