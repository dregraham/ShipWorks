using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Shipping.Carriers.None
{
    /// <summary>
    /// Adapter for None specific shipment information
    /// </summary>
    public class NoneShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;
        private readonly NoneShipmentType shipmentType;

        /// <summary>
        /// Constuctor
        /// </summary>
        public NoneShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            this.shipment = shipment;
            shipmentType = shipmentTypeFactory.Get(shipment) as NoneShipmentType;
        }
        
        /// <summary>
        /// Id of the None account associated with this shipment
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used", 
            Justification = "None shipment types don't have accounts")]
        [SuppressMessage("SonarQube", "S108:Nested blocks of code should not be left empty",
            Justification = "None shipment types don't have accounts")]
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
                return ShipmentTypeCode.None;
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
                return false;
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
            return new Dictionary<ShipmentEntity, Exception>();
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

        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public IEnumerable<IPackageAdapter> GetPackageAdapters(int numberOfPackages)
        {
            return shipmentType.GetPackageAdapters(shipment);
        }
    }
}
