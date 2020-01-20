﻿using System.Threading.Tasks;

namespace ShipWorks.ShipEngine
{
    /// <summary>
    /// Client to the ShipEngine Partner API
    /// </summary>
    public interface IShipEnginePartnerWebClient
    {
        /// <summary>
        /// Creates a new ShipEngine account and returns the account ID
        /// </summary>
        Task<string> CreateNewAccountAsync(string partnerApiKey);

        /// <summary>
        /// Creates a new ShipEngine account and returns the account ID
        /// </summary>
        string CreateNewAccount(string partnerApiKey);

        /// <summary>
        /// Gets an ApiKey from the ShipEngine API
        /// </summary>
        Task<string> GetApiKeyAsync(string partnerApiKey, string shipEngineAccountId);

        /// <summary>
        /// Gets an ApiKey from the ShipEngine API
        /// </summary>
        string GetApiKey(string partnerApiKey, string shipEngineAccountId);
    }
}