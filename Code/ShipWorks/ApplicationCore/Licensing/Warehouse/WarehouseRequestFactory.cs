using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net.RestSharp;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Common.Net;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Create an initialized request for a warehouse
    /// </summary>
    [Component]
    public class WarehouseRequestFactory : IWarehouseRequestFactory
    {
        private readonly IRestRequestFactory restRequestFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseRequestFactory(IRestRequestFactory restRequestFactory)
        {
            this.restRequestFactory = restRequestFactory;
        }

        /// <summary>
        /// Create an initialized request for a warehouse
        /// </summary>
        public IRestRequest Create(string endpoint, Method method, object payload)
        {
            var request = restRequestFactory.Create(endpoint, method);
            request.JsonSerializer = new RestSharpJsonNetSerializer();
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(payload);

            return request;
        }
    }
}
