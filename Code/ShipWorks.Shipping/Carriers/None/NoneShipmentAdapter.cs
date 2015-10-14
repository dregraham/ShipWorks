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

        /// <summary>
        /// Constuctor
        /// </summary>
        public NoneShipmentAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            this.shipment = shipment;
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
    }
}
