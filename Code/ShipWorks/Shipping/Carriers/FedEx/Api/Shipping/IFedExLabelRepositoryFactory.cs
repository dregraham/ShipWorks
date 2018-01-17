using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping
{
    /// <summary>
    /// Factory for creating label repositories for a FedEx shipment
    /// </summary>
    public interface IFedExLabelRepositoryFactory
    {
        /// <summary>
        /// Create a label repository for a FedEx shipment
        /// </summary>
        IFedExLabelRepository Create(IShipmentEntity shipment);
    }
}
