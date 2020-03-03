using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS
{
    public class UpsOltLabelServiceTest
    {
        private readonly AutoMock mock;
        private UpsOltLabelService testObject;
        private readonly ShipmentEntity shipment;

        public UpsOltLabelServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<UpsOltLabelService>();

            shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.UpsOnLineTools,
                Ups = new UpsShipmentEntity()
            };
        }

        [Fact]
        public void Create_DelegatesToValidatorFactoryForValidator()
        {
            testObject.Create(shipment);

            mock.Mock<IUpsShipmentValidatorFactory>().Verify(v => v.Create(shipment));
        }

        [Fact]
        public void Create_DelegatesToLabelClientFactoryForValidator()
        {
            testObject.Create(shipment);
            
            mock.Mock<IUpsLabelClientFactory>().Verify(f => f.GetClient(shipment));
        }
    }
}
