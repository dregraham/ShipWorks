using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.OrderLookup.ShipmentHistory;
using ShipWorks.Shipping;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.ShipmentHistory
{
    public class PreviousShipmentActionManagerTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IShippingManager> shippingManager;
        private readonly Mock<IMessenger> messenger;
        private readonly Mock<ISqlAdapter> sqlAdapter;
        private readonly Mock<ISqlAdapterFactory> sqlAdapterFactory;
        private PreviousShipmentActionManager testObject;

        public PreviousShipmentActionManagerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shippingManager = mock.Mock<IShippingManager>();
            messenger = mock.Mock<IMessenger>();
            sqlAdapter = mock.Mock<ISqlAdapter>();
            sqlAdapterFactory = mock.Mock<ISqlAdapterFactory>();
        }

        [Fact]
        public async Task ReprintLastShipment_SendsReprintLabelsMessage()
        {
            ShipmentEntity unRefreshedShipment = new ShipmentEntity(3);

            sqlAdapter.Setup(a => a.FetchFirstAsync(It.IsAny<DynamicQuery<long?>>())).ReturnsAsync(unRefreshedShipment.ShipmentID);
            sqlAdapterFactory.Setup(f => f.Create()).Returns(sqlAdapter.Object);

            shippingManager
                .Setup(a => a.RefreshShipment(It.IsAny<ShipmentEntity>()))
                .Callback((ShipmentEntity s) =>
                {
                    s.Processed = true;
                    s.Voided = false;
                });

            testObject = mock.Create<PreviousShipmentActionManager>();
            await testObject.ReprintLastShipment().ConfigureAwait(false);

            messenger.Verify(m => m.Send(It.IsAny<ReprintLabelsMessage>(), string.Empty), Times.Once);
        }

        [Fact]
        public async Task ReprintLastShipment_DoesNotSendMessage_WhenShipmentIsDeleted()
        {
            ShipmentEntity unRefreshedShipment = new ShipmentEntity(3) {DeletedFromDatabase = true};

            sqlAdapter.Setup(a => a.FetchFirstAsync(It.IsAny<DynamicQuery<long?>>())).ReturnsAsync(unRefreshedShipment.ShipmentID);
            sqlAdapterFactory.Setup(f => f.Create()).Returns(sqlAdapter.Object);

            shippingManager
                .Setup(a => a.RefreshShipment(It.IsAny<ShipmentEntity>()))
                .Throws<ObjectDeletedException>();

            testObject = mock.Create<PreviousShipmentActionManager>();
            await testObject.ReprintLastShipment().ConfigureAwait(false);

            messenger.Verify(m => m.Send(It.IsAny<ReprintLabelsMessage>(), string.Empty), Times.Never);
        }

        [Fact]
        public async Task ReprintLastShipment_DoesNotSendReprintLabelsMessage_WhenShipmentHasBeenDeleted()
        {
            sqlAdapter.Setup(a => a.FetchFirstAsync(It.IsAny<DynamicQuery<long>>())).ReturnsAsync(null);
            sqlAdapterFactory.Setup(f => f.Create()).Returns(sqlAdapter.Object);

            testObject = mock.Create<PreviousShipmentActionManager>();
            await testObject.ReprintLastShipment().ConfigureAwait(false);

            messenger.Verify(m => m.Send(It.IsAny<ReprintLabelsMessage>(), string.Empty), Times.Never);
        }
    }
}
