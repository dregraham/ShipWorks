using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Interface that represents a surcharge
    /// </summary>
    public interface IUpsSurcharge
    {
        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        void Apply(UpsShipmentEntity shipment, UpsLocalServiceRate serviceRate);
    }
}