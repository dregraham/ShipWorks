using ShipWorks.Data.Model.Custom;

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
        ICarrierAccountRetriever<ICarrierAccount> Get(ShipmentTypeCode shipmentType);
    }
}
