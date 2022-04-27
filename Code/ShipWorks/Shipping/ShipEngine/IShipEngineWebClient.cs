using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount;

namespace ShipWorks.Shipping.ShipEngine
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
        /// Connect the given DHL eCommerce account to ShipEngine
        /// </summary>
        Task<GenericResult<string>> ConnectDhlEcommerceAccount(DhlEcommerceRegistrationRequest dhlRequest);

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
        Task<ShipWorks.Shipping.ShipEngine.DTOs.RateShipmentResponse> RateShipment(ShipWorks.Shipping.ShipEngine.DTOs.RateShipmentRequest request, ApiLogSource apiLogSource);

        /// <summary>
        /// purchase a label from ShipEngine using the given rateid
        /// </summary>
        Task<ShipWorks.Shipping.ShipEngine.DTOs.Label> PurchaseLabelWithRate(string rateId, ShipWorks.Shipping.ShipEngine.DTOs.PurchaseLabelWithoutShipmentRequest request, ApiLogSource apiLogSource);

        /// <summary>
        /// Purchases a label from ShipEngine using the given request
        /// </summary>
        Task<ShipWorks.Shipping.ShipEngine.DTOs.Label> PurchaseLabel(ShipWorks.Shipping.ShipEngine.DTOs.PurchaseLabelRequest request, ApiLogSource apiLogSource, TelemetricResult<IDownloadedLabelData> telemetricResult);

        /// <summary>
        /// Void a shipment label
        /// </summary>
        Task<ShipWorks.Shipping.ShipEngine.DTOs.VoidLabelResponse> VoidLabel(string labelId, ApiLogSource apiLogSource);

        /// <summary>
        /// Track a shipment using the label ID
        /// </summary>
        Task<ShipWorks.Shipping.ShipEngine.DTOs.TrackingInformation> Track(string labelId, ApiLogSource apiLogSource);

        /// <summary>
        /// Track a shipment using the carrier code and tracking number
        /// </summary>
        /// <returns></returns>
        Task<ShipWorks.Shipping.ShipEngine.DTOs.TrackingInformation> Track(string carrier, string trackingNumber, ApiLogSource apiLogSource);

        /// <summary>
        /// Connects the given stamps.com account to the users ShipEngine account
        /// </summary>
        Task<GenericResult<string>> ConnectStampsAccount(string username, string password);

        /// <summary>
        /// Disconnect the given stamps.com account from the users ShipEngine account
        /// </summary>
        Task<Result> DisconnectStampsAccount(string carrierId);

        /// <summary>
        /// Update the given stamps.com account to the users ShipEngine account
        /// </summary>
        Task<Result> UpdateStampsAccount(string carrierId, string username, string password);

        /// <summary>
        /// Register a UPS account with One Balance
        /// </summary>
        /// <param name="deviceIdentity">Identifier provided by IOvations software</param>
        Task<GenericResult<string>> RegisterUpsAccount(PersonAdapter personAdapter, string deviceIdentity);

        /// <summary>
        /// Create an Asendia Manifest for the given label IDs
        /// </summary>
        Task<Result> CreateAsendiaManifest(IEnumerable<string> labelIds);
    }
}
