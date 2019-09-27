using System;
using System.Collections.Generic;
using System.Threading;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps
{
    public class ShipmentPreparationStepTest : IDisposable
    {
        readonly ShipmentEntity shipment;
        readonly AutoMock mock;
        readonly ShipmentPreparationStep testObject;
        private ProcessShipmentState defaultInput;
        private readonly Mock<IScanPackOrderValidator> scanPackOrderValidator;

        public ShipmentPreparationStepTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ShipmentPreparationStep>();
            shipment = Create.Shipment(new OrderEntity()).AsBestRate().Set(x => x.ShipmentID = 2031).Build();
            defaultInput = new ProcessShipmentState(0, shipment, new Dictionary<long, Exception>(), null, new CancellationTokenSource());

            mock.Mock<IShippingManager>()
                .Setup(x => x.ValidateLicense(It.IsAny<StoreEntity>(), It.IsAny<IDictionary<long, Exception>>()))
                .Returns<Exception>(null);

            scanPackOrderValidator = mock.Mock<IScanPackOrderValidator>();
            scanPackOrderValidator.Setup(v => v.CanProcessShipment(It.IsAny<OrderEntity>()))
                .Returns(Result.FromSuccess());
        }

        [Fact]
        public void PrepareShipment_DelegatesToScanPackOrderValidatorWithShipmentsOrder()
        {
            StoreEntity store = new StoreEntity() { Enabled = true };
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(It.IsAny<long>())).Returns(store);

            var result = testObject.PrepareShipment(defaultInput);
            scanPackOrderValidator.Verify(s => s.CanProcessShipment(defaultInput.OriginalShipment.Order));
        }

        [Fact]
        public void PrepareShipment_ReturnsExceptionResult_WhenInputWasNotSuccessful()
        {
            var input = new ProcessShipmentState(0, new ShipmentEntity(), new ShippingException(), new CancellationTokenSource());

            var result = testObject.PrepareShipment(input);

            Assert.False(result.Success);
            Assert.Equal(input.Exception, result.Exception);
            Assert.Equal(input.OriginalShipment, result.OriginalShipment);
        }

        [Fact]
        public void PrepareShipment_ReturnsExceptionResult_WhenPermissionCheckFails()
        {
            mock.Mock<ISecurityContext>()
                .Setup(x => x.DemandPermission(PermissionType.ShipmentsCreateEditProcess, 2031))
                .Throws<PermissionException>();

            var result = testObject.PrepareShipment(defaultInput);

            Assert.IsType<PermissionException>(result.Exception.InnerException);
        }

        [Fact]
        public void PrepareShipment_ReturnsException_WhenEntityLockCouldNotBeSecured()
        {
            mock.Mock<IResourceLockFactory>()
                .Setup(x => x.GetEntityLock(2031, "Process Shipment"))
                .Throws(new SqlAppResourceLockException("Foo"));

            var result = testObject.PrepareShipment(defaultInput);

            Assert.IsType<SqlAppResourceLockException>(result.Exception.InnerException);
        }

        [Fact]
        public void PrepareShipment_DelegatesToShippingManager()
        {
            var result = testObject.PrepareShipment(defaultInput);

            mock.Mock<IShippingManager>()
                .Verify(x => x.EnsureShipmentIsLoadedWithOrder(shipment));
        }

        [Fact]
        public void PrepareShipment_ReturnsException_WhenShipmentIsAlreadyProcessed()
        {
            shipment.Processed = true;

            var result = testObject.PrepareShipment(defaultInput);

            Assert.IsType<ShipmentAlreadyProcessedException>(result.Exception);
            Assert.Contains("already been processed", result.Exception.Message);
        }

        [Fact]
        public void PrepareShipment_ReturnsException_WhenUsingInsureShipAndNoItemNames()
        {
            StoreEntity store = new StoreEntity() { Enabled = true };
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(It.IsAny<long>())).Returns(store);

            shipment.Insurance = true;
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;

            var result = testObject.PrepareShipment(defaultInput);

            Assert.IsType<ShippingException>(result.Exception);
            Assert.Contains("insure", result.Exception.Message);
        }

        [Fact]
        public void PrepareShipment_ReturnsException_WhenOrderDoesNotExist()
        {
            shipment.Order = null;

            var result = testObject.PrepareShipment(defaultInput);

            Assert.IsType<ShippingException>(result.Exception);
            Assert.Contains("order", result.Exception.Message);
        }

        [Fact]
        public void PrepareShipment_ReturnsException_WhenStoreDoesNotExist()
        {
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(1234)).Returns<StoreEntity>(null);
            shipment.Order.StoreID = 1234;

            var result = testObject.PrepareShipment(defaultInput);

            Assert.IsType<ShippingException>(result.Exception);
            Assert.Contains("store", result.Exception.Message);
        }

        [Fact]
        public void PrepareShipment_ReturnsException_WhenValidateLicenseReturnsError()
        {
            StoreEntity store = new StoreEntity() { Enabled = true };
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(It.IsAny<long>())).Returns(store);
            mock.Mock<IShippingManager>()
                .Setup(x => x.ValidateLicense(store, defaultInput.LicenseCheckCache))
                .Returns(new Exception("Foo"));

            var result = testObject.PrepareShipment(defaultInput);

            Assert.IsType<ShippingException>(result.Exception);
            Assert.Contains("Foo", result.Exception.Message);
        }

        [Fact]
        public void PrepareShipment_DelegatesToPreProcessor_WhenShipmentIsValid()
        {
            StoreEntity store = new StoreEntity() { Enabled = true };
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(It.IsAny<long>())).Returns(store);

            RateResult rate = new RateResult("Foo", "1");
            Action callback = () => { };

            ProcessShipmentState state = new ProcessShipmentState(0, shipment, new Dictionary<long, Exception>(), rate, new CancellationTokenSource());
            testObject.CounterRateCarrierConfiguredWhileProcessing = callback;

            Mock<IShipmentPreProcessor> preProcessor = mock.CreateMock<IShipmentPreProcessor>();
            mock.Mock<IShipmentPreProcessorFactory>()
                .Setup(x => x.Create(ShipmentTypeCode.BestRate)).Returns(preProcessor);

            var result = testObject.PrepareShipment(state);

            preProcessor.Verify(x => x.Run(shipment, rate, callback));
        }

        [Fact]
        public void PrepareShipment_ReturnsCanceledException_WhenPreprocessorReturnsNull()
        {
            StoreEntity store = new StoreEntity() { Enabled = true };
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(It.IsAny<long>())).Returns(store);

            Mock<IShipmentPreProcessor> preProcessor = mock.CreateMock<IShipmentPreProcessor>(s =>
                s.Setup(x => x.Run(It.IsAny<ShipmentEntity>(), It.IsAny<RateResult>(), It.IsAny<Action>()))
                    .Returns<IEnumerable<ShipmentEntity>>(null));
            mock.Mock<IShipmentPreProcessorFactory>()
                .Setup(x => x.Create(It.IsAny<ShipmentTypeCode>())).Returns(preProcessor);

            var result = testObject.PrepareShipment(defaultInput);

            Assert.IsType<ShippingException>(result.Exception);
            Assert.Contains("cancel", result.Exception.Message);
        }

        [Fact]
        public void PrepareShipment_ReturnsPreprocessedShipments_WhenPreprocessorSucceeds()
        {
            StoreEntity store = new StoreEntity() { Enabled = true };
            mock.Mock<IStoreManager>().Setup(x => x.GetStore(It.IsAny<long>())).Returns(store);

            List<ShipmentEntity> shipments = new List<ShipmentEntity>();
            Mock<IShipmentPreProcessor> preProcessor = mock.CreateMock<IShipmentPreProcessor>(s =>
                s.Setup(x => x.Run(It.IsAny<ShipmentEntity>(), It.IsAny<RateResult>(), It.IsAny<Action>()))
                    .Returns(shipments));
            mock.Mock<IShipmentPreProcessorFactory>()
                .Setup(x => x.Create(It.IsAny<ShipmentTypeCode>())).Returns(preProcessor);

            var result = testObject.PrepareShipment(defaultInput);

            Assert.Equal(shipments, result.Shipments);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
