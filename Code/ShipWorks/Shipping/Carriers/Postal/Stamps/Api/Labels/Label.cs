using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Api.Labels
{
    /// <summary>
    /// An abstract class to encapsulate the logic for persisting Stamps.com
    /// labels to the data source.
    /// </summary>
    public abstract class Label : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Label" /> class.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="name">The name of the label (primary, part2, etc.)</param>
        protected Label(ShipmentEntity shipmentEntity, string name)
        {
            ShipmentEntity = shipmentEntity;
            Name = name;
        }

        /// <summary>
        /// Gets the shipment entity that the label is for.
        /// </summary>
        public ShipmentEntity ShipmentEntity { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Saves the label to the underlying data source.
        /// </summary>
        public abstract void Save();

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected abstract void Dispose(bool disposing);
    }
}
