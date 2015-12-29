using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Other
{
    /// <summary>
    /// Adapter for other specific shipment information
    /// </summary>
    public class OtherShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;
        private readonly OtherShipmentType shipmentType;
        private readonly ICustomsManager customsManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OtherShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeManager, nameof(shipmentTypeManager));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));

            this.shipment = shipment;
            this.customsManager = customsManager;
            shipmentType = shipmentTypeManager.Get(shipment) as OtherShipmentType;
        }

        /// <summary>
        /// Id of the other account associated with this shipment
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used",
            Justification = "Other shipment types don't have accounts")]
        [SuppressMessage("SonarQube", "S108:Nested blocks of code should not be left empty",
            Justification = "Other shipment types don't have accounts")]
        public long? AccountId
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// The shipment associated with this adapter
        /// </summary>
        public ShipmentEntity Shipment
        {
            get
            {
                return shipment;
            }
        }

        /// <summary>
        /// The shipment type code of this shipment adapter
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode
        {
            get
            {
                return ShipmentTypeCode.Other;
            }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public bool SupportsAccounts
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Does this shipment type support multiple packages?
        /// </summary>
        public bool SupportsMultiplePackages
        {
            get
            {
                return shipmentType.SupportsMultiplePackages;
            }
        }

        /// <summary>
        /// Is this shipment a domestic shipment?
        /// </summary>
        public bool IsDomestic
        {
            get
            {
                return shipmentType.IsDomestic(shipment);
            }
        }

        /// <summary>
        /// Updates shipment dynamic data, total weight, etc
        /// </summary>
        /// <returns>Dictionary of shipments and exceptions.</returns>
        public IDictionary<ShipmentEntity, Exception> UpdateDynamicData()
        {
            shipmentType.UpdateDynamicShipmentData(shipment);
            shipmentType.UpdateTotalWeight(shipment);

            return customsManager.EnsureCustomsLoaded(new[] { shipment });
        }

        /// <summary>
        /// Does this shipment type support package Types?
        /// </summary>
        public bool SupportsPackageTypes => false;

        /// <summary>
        /// DateTime of the shipment
        /// </summary>
        public DateTime ShipDate
        {
            get { return shipment.ShipDate; }
            set { shipment.ShipDate = value; }
        }

        /// <summary>
        /// Total weight of the shipment
        /// </summary>
        public double TotalWeight
        {
            get { return shipment.TotalWeight; }
        }

        /// <summary>
        /// Content weight of the shipment
        /// </summary>
        public double ContentWeight
        {
            get { return shipment.ContentWeight; }
            set { shipment.ContentWeight = value; }
        }

        /// <summary>
        /// Is Insurance requested?
        /// </summary>
        public bool UsingInsurance
        {
            get { return shipment.Insurance; }
            set { shipment.Insurance = value; }
        }

        /// <summary>
        /// Service type selected
        /// </summary>
        public int ServiceType { get; set; } = 0;

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public IEnumerable<IPackageAdapter> GetPackageAdapters()
        {
            UpdateDynamicData();
            return shipmentType.GetPackageAdapters(shipment);
        }

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public IEnumerable<IPackageAdapter> GetPackageAdapters(int numberOfPackages)
        {
            return GetPackageAdapters();
        }
    }
}
