using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Actions;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartStoreTypeTest
    {
        private readonly AutoMock mock;

        public WalmartStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsOrderNumberIdentifier()
        {
            WalmartStoreEntity store = new WalmartStoreEntity();
            store.TypeCode = (int) StoreTypeCode.Walmart;

            WalmartStoreType testObject = mock.Create<WalmartStoreType>(new TypedParameter(typeof(StoreEntity), store));
            OrderEntity order = new OrderEntity() { OrderNumber = 7 };
            OrderNumberIdentifier identifier = new OrderNumberIdentifier(7);

            Assert.Equal(identifier.ToString(), testObject.CreateOrderIdentifier(order).ToString());
        }

        [Fact]
        public void CreateAddStoreWizardOnlineUpdateActionControl_ReturnsOnlineUpdateShipmentUpdateActionControlWithWalmartTaskType()
        {
            WalmartStoreEntity store = new WalmartStoreEntity();
            store.TypeCode = (int) StoreTypeCode.Walmart;

            WalmartStoreType testObject = mock.Create<WalmartStoreType>(new TypedParameter(typeof(StoreEntity), store));

            var onlineUpdateActionControl = testObject.CreateAddStoreWizardOnlineUpdateActionControl();

            var scope = mock.CreateMock<ILifetimeScope>();
            scope.Setup(x => x.Resolve(typeof(WalmartShipmentUploadTask)))
                .Returns(new WalmartShipmentUploadTask());

            ActionTask actionTask = onlineUpdateActionControl.CreateActionTasks(scope.Object, store).FirstOrDefault();

            Assert.Equal(typeof(WalmartShipmentUploadTask), actionTask.GetType());
        }

        [Fact]
        public void CreateOnlineUpdateInstanceCommands_DelegatesToWalmartOnlineUpdateInstanceCommandsFactory()
        {
            WalmartStoreEntity store = new WalmartStoreEntity();
            store.TypeCode = (int) StoreTypeCode.Walmart;

            var commandFactory = mock.Mock<IWalmartOnlineUpdateInstanceCommands>();
            mock.MockFunc<WalmartStoreEntity, IWalmartOnlineUpdateInstanceCommands>(commandFactory);

            WalmartStoreType testObject = mock.Create<WalmartStoreType>(new TypedParameter(typeof(StoreEntity), store));

            testObject.CreateOnlineUpdateInstanceCommands();

            commandFactory.Verify(x => x.Create(), Times.Once);
        }
    }
}