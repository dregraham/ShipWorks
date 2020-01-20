using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.ShipEngine
{
    /// <summary>
    /// Web client for communicating with ShipEngine
    /// </summary>
    public interface IShipEngineWebClient
    {
        /// <summary>
        /// Connects the given DHL account to the users ShipEngine account
        /// </summary>
        /// <returns>The CarrierId</returns>
        Task<GenericResult<string>> ConnectDhlAccount(string accountNumber);

        /// <summary>
        /// Connects the given Asendia account to the users ShipEngine account
        /// </summary>
        /// <returns>The CarrierId</returns>
        Task<GenericResult<string>> ConnectAsendiaAccount(string accountNumber, string username, string password);

        /// <summary>
        /// Disconnect the given amazon shipping account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Task<Result> DisconnectAmazonShippingAccount(string accountId);

        /// <summary>
        /// Connect an Amazon Shipping Account
        /// </summary>
        /// <remarks>
        /// unlike the other methods in this class we are manually interacting
        /// with the ShipEngine API because they have not added connecting to
        /// Amazon to their DLL yet
        /// </remarks>
        Task<GenericResult<string>> ConnectAmazonShippingAccount(string authCode);

        /// <summary>
        /// Update an amazon accounts info
        /// </summary>
        Task<Result> UpdateAmazonAccount(IAmazonSWAAccountEntity amazonSwaAccount);

        /// <summary>
        /// Gets rates from ShipEngine using the given request
        /// </summary>
        /// <param name="request">The rate shipment request</param>
        /// <returns>The rate shipment response</returns>
        Task<RateShipmentResponse> RateShipment(RateShipmentRequest request, ApiLogSource apiLogSource);

        /// <summary>
        /// purchase a label from ShipEngine using the given rateid
        /// </summary>
        Task<Label> PurchaseLabelWithRate(string rateId, PurchaseLabelWithoutShipmentRequest request, ApiLogSource apiLogSource);

        /// <summary>
        /// Purchases a label from ShipEngine using the given request
        /// </summary>
        Task<Label> PurchaseLabel(PurchaseLabelRequest request, ApiLogSource apiLogSource, TelemetricResult<IDownloadedLabelData> telemetricResult);

        /// <summary>
        /// Void a shipment label
        /// </summary>
        Task<VoidLabelResponse> VoidLabel(string labelId, ApiLogSource apiLogSource);

        /// <summary>
        /// Track a shipment using the label ID
        /// </summary>
        Task<TrackingInformation> Track(string labelId, ApiLogSource apiLogSource);

        /// <summary>
        /// Get an account ID from a WhoAmI request
        /// </summary>
        Task<string> GetAccountIDAsync();

        /// <summary>
        /// Get an account ID synchronously from a WhoAmI request
        /// </summary>
        string GetAccountID();

        /// <summary>
        /// Add a new store to ShipEngine
        /// </summary>
        Guid? AddStore(ApiOrderSourceAccountInformationRequest accountInfo, string resource);

        /// <summary>
        /// Update a stores credentials in ShipEngine
        /// </summary>
        Guid? UpdateStoreCredentials(ApiOrderSourceAccountInformationRequest accountInfo, Guid? orderSourceId, string resource);


        /// <summary>
        /// Remove a store from ShipEngine
        /// </summary>
        void DeleteStore(Guid? orderSourceId);
    }
}