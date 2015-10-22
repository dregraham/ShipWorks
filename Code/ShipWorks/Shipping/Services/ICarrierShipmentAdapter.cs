﻿using System;
using System.Collections.Generic;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Adapter for carrier specific fields that are actually common across them
    /// </summary>
    public interface ICarrierShipmentAdapter
    {
        /// <summary>
        /// Id of the carrier account
        /// </summary>
        long? AccountId { get; set; }

        /// <summary>
        /// The shipment associated with this adapter
        /// </summary>
        ShipmentEntity Shipment { get; }

        /// <summary>
        /// The shipment type code of this shipment adapter
        /// </summary>
        ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        bool SupportsAccounts { get; }

        /// <summary>
        /// Does this shipment type support multiple packages?
        /// </summary>
        bool SupportsMultiplePackages { get; }

        /// <summary>
        /// Is this shipment a domestic shipment?
        /// </summary>
        bool IsDomestic { get; }

        /// <summary>
        /// Updates shipment dynamic data, total weight, etc
        /// </summary>
        /// <param name="validatedAddressScope"></param>
        /// <returns>Dictionary of shipments and exceptions.</returns>
        IDictionary<ShipmentEntity, Exception> UpdateDynamicData();
    }
}