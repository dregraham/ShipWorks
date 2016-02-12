﻿using System.Collections.Generic;
using System.Xml.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    public interface IUspsWebClient
    {
        /// <summary>
        /// Authenticate the given user with USPS.
        /// </summary>
        void AuthenticateUser(string username, string password);

        /// <summary>
        /// Get the account info for the given USPS user name
        /// </summary>
        object GetAccountInfo(UspsAccountEntity account);

        /// <summary>
        /// Changes the contract associated with the given account based on the contract type provided.
        /// </summary>
        void ChangeToExpeditedPlan(UspsAccountEntity account, string promoCode);

        /// <summary>
        /// Checks with USPS API to get the contract type of the account.
        /// </summary>
        UspsAccountContractType GetContractType(UspsAccountEntity account);

        /// <summary>
        /// Purchase postage for the given account for the specified amount.  ControlTotal is the ControlTotal value last retrieved from GetAccountInfo.
        /// </summary>
        void PurchasePostage(UspsAccountEntity account, decimal amount, decimal controlTotal);

        /// <summary>
        /// Get the rates for the given shipment based on its settings
        /// </summary>
        List<RateResult> GetRates(ShipmentEntity shipment);

        /// <summary>
        /// Creates the scan form.
        /// </summary>
        /// <param name="shipments">The shipments.</param>
        /// <param name="uspsAccountEntity">The USPS account entity.</param>
        /// <returns>An XDocument having a ScanForm node as the root which contains a TransactionId and Url nodes to
        /// identify results from USPS</returns>
        XDocument CreateScanForm(IEnumerable<UspsShipmentEntity> shipments, UspsAccountEntity uspsAccountEntity);

        /// <summary>
        /// Void the given already processed shipment
        /// </summary>
        void VoidShipment(ShipmentEntity shipment);

        /// <summary>
        /// Process the given shipment, downloading label images and tracking information
        /// </summary>
        void ProcessShipment(ShipmentEntity shipment);

        /// <summary>
        /// Populates a usps account entity.
        /// </summary>
        /// <param name="account">The account.</param>
        void PopulateUspsAccountEntity(UspsAccountEntity account);
    }
}