﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Adapter for FedEx specific shipment information
    /// </summary>
    public class FedExShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Constuctor
        /// </summary>
        public FedExShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager) : base(shipment, shipmentTypeFactory, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
            MethodConditions.EnsureArgumentIsNotNull(shipment.FedEx, nameof(shipment.FedEx));
        }

        /// <summary>
        /// Id of the FedEx account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.FedEx.FedExAccountID; }
            set { Shipment.FedEx.FedExAccountID = value.GetValueOrDefault(); }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public override bool SupportsAccounts
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public override bool SupportsPackageTypes => true;

        /// <summary>
        /// Service type selected
        /// </summary>
        public override int ServiceType
        {
            get { return Shipment.FedEx.Service; }
            set { Shipment.FedEx.Service = value; }
        }

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(int numberOfPackages)
        {
            FedExShipmentEntity carrierShipment = Shipment.FedEx;

            // Need more
            while (carrierShipment.Packages.Count < numberOfPackages)
            {
                FedExPackageEntity package = FedExUtility.CreateDefaultPackage();
                carrierShipment.Packages.Add(package);
            }

            // Need less
            while (carrierShipment.Packages.Count > numberOfPackages)
            {
                FedExPackageEntity package = carrierShipment.Packages[carrierShipment.Packages.Count - 1];
                carrierShipment.Packages.Remove(package);
            }

            return GetPackageAdapters();
        }
    }
}
