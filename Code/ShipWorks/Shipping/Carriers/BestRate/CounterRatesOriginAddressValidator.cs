using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public static class CounterRatesOriginAddressValidator
    {
        /// <summary>
        /// Validates the origin address of the shipment to ensure there is data for Streeet1, City, State/Province, 
        /// Postal code, and country code.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="CounterRatesOriginAddressException">Thrown when the origin address is missing a required data point.</exception>
        public static void Validate(ShipmentEntity shipment)
        {
            // Check to see if the address is incomplete
            if (string.IsNullOrWhiteSpace(shipment.OriginStreet1) || string.IsNullOrWhiteSpace(shipment.OriginCity) || string.IsNullOrWhiteSpace(shipment.OriginStateProvCode)
                || string.IsNullOrWhiteSpace(shipment.OriginPostalCode) || string.IsNullOrWhiteSpace(shipment.OriginCountryCode))
            {
                throw new CounterRatesOriginAddressException(shipment, "The origin address of this shipment is invalid for getting counter rates.");
            }
        }
    }
}
