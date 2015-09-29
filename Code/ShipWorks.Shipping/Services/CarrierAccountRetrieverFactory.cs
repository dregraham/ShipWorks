using ShipWorks.Shipping.Carriers;
using ShipWorks.Data.Model.Custom;
using Autofac.Features.Indexed;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Factory for getting carrier account retrievers
    /// </summary>
    public class CarrierAccountRetrieverFactory : ICarrierAccountRetrieverFactory
    {
        private IIndex<ShipmentTypeCode, ICarrierAccountRetriever<ICarrierAccount>> lookup;

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierAccountRetrieverFactory(IIndex<ShipmentTypeCode, ICarrierAccountRetriever<ICarrierAccount>> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Get a carrier account retriever
        /// </summary>
        public ICarrierAccountRetriever<ICarrierAccount> Get(ShipmentTypeCode shipmentType)
        {
            return lookup[shipmentType];
        }
    }
}
