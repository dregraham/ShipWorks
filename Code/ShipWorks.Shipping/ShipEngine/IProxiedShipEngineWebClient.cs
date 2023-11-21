using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Class for making requests to ShipEngine through our Hub proxy
    /// </summary>
    public interface IProxiedShipEngineWebClient
    {
        /// <summary>
        /// Connect an Amazon Shipping Account
        /// </summary>
        /// <remarks>
        /// unlike the other methods in this class we are manually interacting
        /// with the ShipEngine API because they have not added connecting to
        /// Amazon to their DLL yet
        /// </remarks>
        Task<GenericResult<string>> ConnectAmazonShippingAccount(string authCode, Func<Task<GenericResult<string>>> GetAmazonShippingCarrierID);

        /// <summary>
        /// Create an Asendia Manifest for the given label IDs, retrying if necessary
        /// </summary>
        Task<Result> CreateAsendiaManifest(IEnumerable<string> labelIDs);

        /// <summary>
        /// Get the api key
        /// </summary>
        Task<string> GetApiKey();

        /// <summary>
        /// Register a UPS account with One Balance
        /// </summary>
        Task<GenericResult<string>> RegisterUpsAccount(PersonAdapter person, string deviceIdentity);

        /// <summary>
        /// Update an Amazon Accounts info
        /// </summary>
        Task<Result> UpdateAmazonAccount(IAmazonSWAAccountEntity amazonSwaAccount);

        /// <summary>
        /// Update the given stamps account with the username and password
        /// </summary>
        Task<Result> UpdateStampsAccount(string carrierId, string username, string password);
        Task<GenericResult<bool>> UpsGroundSaverEnable(string seAccountId);
        Task<GenericResult<bool>> UpsGroundSaverEnabledState(string seAccountId);
    }
}