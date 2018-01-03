using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Contains function to determine if customs are required
    /// </summary>
    public interface ICustomsRequired
    {
        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment
        /// </summary>
        bool IsCustomsRequired(IShipmentEntity shipment);
    }
}