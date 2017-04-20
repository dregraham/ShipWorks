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
    /// Factory for creating web client authenticators
    /// </summary>
    public interface IBigCommerceAuthenticatorFactory
    {
        /// <summary>
        /// Create the correct authenticator based on the store
        /// </summary>
        IAuthenticator Create(IBigCommerceStoreEntity store);
    }
}
