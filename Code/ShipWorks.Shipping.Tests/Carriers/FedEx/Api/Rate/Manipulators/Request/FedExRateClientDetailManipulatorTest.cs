using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateClientDetailManipulatorTest
    {
        private readonly ShipmentEntity shipment;
        private FedExRateClientDetailManipulator testObject;
        private readonly AutoMock mock;

        public FedExRateClientDetailManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment().AsFedEx().Build();

            testObject = mock.Create<FedExRateClientDetailManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment, FedExRateRequestOptions.None));
        }

        [Fact]
        public void Manipulate_DelegatesToSettings_ForFedExAccount()
        {
            testObject.Manipulate(shipment, new RateRequest());

            mock.Mock<IFedExSettingsRepository>().Verify(x => x.GetAccountReadOnly(shipment));
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull()
        {
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "67890" });

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.Equal("12345", result.ClientDetail.AccountNumber);
            Assert.Equal("67890", result.ClientDetail.MeterNumber);
        }

        [Fact]
        public void Manipulate_OverwritesClientDetail_WhenWebAuthenticationDetailsIsNotNull()
        {
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "67890" });

            var result = testObject.Manipulate(shipment, new RateRequest { ClientDetail = new ClientDetail { AccountNumber = "111" } });

            Assert.Equal("12345", result.ClientDetail.AccountNumber);
        }
    }
}
