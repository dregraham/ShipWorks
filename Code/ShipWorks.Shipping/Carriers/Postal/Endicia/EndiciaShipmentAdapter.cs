using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Adapter for Endicia specific shipment information
    /// </summary>
    public class EndiciaShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaShipmentAdapter(ShipmentEntity shipment, IShipmentTypeManager shipmentTypeManager, ICustomsManager customsManager) : base(shipment, shipmentTypeManager, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal, nameof(shipment.Postal));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal.Endicia, nameof(shipment.Postal.Endicia));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }

        /// <summary>
        /// Id of the Endicia account associated with this shipment
        /// </summary>
        public override long? AccountId
        {
            get { return Shipment.Postal.Endicia.EndiciaAccountID; }
            set { Shipment.Postal.Endicia.EndiciaAccountID = value.GetValueOrDefault(); }
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
            get { return Shipment.Postal.Service; }
            set { Shipment.Postal.Service = value; }
        }
        
        /// <summary>
        /// List of package adapters for the shipment
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(int numberOfPackages)
        {
            return GetPackageAdapters();
        }
    }
}
