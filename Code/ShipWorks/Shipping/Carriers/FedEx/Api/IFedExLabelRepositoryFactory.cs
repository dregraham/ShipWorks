using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory for label repositories
    /// </summary>
    /// <remarks>This class wraps parameters to reduce the amount seen by NDepend</remarks>
    public interface IFedExLabelRepositoryFactory
    {
        /// <summary>
        /// Create a FedEx label repository
        /// </summary>
        ILabelRepository CreateFedEx();

        /// <summary>
        /// Create a FIMS label repository
        /// </summary>
        IFimsLabelRepository CreateFims();
    }
}