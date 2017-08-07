using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetShipmentRequestFactoryTest
    {
        private readonly AutoMock mock;
        private readonly JetShipmentRequestFactory testObject;

        public JetShipmentRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<JetShipmentRequestFactory>();
        }

        [Fact]
        public void Create_PopulatesOrderDetails()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                ShipmentType = (int) ShipmentTypeCode.Other,
                Order = new JetOrderEntity()
            };

            shipment.Order.OrderItems.Add(new JetOrderItemEntity() {MerchantSku = "123", Quantity = 3});

            testObject.Create(shipment);

            mock.Mock<IOrderManager>().Verify(m => m.PopulateOrderDetails(shipment));
        }
    }
}