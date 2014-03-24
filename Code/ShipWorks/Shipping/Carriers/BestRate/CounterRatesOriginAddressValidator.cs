using Interapptive.Shared.Business;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Handles origin addresses for counter rates
    /// </summary>
    public static class CounterRatesOriginAddressValidator
    {
        /// <summary>
        /// Ensures that the counter rate shipment has a valid address
        /// </summary>
        public static void EnsureValidAddress(ShipmentEntity shipment)
        {
            if (shipment.OriginOriginID == (int)ShipmentOriginSource.Account ||
                (shipment.OriginOriginID == (int)ShipmentOriginSource.Other && !IsValid(shipment)))
            {
                // We don't have an account for counter rates or "Other" is selected and is incomplete, 
                // so we'll try to use the store address
                OrderEntity order = DataProvider.GetEntity(shipment.OrderID) as OrderEntity;
                StoreEntity store = DataProvider.GetEntity(order.StoreID) as StoreEntity;

                PersonAdapter.Copy(store, string.Empty, shipment, "Origin");
            }

            if (!IsValid(shipment))
            {
                // The store address is incomplete, too, so the origin address is still incomplete
                throw new CounterRatesOriginAddressException(shipment, "The origin address of this shipment is invalid for getting counter rates.");
            }
        }

        /// <summary>
        /// Validates the origin address of the shipment to ensure there is data for Streeet1, City, State/Province, 
        /// Postal code, and country code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="CounterRatesOriginAddressException">Thrown when the origin address is missing a required data point.</exception>
        public static bool IsValid(ShipmentEntity shipment)
        {
            // Check to see if the address is incomplete
            return !(string.IsNullOrWhiteSpace(shipment.OriginStreet1) || 
                string.IsNullOrWhiteSpace(shipment.OriginCity) ||
                string.IsNullOrWhiteSpace(shipment.OriginStateProvCode) ||
                string.IsNullOrWhiteSpace(shipment.OriginPostalCode) ||
                string.IsNullOrWhiteSpace(shipment.OriginCountryCode));
        }
    }
}
