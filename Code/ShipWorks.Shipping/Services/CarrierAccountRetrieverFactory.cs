using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Factory for getting carrier account retrievers
    /// </summary>
    public class CarrierAccountRetrieverFactory :
        Factory<ShipmentTypeCode, ICarrierAccountRetriever>, ICarrierAccountRetrieverFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierAccountRetrieverFactory(IIndex<ShipmentTypeCode, ICarrierAccountRetriever> lookup) : base(lookup)
        {
        }
    }
}
