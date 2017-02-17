namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Factory for getting carrier account retrievers
    /// </summary>
    public interface ICarrierAccountRetrieverFactory
    {
        /// <summary>
        /// Get a carrier account retriever
        /// </summary>
        ICarrierAccountRetriever Create(ShipmentTypeCode shipmentType);
    }
}
