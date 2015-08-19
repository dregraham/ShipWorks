using ShipWorks.Data.Model.EntityClasses;

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
        bool IsCustomsRequired(ShipmentEntity shipment);
    }
}