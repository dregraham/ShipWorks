using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators
{
    public class FedExGlobalShipAddressWebAuthenticationDetailManipulatorTest
    {
        private FedExGlobalShipAddressWebAuthenticationDetailManipulator testObject;
        private readonly AutoMock mock;
        private ShippingSettingsEntity shippingSettings;

        public FedExGlobalShipAddressWebAuthenticationDetailManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shippingSettings = new ShippingSettingsEntity { FedExPassword = "password", FedExUsername = "username" };

            testObject = mock.Create<FedExGlobalShipAddressWebAuthenticationDetailManipulator>();
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull()
        {
            var result = testObject.Manipulate(null, new SearchLocationsRequest());

            Assert.NotNull(result.Value.WebAuthenticationDetail);
        }
    }
}
