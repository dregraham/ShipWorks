using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Adapter for Ups specific shipment information
    /// </summary>
    public class UpsShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Constuctor
        /// </summary>
        public UpsShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager) : base(shipment, shipmentTypeFactory, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Ups, nameof(shipment.Ups));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the Ups account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.Ups.UpsAccountID; }
            set { Shipment.Ups.UpsAccountID = value.GetValueOrDefault(); }
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
            get { return Shipment.Ups.Service; }
            set { Shipment.Ups.Service = value; }
        }

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(int numberOfPackages)
        {
            UpsShipmentEntity carrierShipment = Shipment.Ups;

            // Need more
            while (carrierShipment.Packages.Count < numberOfPackages)
            {
                UpsPackageEntity package = UpsUtility.CreateDefaultPackage();
                carrierShipment.Packages.Add(package);
            }

            // Need less
            while (carrierShipment.Packages.Count > numberOfPackages)
            {
                UpsPackageEntity package = carrierShipment.Packages[carrierShipment.Packages.Count - 1];
                carrierShipment.Packages.Remove(package);
            }

            return GetPackageAdapters();
        }
    }
}
