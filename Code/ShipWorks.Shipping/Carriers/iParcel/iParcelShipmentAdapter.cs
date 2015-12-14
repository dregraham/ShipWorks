using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Adapter for specific shipment information
    /// </summary>
    [SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention",
        Justification = "Class is names to match iParcel's naming convention")]
    public class iParcelShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;
        private readonly iParcelShipmentType shipmentType;
        private readonly ICustomsManager customsManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.IParcel, nameof(shipment.IParcel));
            MethodConditions.EnsureArgumentIsNotNull(shipmentTypeFactory, nameof(shipmentTypeFactory));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));

            this.shipment = shipment;
            this.customsManager = customsManager;
            shipmentType = shipmentTypeFactory.Get(shipment) as iParcelShipmentType;
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public long? AccountId
        {
            get { return shipment.IParcel.IParcelAccountID; }
            set { shipment.IParcel.IParcelAccountID = value.GetValueOrDefault(); }
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
                return ShipmentTypeCode.iParcel;
            }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public bool SupportsAccounts
        {
            get
            {
                return true;
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
        public int ServiceType
        {
            get { return shipment.IParcel.Service; }
            set { shipment.IParcel.Service = value; }
        }

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
            IParcelShipmentEntity iParcel = shipment.IParcel;

            // Need more
            while (iParcel.Packages.Count < numberOfPackages)
            {
                IParcelPackageEntity package = iParcelShipmentType.CreateDefaultPackage();
                iParcel.Packages.Add(package);
            }

            // Need less
            while (iParcel.Packages.Count > numberOfPackages)
            {
                IParcelPackageEntity package = iParcel.Packages[iParcel.Packages.Count - 1];
                iParcel.Packages.Remove(package);
            }

            return GetPackageAdapters();
        }
    }
}
