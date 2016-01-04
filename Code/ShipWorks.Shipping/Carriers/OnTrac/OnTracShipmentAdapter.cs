using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Adapter for specific shipment information
    /// </summary>
    public class OnTracShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Constuctor
        /// </summary>
        public OnTracShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager) : base(shipment, shipmentTypeFactory, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.OnTrac, nameof(shipment.OnTrac));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.OnTrac.OnTracAccountID; }
            set { Shipment.OnTrac.OnTracAccountID = value.GetValueOrDefault(); }
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
            get { return Shipment.OnTrac.Service; }
            set { Shipment.OnTrac.Service = value; }
        }
        
        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(int numberOfPackages)
        {
            return GetPackageAdapters();
        }

        /// <summary>
        /// Are customs allowed?
        /// </summary>
        public override bool CustomsAllowed => false;
    }
}
