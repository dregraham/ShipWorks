using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Labels
{
    /// <summary>
    /// Represents a USPS thermal label and encapsulates the logic for
    /// persisting the label to the data source.
    /// </summary>
    public class ThermalLabel : Label
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThermalLabel"/> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="name">The name.</param>
        /// <param name="thermalData">The thermal data.</param>
        public ThermalLabel(ShipmentEntity shipmentEntity, string name, byte[] thermalData)
            : base(shipmentEntity, name)
        {
            ThermalData = thermalData;
        }

        /// <summary>
        /// Gets the thermal data.
        /// </summary>
        public byte[] ThermalData { get; private set; }

        /// <summary>
        /// Saves the label to the underlying data source.
        /// </summary>
        public override void Save()
        {
            DataResourceManager.CreateFromBytes(ThermalData, ShipmentEntity.ShipmentID, Name);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Nothing to dispose here.
        }
    }
}
