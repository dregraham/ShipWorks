﻿using ShipEngine.ApiClient.Api;

namespace ShipWorks.ShipEngine
{
    /// <summary>
    /// Interface for creating ShipEngine API Calls
    /// </summary>
    public interface IShipEngineApiFactory
    {
        /// <summary>
        /// Create the CarrierAccountsApi
        /// </summary>
        ICarrierAccountsApi CreateCarrierAccountsApi();

        /// <summary>
        /// Create the CarrierApi
        /// </summary>
        ICarriersApi CreateCarrierApi();

        /// <summary>
        /// Creates the Rate API
        /// </summary>
        IRatesApi CreateRatesApi();

        /// <summary>
        /// Create the Labels Api
        /// </summary>
        ILabelsApi CreateLabelsApi();

        /// <summary>
        /// Create the Orders Source Api
        /// </summary>
        IOrderSourceAccountsApi CreateOrderSourceApi();
    }
}