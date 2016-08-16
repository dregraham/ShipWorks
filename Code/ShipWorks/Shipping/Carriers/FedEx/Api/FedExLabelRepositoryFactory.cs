using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory for label repositories
    /// </summary>
    /// <remarks>This class wraps parameters to reduce the amount seen by NDepend</remarks>
    public class FedExLabelRepositoryFactory : IFedExLabelRepositoryFactory
    {
        /// <summary>
        /// Create a FedEx label repository
        /// </summary>
        public ILabelRepository CreateFedEx() => new FedExLabelRepository();

        /// <summary>
        /// Create a FIMS label repository
        /// </summary>
        public IFimsLabelRepository CreateFims() => new FimsLabelRepository();
    }
}
