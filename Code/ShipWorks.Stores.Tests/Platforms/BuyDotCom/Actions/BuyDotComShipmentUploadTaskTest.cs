using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BuyDotCom.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Platforms.BuyDotCom.Actions
{
    public class BuyDotComShipmentUploadTaskTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IActionStepContext> actionContext;

        public BuyDotComShipmentUploadTaskTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            mock.Mock<IStoreManager>()
                .Setup(x => x.GetStore(AnyLong))
                .Returns(new BuyDotComStoreEntity());

            actionContext = mock.Mock<IActionStepContext>();
        }

        [Fact]
        public async Task RunAsync_ThrowsArgumentNullException_WhenContextIsNull()
        {
            var testObject = mock.Create<BuyDotComShipmentUploadTask>();
            testObject.StoreID = 1006;

            await Assert.ThrowsAsync<ArgumentNullException>(() => testObject.RunAsync(new List<long> { 1031 }, null));
        }

        [Fact]
        public async Task RunAsync_ThrowsActionTaskRunException_WhenStoreIDIsNotSet()
        {
            var testObject = mock.Create<BuyDotComShipmentUploadTask>();

            await Assert.ThrowsAsync<ActionTaskRunException>(() => testObject.RunAsync(new List<long> { 1031 }, actionContext.Object));
        }

        [Fact]
        public async Task RunAsync_DelegatesToStoreManager()
        {
            var testObject = mock.Create<BuyDotComShipmentUploadTask>();
            testObject.StoreID = 1006;

            await testObject.RunAsync(new List<long> { 1031 }, actionContext.Object);

            mock.Mock<IStoreManager>().Verify(x => x.GetStore(1006L));
        }

        [Fact]
        public async Task RunAsync_ThrowsActionTaskRunException_WhenStoreIsNotFound()
        {
            var testObject = mock.Create<BuyDotComShipmentUploadTask>();
            testObject.StoreID = 1006;

            mock.Mock<IStoreManager>().Setup(x => x.GetStore(AnyLong)).Returns((StoreEntity) null);

            await Assert.ThrowsAsync<ActionTaskRunException>(() => testObject.RunAsync(new List<long> { 1031 }, actionContext.Object));
        }

        [Fact]
        public async Task RunAsync_ThrowsActionTaskRunException_WhenStoreIsNotButDotCom()
        {
            mock.Mock<IStoreManager>()
                   .Setup(x => x.GetStore(AnyLong))
                   .Returns(new StoreEntity());
            var testObject = mock.Create<BuyDotComShipmentUploadTask>();
            testObject.StoreID = 1006;

            await Assert.ThrowsAsync<ActionTaskRunException>(() => testObject.RunAsync(new List<long> { 1031 }, actionContext.Object));
        }

        [Fact]
        public async Task RunAsync_Postpones_WhenCanPostponeAndIsUnderLimit()
        {
            actionContext.Setup(x => x.GetPostponedData()).Returns(new[] { new List<long> { 2031, 3031 } });
            actionContext.SetupGet(x => x.CanPostpone).Returns(true);

            var testObject = mock.Create<BuyDotComShipmentUploadTask>();
            testObject.StoreID = 1006;

            await testObject.RunAsync(new List<long> { 1031 }, actionContext.Object);

            actionContext.Verify(x => x.Postpone(UnorderedEnumerable(1031L)));
        }

        [Fact]
        public async Task RunAsync_DoesNotPostpone_WhenCanPostponeButIsOverLimit()
        {
            actionContext.Setup(x => x.GetPostponedData()).Returns(new[] { Enumerable.Range(1, 300).Select(Convert.ToInt64).ToList() });
            actionContext.SetupGet(x => x.CanPostpone).Returns(true);

            var testObject = mock.Create<BuyDotComShipmentUploadTask>();
            testObject.StoreID = 1006;

            await testObject.RunAsync(new List<long> { 1031 }, actionContext.Object);

            actionContext.Verify(x => x.Postpone(It.IsAny<IEnumerable<long>>()), Times.Never);
        }

        [Fact]
        public async Task RunAsync_DelegatesToShipmentDetailsUpdater()
        {
            var testObject = mock.Create<BuyDotComShipmentUploadTask>();
            testObject.StoreID = 1006;

            await testObject.RunAsync(new List<long> { 1031 }, actionContext.Object);

            mock.Mock<IBuyDotComShipmentDetailsUpdater>()
                .Verify(x => x.UploadShipmentDetailsForShipments(It.IsAny<IBuyDotComStoreEntity>(), new[] { 1031L }));
        }

        [Fact]
        public async Task RunAsync_DelegatesToShipmentDetailsUpdater_WithPostponedData()
        {
            actionContext.Setup(x => x.GetPostponedData()).Returns(new[] { new List<long> { 2031, 3031 }, new List<long> { 5031 } });
            var testObject = mock.Create<BuyDotComShipmentUploadTask>();
            testObject.StoreID = 1006;

            await testObject.RunAsync(new List<long> { 1031 }, actionContext.Object);

            mock.Mock<IBuyDotComShipmentDetailsUpdater>()
                .Verify(x => x.UploadShipmentDetailsForShipments(
                    It.IsAny<IBuyDotComStoreEntity>(),
                    UnorderedEnumerable(1031L, 2031L, 3031L, 5031L)));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
