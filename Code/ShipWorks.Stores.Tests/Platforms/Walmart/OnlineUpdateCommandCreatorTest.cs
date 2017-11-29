using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Walmart.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class OnlineUpdateCommandCreatorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IMenuCommandExecutionContext> menuContext;

        public OnlineUpdateCommandCreatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            menuContext = mock.Mock<IMenuCommandExecutionContext>();
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { 42L });
        }

        [Fact]
        public async Task OnUploadShipmentDetails_DelegatesToOrderManager_GetLatestActiveShipment()
        {
            var testObject = mock.Create<WalmartOnlineUpdateInstanceCommands>();

            await testObject.OnUploadShipmentDetails(null, menuContext.Object);

            mock.Mock<IOrderManager>().Verify(o => o.GetLatestActiveShipmentAsync(42L), Times.Once);
        }

        [Fact]
        public async Task OnUploadShipmentDetails_DoesNotUpdateShipmentDetails_WhenNoLatestActiveShipment()
        {
            var testObject = mock.Create<WalmartOnlineUpdateInstanceCommands>();

            await testObject.OnUploadShipmentDetails(null, menuContext.Object);

            mock.Mock<IShipmentDetailsUpdater>().Verify(u => u.UpdateShipmentDetails(It.IsAny<IWalmartStoreEntity>(), It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public async Task OnUploadShipmentDetails_UploadsShipmentDetails_WhenActiveShipmentExists()
        {
            var shipment = new ShipmentEntity();
            mock.Mock<IOrderManager>()
                .Setup(o => o.GetLatestActiveShipmentAsync(42L))
                .ReturnsAsync(shipment);

            var testObject = mock.Create<WalmartOnlineUpdateInstanceCommands>();

            await testObject.OnUploadShipmentDetails(null, menuContext.Object);

            mock.Mock<IShipmentDetailsUpdater>().Verify(u => u.UpdateShipmentDetails(It.IsAny<IWalmartStoreEntity>(), shipment), Times.Once);
        }

        [Fact]
        public async Task OnUploadShipmentDetails_UploadsShipmentDetails_DelegatesToEnsureShipmentLoadedAsync()
        {
            var shipment = new ShipmentEntity();
            mock.Mock<IOrderManager>()
                .Setup(o => o.GetLatestActiveShipmentAsync(42L))
                .ReturnsAsync(shipment);

            var testObject = mock.Create<WalmartOnlineUpdateInstanceCommands>();

            await testObject.OnUploadShipmentDetails(null, menuContext.Object);

            mock.Mock<IShippingManager>().Verify(u => u.EnsureShipmentLoadedAsync(shipment), Times.Once);
        }

        [Fact]
        public async Task OnUploadShipmentDetails_CompletesWithSuccess_WhenNoError()
        {
            var shipment = new ShipmentEntity();
            mock.Mock<IOrderManager>()
                .Setup(o => o.GetLatestActiveShipmentAsync(42L))
                .ReturnsAsync(shipment);

            var testObject = mock.Create<WalmartOnlineUpdateInstanceCommands>();

            await testObject.OnUploadShipmentDetails(null, menuContext.Object);

            menuContext.Verify(c => c.Complete(It.Is<IEnumerable<Exception>>(exceptions => exceptions.None()), MenuCommandResult.Error));
        }

        [Fact]
        public async Task OnUploadShipmentDetails_CompletesWithFailure_WhenWalmartExceptionThrown()
        {
            var walmartException = new WalmartException();
            mock.Mock<IOrderManager>()
                .Setup(o => o.GetLatestActiveShipmentAsync(42L))
                .ThrowsAsync(walmartException);

            var testObject = mock.Create<WalmartOnlineUpdateInstanceCommands>();

            await testObject.OnUploadShipmentDetails(null, menuContext.Object);

            menuContext.Verify(c => c.Complete(It.Is<IEnumerable<Exception>>(exceptions => exceptions.Single() == walmartException), MenuCommandResult.Error));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
