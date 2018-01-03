using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;
using ShipWorks.Tests.Shared.Carriers.FedEx;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.GlobalShipAddress.Manipulators.Request
{
    public class FedExGlobalShipAddressClientDetailManipulatorTest
    {
        private FedExGlobalShipAddressClientDetailManipulator testObject;
        private ShipmentEntity shipment;
        private readonly AutoMock mock;

        public FedExGlobalShipAddressClientDetailManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var account = new FedExAccountEntity { AccountNumber = "123", MeterNumber = "456" };

            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(account);

            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();

            testObject = mock.Create<FedExGlobalShipAddressClientDetailManipulator>();
        }

        [Fact]
        public void Manipulate_ClientProductInformationIsCorrect_DefaultClientProductDetails()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal("ITSW", result.Value.ClientDetail.ClientProductId);
            Assert.Equal("6828", result.Value.ClientDetail.ClientProductVersion);
        }

        [Fact]
        public void Manipulate_AccountNumberIsCorrect_AccountIdIs123()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal("123", result.Value.ClientDetail.AccountNumber);
        }

        [Fact]
        public void Manipulate_MeterNumberIsCorrect_MeterNumberIs456()
        {
            var result = testObject.Manipulate(shipment, new SearchLocationsRequest());

            Assert.Equal("456", result.Value.ClientDetail.MeterNumber);
        }

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount()
        {
            testObject.Manipulate(shipment, new SearchLocationsRequest());

            mock.Mock<IFedExSettingsRepository>().Verify(r => r.GetAccountReadOnly(shipment));
        }
    }
}
