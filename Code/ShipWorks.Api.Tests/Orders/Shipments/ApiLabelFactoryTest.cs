using Xunit;
using ShipWorks.Tests.Shared;
using ShipWorks.Api.Orders.Shipments;
using ShipWorks.Data;
using Moq;
using ShipWorks.Editions.Brown;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Services;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Api.Tests.Orders.Shipments
{
    public class ApiLabelFactoryTest
    {
        private readonly AutoMock mock;
        private readonly ApiLabelFactory testObject;

        public ApiLabelFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ApiLabelFactory>();
        }

        [Fact]
        public void GetLabels_DelegatesToDataResourceManager_GetConsumerResourceReferences()
        {
            testObject.GetLabels(42);
            mock.Mock<IDataResourceManager>().Verify(m => m.GetConsumerResourceReferences(42), Times.Once);
        }

        [Fact]
        public void GetLabelsWithAdapter_DelegatesToDataResourceManagerGetConsumerResourceReferences_WhenDHL()
        {
            var adapter = mock.Mock<ICarrierShipmentAdapter>();
            adapter.SetupGet(a => a.Shipment).Returns(new ShipmentEntity() { ShipmentID = 999, ShipmentTypeCode = ShipmentTypeCode.DhlExpress });

            testObject.GetLabels(adapter.Object);

            mock.Mock<IDataResourceManager>().Verify(m => m.GetConsumerResourceReferences(999), Times.Once);
        }
    }
}
