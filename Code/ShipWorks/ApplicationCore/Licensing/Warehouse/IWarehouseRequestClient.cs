﻿using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using RestSharp;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Represents the WarehouseRequestclient
    /// </summary>
    public interface IWarehouseRequestClient
    {
        /// <summary>
        /// Make the given request
        /// </summary>
        Task<GenericResult<IRestResponse>> MakeRequest(IRestRequest restRequest, string logName);
    }
}