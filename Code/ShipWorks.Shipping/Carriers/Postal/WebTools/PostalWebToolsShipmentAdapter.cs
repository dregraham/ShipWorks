using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Adapter for WebTools specific shipment information
    /// </summary>
    public class PostalWebToolsShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Constuctor
        /// </summary>
        public PostalWebToolsShipmentAdapter(ShipmentEntity shipment)
        {
            this.shipment = shipment;
        }
        
        /// <summary>
        /// Id of the WebTools account associated with this shipment
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used", 
            Justification = "WebTools shipment types don't have accounts")]
        [SuppressMessage("SonarQube", "S108:Nested blocks of code should not be left empty",
            Justification = "WebTools shipment types don't have accounts")]
        public long? AccountId
        {
            get { return null; }
            set { }
        }
    }
}
