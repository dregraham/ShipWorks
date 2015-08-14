using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    public interface IFedExShippingClerk : IShippingClerk
    {
        /// <summary>
        /// Processes the end of day close for ground shipments.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A FedExEndOfDayCloseEntity object containing the details of each closing; a null value is returned if there weren't any shipments closed.</returns>
        FedExEndOfDayCloseEntity CloseGround(FedExAccountEntity account);

        /// <summary>
        /// Processes the end of day close for smart post shipments.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A FedExEndOfDayCloseEntity object containing the details of each closing; a null value is returned if there weren't any shipments closed.</returns>
        FedExEndOfDayCloseEntity CloseSmartPost(FedExAccountEntity account);

        /// <summary>
        /// Queries FedEx for HoldAtLocations near the destination address.
        /// </summary>
        DistanceAndLocationDetail[] PerformHoldAtLocationSearch(ShipmentEntity shipment);
    }
}
