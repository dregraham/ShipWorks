using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Api.Orders.Shipments;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Tests.Shared;
using ShipWorks.UI;
using Xunit;

namespace ShipWorks.Api.Tests.Orders.Shipments
{
    public class ApiShipmentProcessorTest
    {
        private readonly AutoMock mock;
        private readonly ApiShipmentProcessor testObject;
        private readonly Mock<IShipmentProcessor> shipmentProcessor;

        public ApiShipmentProcessorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentProcessor = mock.Mock<IShipmentProcessor>();

            testObject = mock.Create<ApiShipmentProcessor>();

            mock.Provide(shipmentProcessor.Object);
        }

        [Fact]
        public async Task Process_OverridesRegistraionForUIDependedComponents()
        {
            mock.Container.ChildLifetimeScopeBeginning += (sender, args) =>
            {
                Assert.Equal(typeof(BackgroundAsyncMessageHelper), args.LifetimeScope.Resolve<IAsyncMessageHelper>().GetType());
                Assert.Equal(typeof(NullNudgeManager), args.LifetimeScope.Resolve<INudgeManager>().GetType());
            };

            var shipment = new ShipmentEntity();
            
            await testObject.Process(shipment);
        }

        [Fact]
        public async Task Process_DelegatesToShipmentProcessorWithShipment()
        {
            mock.Container.ChildLifetimeScopeBeginning += (sender, args) =>
            {
                Assert.Equal(typeof(BackgroundAsyncMessageHelper), args.LifetimeScope.Resolve<IAsyncMessageHelper>().GetType());
                Assert.Equal(typeof(NullNudgeManager), args.LifetimeScope.Resolve<INudgeManager>().GetType());
            };

            var shipment = new ShipmentEntity();

            await testObject.Process(shipment);

            shipmentProcessor.Verify(s => s.Process(new[] { shipment }, It.IsAny<ICarrierConfigurationShipmentRefresher>(), null, null));
        }

        [Fact]
        public async Task Process_ReturnsFirstProcessShipmentResult_WhenResultsAreNotEmpty()
        {
            mock.Container.ChildLifetimeScopeBeginning += (sender, args) =>
            {
                Assert.Equal(typeof(BackgroundAsyncMessageHelper), args.LifetimeScope.Resolve<IAsyncMessageHelper>().GetType());
                Assert.Equal(typeof(NullNudgeManager), args.LifetimeScope.Resolve<INudgeManager>().GetType());
            };

            var shipment = new ShipmentEntity();

            var processResult = new[] { new ProcessShipmentResult(shipment), new ProcessShipmentResult() };

            shipmentProcessor.Setup(s => s.Process(new[] { shipment }, It.IsAny<ICarrierConfigurationShipmentRefresher>(), null, null))
                .ReturnsAsync(processResult);

            var result = await testObject.Process(shipment);

            Assert.Equal(processResult.First(), result);
        }

        [Fact]
        public async Task Process_ReturnsDefault_WhenResultsAreEmpty()
        {
            mock.Container.ChildLifetimeScopeBeginning += (sender, args) =>
            {
                Assert.Equal(typeof(BackgroundAsyncMessageHelper), args.LifetimeScope.Resolve<IAsyncMessageHelper>().GetType());
                Assert.Equal(typeof(NullNudgeManager), args.LifetimeScope.Resolve<INudgeManager>().GetType());
            };

            var shipment = new ShipmentEntity();
            
            shipmentProcessor.Setup(s => s.Process(new[] { shipment }, It.IsAny<ICarrierConfigurationShipmentRefresher>(), null, null))
                .ReturnsAsync(Enumerable.Empty<ProcessShipmentResult>());

            var result = await testObject.Process(shipment);

            Assert.Equal(default, result);
        }
    }
}
