﻿using System.Collections.Generic;
using System.Xml.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Api
{
    public interface IStampsWebClient
    {
        /// <summary>
        /// Authenticate the given user with Stamps.com.
        /// </summary>
        void AuthenticateUser(string username, string password);

        /// <summary>
        /// Get the account info for the given Stamps.com user name
        /// </summary>
        object GetAccountInfo(StampsAccountEntity account);

        /// <summary>
        /// Changes the contract associated with the given account based on the contract type provided.
        /// </summary>
        void ChangeToExpeditedPlan(StampsAccountEntity account, string promoCode);

        /// <summary>
        /// Checks with Stamps.com API to get the contract type of the account.
        /// </summary>
        StampsAccountContractType GetContractType(StampsAccountEntity account);

        /// <summary>
        /// Get the Stamps.com URL of the given urlType
        /// </summary>
        string GetUrl(StampsAccountEntity account, UrlType urlType);

        /// <summary>
        /// Purchase postage for the given account for the specified amount.  ControlTotal is the ControlTotal value last retrieved from GetAccountInfo.
        /// </summary>
        void PurchasePostage(StampsAccountEntity account, decimal amount, decimal controlTotal);

        /// <summary>
        /// Get the rates for the given shipment based on its settings
        /// </summary>
        List<RateResult> GetRates(ShipmentEntity shipment);
        
        /// <summary>
        /// Registers a new account with Stamps.com.
        /// </summary>
        StampsRegistrationResult RegisterAccount(StampsRegistration registration);

        /// <summary>
        /// Creates the scan form.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <param name="stampsAccountEntity">The stamps account entity.</param>
        /// <returns>An XDocument having a ScanForm node as the root which contains a TransactionId and Url nodes to 
        /// identify results from Stamps.com</returns>
        XDocument CreateScanForm(IEnumerable<StampsShipmentEntity> shipments, StampsAccountEntity stampsAccountEntity);

        /// <summary>
        /// Void the given already processed shipment
        /// </summary>
        void VoidShipment(ShipmentEntity shipment);

        /// <summary>
        /// Process the given shipment, downloading label images and tracking information
        /// </summary>
        void ProcessShipment(ShipmentEntity shipment);
    }
}