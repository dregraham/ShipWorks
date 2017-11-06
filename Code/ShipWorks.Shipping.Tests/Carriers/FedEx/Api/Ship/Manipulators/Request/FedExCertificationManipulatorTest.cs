using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExCertificationManipulatorTest
    {
        private FedExCertificationManipulator testObject;
        private readonly AutoMock mock;
        private ShipmentEntity shipment;

        public FedExCertificationManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = Create.Shipment().AsFedEx().Build();
            testObject = mock.Create<FedExCertificationManipulator>();
        }

        [Theory]
        [InlineData(false, null, false)]
        [InlineData(false, "", false)]
        [InlineData(false, "foo", false)]
        [InlineData(true, null, false)]
        [InlineData(true, "", false)]
        [InlineData(true, "foo", true)]
        public void ShouldApply_ReturnsAppropriateValues_ForInput(bool interapptiveUser, string referencePO, bool expected)
        {
            mock.Mock<IFedExSettingsRepository>()
                .SetupGet(x => x.IsInterapptiveUser)
                .Returns(interapptiveUser);

            shipment.FedEx.ReferencePO = referencePO;

            var result = testObject.ShouldApply(shipment);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_AccountsForNullTransactionDetail()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.TransactionDetail);
        }

        [Theory]
        [InlineData("foo", "Processed Foo")]
        [InlineData("bar", "Processed Bar")]
        public void Manipulate_AssignsTransactionId_WhenIsInterapptiveUserIsTrue_AndReferencePOIsNonEmptyString(string referencePO, string processValue)
        {
            shipment.FedEx.ReferencePO = referencePO;

            mock.Mock<IFedExShipmentTokenProcessor>()
                .Setup(x => x.ProcessTokens(referencePO, shipment))
                .Returns(processValue);

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(processValue, result.Value.TransactionDetail.CustomerTransactionId);
        }
    }
}
