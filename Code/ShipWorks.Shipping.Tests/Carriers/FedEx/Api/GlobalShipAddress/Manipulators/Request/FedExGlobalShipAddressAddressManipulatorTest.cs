using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    public class FedExGlobalShipAddressAddressManipulatorTest
    {
        private FedExGlobalShipAddressAddressManipulator testObject;
        private ShipmentEntity shipment;

        public FedExGlobalShipAddressAddressManipulatorTest()
        {
            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            testObject = new FedExGlobalShipAddressAddressManipulator();
        }

        [Fact]
        public void Manipulate_HasAddress_AddressInShipment()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.NotNull(result.Value.Address);
        }

        [Fact]
        public void Manipulate_HasAddressStreetLines_AddressStreetLinesInShipment()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.NotNull(result.Value.Address.StreetLines != null);
            Assert.Equal(2, result.Value.Address.StreetLines.Length);
        }

        [Fact]
        public void Manipulate_StreetLinesMatchRequest_AddressStreetLinesInShipment()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(shipment.ShipStreet1, result.Value.Address.StreetLines[0]);
            Assert.Equal(shipment.ShipStreet2, result.Value.Address.StreetLines[1]);
        }

        [Fact]
        public void Manipulate_CityCorrect_AddressInRequestHasCity()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(shipment.ShipCity, result.Value.Address.City);
        }

        [Fact]
        public void Manipulate_StateCorrect_AddressInRequestHasState()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(shipment.ShipStateProvCode, result.Value.Address.StateOrProvinceCode);
        }

        [Fact]
        public void Manipulate_ZipCorrect_AddressInRequestHasZip()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(shipment.ShipPostalCode, result.Value.Address.PostalCode);
        }

        [Fact]
        public void Manipulate_CountryCorrect_AddressInRequestHasCountry()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(shipment.ShipCountryCode, result.Value.Address.CountryCode);
        }

        [Fact]
        public void Manipulate_ResidentialCorrect_AddressInRequestHasResidentialResult()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(shipment.ResidentialResult, result.Value.Address.Residential);
        }

        [Fact]
        public void Manipulate_OtherCriteriaCorrect_ShipmentValid()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal(LocationsSearchCriteriaType.ADDRESS, result.Value.LocationsSearchCriterion);
            Assert.True(result.Value.LocationsSearchCriterionSpecified);
        }
    }
}
