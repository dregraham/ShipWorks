using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    [TestClass]
    public class CounterRatesOriginAddressValidatorTest
    {

        [TestMethod]
        [ExpectedException(typeof(CounterRatesOriginAddressException))]
        public void Validate_ThrowsCounterRatesOriginAddressException_WhenOriginStreet1IsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginStreet1 = string.Empty;

            CounterRatesOriginAddressValidator.Validate(shipment);
        }

        [TestMethod]
        [ExpectedException(typeof(CounterRatesOriginAddressException))]
        public void Validate_ThrowsCounterRatesOriginAddressException_WhenOriginCityIsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginCity = string.Empty;

            CounterRatesOriginAddressValidator.Validate(shipment);
        }

        [TestMethod]
        [ExpectedException(typeof(CounterRatesOriginAddressException))]
        public void Validate_ThrowsCounterRatesOriginAddressException_WhenOriginStateIsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginStateProvCode = string.Empty;

            CounterRatesOriginAddressValidator.Validate(shipment);
        }

        [TestMethod]
        [ExpectedException(typeof(CounterRatesOriginAddressException))]
        public void Validate_ThrowsCounterRatesOriginAddressException_WhenOriginPostalCode1IsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginPostalCode = string.Empty;

            CounterRatesOriginAddressValidator.Validate(shipment);
        }

        [TestMethod]
        [ExpectedException(typeof(CounterRatesOriginAddressException))]
        public void Validate_ThrowsCounterRatesOriginAddressException_WhenOriginCountryCodeIsEmpty_Test()
        {
            ShipmentEntity shipment = CreateShipment();
            shipment.OriginCountryCode = string.Empty;

            CounterRatesOriginAddressValidator.Validate(shipment);
        }

        private ShipmentEntity CreateShipment()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                OriginStreet1 = "1 Memorial Drive",
                OriginCity = "St. Louis",
                OriginStateProvCode = "MO",
                OriginPostalCode = "63102",
                OriginCountryCode = "US"
            };

            return shipment;
        }
    }
}
