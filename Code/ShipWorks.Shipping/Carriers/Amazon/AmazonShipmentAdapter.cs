using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using System.Diagnostics.CodeAnalysis;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Other;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Adapter for other specific shipment information
    /// </summary>
    public class AmazonShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;
        private readonly AmazonShipmentType shipmentType;

        /// <summary>
        /// Constuctor
        /// </summary>
        public AmazonShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon, nameof(shipment.Amazon));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeFactory, nameof(shipmentTypeFactory));

            this.shipment = shipment;
            shipmentType = shipmentTypeFactory.Get(shipment) as AmazonShipmentType;
        }
        
        /// <summary>
        /// Id of the other account associated with this shipment
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used", Justification = "Amazon shipment types don't have accounts")]
        [SuppressMessage("SonarQube", "S108:Nested blocks of code should not be left empty", Justification = "Amazon shipment types don't have accounts")]
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
                return ShipmentTypeCode.Amazon;
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

            // Amazon carrier doesn't have customs, so just return the shipment and no exception
            IDictionary<ShipmentEntity, Exception> results = new Dictionary<ShipmentEntity, Exception>();

            return results;
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
            return shipmentType.GetPackageAdapters(shipment);
        }
    }
}
