using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Ups.ShipEngine
{
    /// <summary>
    /// Rating service for UPS via ShipEngine
    /// </summary>
    public interface IUpsShipEngineRatingService
    {
        /// <summary>
        /// Retrieve rates from ShipEngine for the given shipment
        /// </summary>
        Task<RateGroup> GetRates(ShipmentEntity shipment);
    }
}