using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using Xunit;
using VersionId = ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress.VersionId;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    public class FedExGlobalShipAddressVersionManipulatorTest
    {
        private FedExGlobalShipAddressVersionManipulator testObject;

        public FedExGlobalShipAddressVersionManipulatorTest()
        {
            testObject = new FedExGlobalShipAddressVersionManipulator();
        }

        [Fact]
        public void Manipulate_SetsServiceIdToGlobalShipAddress()
        {
            var result = testObject.Manipulate(null, new SearchLocationsRequest());

            VersionId version = result.Value.Version;
            Assert.Equal("gsai", version.ServiceId);
        }

        [Fact]
        public void Manipulate_SetsMajorTo1()
        {
            var result = testObject.Manipulate(null, new SearchLocationsRequest());

            VersionId version = result.Value.Version;
            Assert.Equal(2, version.Major);
        }

        [Fact]
        public void Manipulate_SetsMinorTo0()
        {
            var result = testObject.Manipulate(null, new SearchLocationsRequest());

            VersionId version = result.Value.Version;
            Assert.Equal(0, version.Minor);
        }

        [Fact]
        public void Manipulate_SetsIntermediateTo0()
        {
            var result = testObject.Manipulate(null, new SearchLocationsRequest());

            VersionId version = result.Value.Version;
            Assert.Equal(0, version.Intermediate);
        }
    }
}
