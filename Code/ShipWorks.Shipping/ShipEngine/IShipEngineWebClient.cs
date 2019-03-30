﻿using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;

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
        /// Connects the given Asendia account to the users ShipEngine account
        /// </summary>
        /// <returns>The CarrierId</returns>
        Task<GenericResult<string>> ConnectAsendiaAccount(string accountNumber, string username, string password);

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
        Task<Label> PurchaseLabel(PurchaseLabelRequest request, ApiLogSource apiLogSource);

        /// <summary>
        /// Void a shipment label
        /// </summary>
        Task<VoidLabelResponse> VoidLabel(string labelId, ApiLogSource apiLogSource);

		/// <summary>
        /// Track a shipment using the label ID
        /// </summary>
        Task<TrackingInformation> Track(string labelId, ApiLogSource apiLogSource);
    }
}