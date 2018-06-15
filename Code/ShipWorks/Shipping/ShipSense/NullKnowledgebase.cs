using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Shipping.ShipSense
{
    /// <summary>
    /// Acts as the no-operation data source for ShipSense.
    /// </summary>
    [Component(RegistrationType.Self)]
    public class NullKnowledgebase : IKnowledgebase
    {
        /// <summary>
        /// Gets the hashing strategy used by the knowledge base.
        /// </summary>
        public IKnowledgebaseHash HashingStrategy { get; }

        /// <summary>
        /// Gets the IShipSenseOrderItemKeyFactory being used by the knowledge base.
        /// </summary>
        public IShipSenseOrderItemKeyFactory KeyFactory { get; }

        /// <summary>
        /// Saves the knowledge base entry to the knowledge base using the order data as the key.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="order">The order.</param>
        public void Save(KnowledgebaseEntry entry, OrderEntity order)
        {
        }

        /// <summary>
        /// Gets the knowledge base entry for the items in the given order. If an entry does not already exist in the
        /// knowledge base, this will act as a factory method and create an empty entry.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>An instance of a a KnowledgebaseEntr; the package data will be empty if there is not an entry in the knowledge base.</returns>
        public KnowledgebaseEntry GetEntry(OrderEntity order)
        {
            return new KnowledgebaseEntry();
        }

        /// <summary>
        /// Determines whether the specified shipment has overwritten the data in the ShipSense knowledge base.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A Boolean value indicating if the shipment has overwritten data provided by ShipSense.</returns>
        public bool IsOverwritten(ShipmentEntity shipment)
        {
            return false;
        }

        /// <summary>
        /// Resets/truncates the underlying knowledge base data on a background thread causing
        /// the knowledge base to be reset as if it were new.
        /// </summary>
        /// <param name="initiatedBy">The initiated by.</param>
        /// <param name="progressReporter">The progress reporter.</param>
        /// <returns>The Task that is executing the operation.</returns>
        public Task ResetAsync(UserEntity initiatedBy, IProgressReporter progressReporter)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs the shipment data to the ShipSense knowledge base. All exceptions will be caught
        /// and logged and wrapped in a ShippingException.
        /// </summary>
        public void LogShipment(ShipmentType shipmentType, ShipmentEntity shipment)
        {
        }

        /// <summary>
        /// Gets the hash result for the given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>A KnowledgebaseHashResult object.</returns>
        public KnowledgebaseHashResult GetHashResult(OrderEntity order)
        {
            return new KnowledgebaseHashResult(false, string.Empty);
        }
    }
}
