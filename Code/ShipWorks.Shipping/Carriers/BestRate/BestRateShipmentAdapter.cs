using System.Diagnostics.CodeAnalysis;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Adapter for Best Rate specific shipment information
    /// </summary>
    public class BestRateShipmentAdapter : ICarrierShipmentAdapter
    {
        private readonly ShipmentEntity shipment;

        /// <summary>
        /// Constuctor
        /// </summary>
        public BestRateShipmentAdapter(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.BestRate, nameof(shipment.BestRate));

            this.shipment = shipment;
        }

        /// <summary>
        /// BestRate shipments have no accounts
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used",
            Justification = "BestRate shipment types don't have accounts")]
        [SuppressMessage("SonarQube", "S108:Nested blocks of code should not be left empty",
            Justification = "BestRate shipment types don't have accounts")]
        public long? AccountId
        {
            get { return null; }
            set { }
        }
    }
}
