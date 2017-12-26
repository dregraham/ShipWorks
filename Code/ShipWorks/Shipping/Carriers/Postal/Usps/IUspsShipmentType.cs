using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Interface for UspsShipmentType
    /// </summary>
    public interface IUspsShipmentType
    {
        /// <summary>
        /// Validate the shipment before processing or rating
        /// </summary>
        void ValidateShipment(ShipmentEntity shipment);

        /// <summary>
        /// Should we rate shop before processing
        /// </summary>
        bool ShouldRateShop(ShipmentEntity shipment);

        /// <summary>
        /// Get the Express1 account that should be used for auto routing.
        /// Returns null if auto routing should not be used.
        /// </summary>
        bool ShouldTestExpress1Rates(ShipmentEntity shipment);

        /// <summary>
        /// Creates the web client to use to contact the underlying carrier API.
        /// </summary>
        /// <returns>An instance of IUspsWebClient. </returns>
        IUspsWebClient CreateWebClient();

        /// <summary>
        /// Update the shipment to use the specified account
        /// </summary>
        void UseAccountForShipment(UspsAccountEntity account, ShipmentEntity shipment);

        /// <summary>
        /// Update the dynamic data of the shipment
        /// </summary>
        void UpdateDynamicShipmentData(ShipmentEntity shipment);
    }
}