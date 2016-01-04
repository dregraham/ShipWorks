using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.AddressValidation;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Adapter for WebTools specific shipment information
    /// </summary>
    public class PostalWebToolsShipmentAdapter : CarrierShipmentAdapterBase
    {
        /// <summary>
        /// Constuctor
        /// </summary>
        public PostalWebToolsShipmentAdapter(ShipmentEntity shipment, IShipmentTypeFactory shipmentTypeFactory, ICustomsManager customsManager) : base(shipment, shipmentTypeFactory, customsManager)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment.Postal, nameof(shipment.Postal));
            MethodConditions.EnsureArgumentIsNotNull(customsManager, nameof(customsManager));
        }
        
        /// <summary>
        /// Id of the WebTools account associated with this shipment
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used", 
            Justification = "WebTools shipment types don't have accounts")]
        [SuppressMessage("SonarQube", "S108:Nested blocks of code should not be left empty",
            Justification = "WebTools shipment types don't have accounts")]
        public override long? AccountId
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Does this shipment type support accounts?
        /// </summary>
        public override bool SupportsAccounts
        {
            get
            {
                return false;
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
