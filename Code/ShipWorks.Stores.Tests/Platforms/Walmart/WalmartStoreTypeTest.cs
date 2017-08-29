using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Actions.Tasks;
using ShipWorks.ApplicationCore;
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
        public void CreateOrderIdentifier_ReturnsWalmartOrderIdentifier()
        {
            WalmartStoreEntity store = new WalmartStoreEntity();
            store.TypeCode = (int) StoreTypeCode.Walmart;

            WalmartStoreType testObject = mock.Create<WalmartStoreType>(new TypedParameter(typeof(StoreEntity), store));
            OrderEntity order = new WalmartOrderEntity() { PurchaseOrderID = "7" };
            WalmartOrderIdentifier identifier = new WalmartOrderIdentifier("7");

            Assert.Equal(identifier.ToString(), testObject.CreateOrderIdentifier(order).ToString());
        }

        [Fact]
        public void CreateAddStoreWizardOnlineUpdateActionControl_ReturnsOnlineUpdateShipmentUpdateActionControlWithWalmartTaskType()
        {
            IoC.Initialize(mock.Container);
            mock.Provide(mock.Create<WalmartShipmentUploadTask>());

            WalmartStoreEntity store = new WalmartStoreEntity();
            store.TypeCode = (int) StoreTypeCode.Walmart;

            WalmartStoreType testObject = mock.Create<WalmartStoreType>(new TypedParameter(typeof(StoreEntity), store));

            var onlineUpdateActionControl = testObject.CreateAddStoreWizardOnlineUpdateActionControl();

            ActionTask actionTask = onlineUpdateActionControl.CreateActionTasks(mock.Container, store).FirstOrDefault();

            Assert.Equal(typeof(WalmartShipmentUploadTask), actionTask.GetType());
        }
    }
}