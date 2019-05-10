using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps
{
    public class LabelResultLogStepTest : IDisposable
    {
        private readonly AutoMock mock;
        private Tuple<ILabelPersistenceResult, ILabelPersistenceResult> input;
        private readonly Mock<ILabelPersistenceResult> persistenceResult1;
        private readonly Mock<ILabelPersistenceResult> persistenceResult2;
        private readonly ShipmentEntity shipment;
        private readonly ShipmentEntity shipmentForTango;
        private readonly StoreEntity store;
        private readonly Shipping.Services.ShipmentProcessorSteps.LabelResultLogStep testObject;

        public LabelResultLogStepTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment(new OrderEntity()).AsOther().Set(x => x.ShipmentID, 1234).Build();
            shipmentForTango = Create.Shipment(new OrderEntity()).AsOther().Set(x => x.ShipmentID, 5678).Build();
            store = Create.Store<StoreEntity>().Build();

            persistenceResult1 = mock.Mock<ILabelPersistenceResult>();
            persistenceResult1.SetupGet(x => x.OriginalShipment).Returns(shipment);
            persistenceResult1.SetupGet(x => x.ShipmentForTango).Returns(shipmentForTango);
            persistenceResult1.SetupGet(x => x.Store).Returns(store);
            persistenceResult1.SetupGet(x => x.Success).Returns(true);

            persistenceResult2 = mock.Mock<ILabelPersistenceResult>();
            persistenceResult2.SetupGet(x => x.OriginalShipment).Returns(shipment);
            persistenceResult2.SetupGet(x => x.ShipmentForTango).Returns(shipmentForTango);
            persistenceResult2.SetupGet(x => x.Store).Returns(store);
            persistenceResult2.SetupGet(x => x.Success).Returns(true);

            input = new Tuple<ILabelPersistenceResult, ILabelPersistenceResult>(persistenceResult1.Object, persistenceResult2.Object);

            testObject = mock.Create<Shipping.Services.ShipmentProcessorSteps.LabelResultLogStep>();
        }

        [Fact]
        public void Finish_ClearsErrors_ForShipment()
        {
            testObject.Complete(input);

            mock.Mock<IShippingErrorManager>().Verify(x => x.Remove(1234));
        }

        [Fact]
        public void Finish_LogsShipmentToTango_WhenShipmentTypeDoesNotFinishExternally()
        {
            testObject.Complete(input);

            mock.Mock<ITangoLogShipmentProcessor>().Verify(x => x.Add(store, shipmentForTango));
        }

        [Fact]
        public void Finish_SavesShipmentToDatabase_WhenShipmentTypeDoesNotFinishExternally()
        {
            var sqlAdapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.Create()).Returns(sqlAdapter);

            testObject.Complete(input);

            sqlAdapter.Verify(x => x.SaveAndRefetch(shipment), Times.Never);
            sqlAdapter.Verify(x => x.Commit(), Times.Never);
        }

        [Theory]
        [InlineData(typeof(ORMConcurrencyException))]
        [InlineData(typeof(ObjectDeletedException))]
        [InlineData(typeof(SqlForeignKeyException))]
        public void Finish_SetsError_WhenInputWasNotSuccessful(Type exceptionType)
        {
            var exception = CreateException(exceptionType);
            persistenceResult1.SetupGet(x => x.Exception).Returns(exception);
            persistenceResult1.SetupGet(x => x.Success).Returns(false);

            testObject.Complete(input);

            mock.Mock<IShippingErrorManager>().Verify(x => x.SetShipmentErrorMessage(1234, exception, "processed"));
        }

        [Fact]
        public void Finish_SetsError_WhenInputHasShippingException()
        {
            var exception = new ShippingException();
            persistenceResult1.SetupGet(x => x.Exception).Returns(exception);
            persistenceResult1.SetupGet(x => x.Success).Returns(false);

            testObject.Complete(input);

            mock.Mock<IShippingErrorManager>().Verify(x => x.SetShipmentErrorMessage(1234, exception));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Finish_ReturnsInsufficientFundsException_WhenInputExceptionContainsInsufficientFundsException(int levels)
        {
            Exception outOfFundsException = new UspsInsufficientFundsException(new UspsAccountEntity(), "Foo");
            Exception exception = outOfFundsException;
            for (int i = 0; i < levels; i++)
            {
                exception = new Exception($"Level {i}", exception);
            }

            persistenceResult1.SetupGet(x => x.Exception).Returns(new ShippingException("Foo", exception));
            persistenceResult1.SetupGet(x => x.Success).Returns(false);

            var result = testObject.Complete(input);

            Assert.Equal<object>(outOfFundsException, result.Item1.OutOfFundsException);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Finish_ReturnsTermsAndConditionsException_WhenInputExceptionContainsTermsAndConditionsException(int levels)
        {
            Exception termsException = new UspsTermsAndConditionsException("Foo", null);
            Exception exception = termsException;
            for (int i = 0; i < levels; i++)
            {
                exception = new Exception($"Level {i}", exception);
            }

            persistenceResult1.SetupGet(x => x.Exception).Returns(new ShippingException("Foo", exception));
            persistenceResult1.SetupGet(x => x.Success).Returns(false);

            var result = testObject.Complete(input);

            Assert.Equal<object>(termsException, result.Item1.TermsAndConditionsException);
        }

        [Fact]
        public void Finish_DisposesEntityLock_IfSet()
        {
            Mock<IDisposable> entityLock = mock.Mock<IDisposable>();
            persistenceResult1.SetupGet(x => x.EntityLock).Returns(entityLock);

            testObject.Complete(input);

            entityLock.Verify(x => x.Dispose());
        }

        [Fact]
        public void Finish_DelegatesToKnowledgebase()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager(s =>
                s.SetupGet(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Other));
            var result = testObject.Complete(input);

            mock.Mock<IKnowledgebase>().Verify(x => x.LogShipment(shipmentType.Object, shipment));
        }

        [Theory]
        [InlineData(ShipmentTypeCode.UpsWorldShip, true)]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, false)]
        public void Finish_SetsWorldShipStatus_Correctly(ShipmentTypeCode shipmentType, bool expected)
        {
            var newShipment = Modify.Shipment(shipment)
                .AsUps(x => x.WithPackage())
                .Set(x => x.ShipmentTypeCode = shipmentType)
                .Build();
            persistenceResult1.SetupGet(x => x.OriginalShipment).Returns(newShipment);

            var result = testObject.Complete(input);

            Assert.Equal(expected, result.Item1.WorldshipExported);
        }

        [Fact]
        public void Finish_DelegatesToShippingManager_ToRefreshShipment()
        {
            testObject.Complete(input);

            mock.Mock<IShippingManager>().Verify(x => x.RefreshShipment(shipment));
        }

        [Fact]
        public void Finish_SetsErrorMessage_WhenRefreshShipmentFailsAndThereIsNoExistingError()
        {
            mock.Mock<IShippingManager>()
                .Setup(x => x.RefreshShipment(It.IsAny<ShipmentEntity>()))
                .Throws<ObjectDeletedException>();

            var result = testObject.Complete(input);

            mock.Mock<IShippingErrorManager>().Verify(x => x.SetShipmentErrorMessage(1234, It.IsAny<Exception>()));
            Assert.Contains("deleted", result.Item1.ErrorMessage);
        }

        [Fact]
        public void Finish_DoesNotSetErrorMessage_WhenRefreshShipmentFailsAndThereIsAnExistingError()
        {
            mock.Mock<IShippingManager>()
                .Setup(x => x.RefreshShipment(It.IsAny<ShipmentEntity>()))
                .Throws<ObjectDeletedException>();

            persistenceResult1.Setup(x => x.Exception).Returns(new Exception("Foo"));
            persistenceResult1.Setup(x => x.Success).Returns(false);

            var result = testObject.Complete(input);

            mock.Mock<IShippingErrorManager>()
                .Verify(x => x.SetShipmentErrorMessage(It.IsAny<long>(), It.IsAny<Exception>()), Times.Never);
            Assert.DoesNotContain("deleted", result.Item1.ErrorMessage);
        }

        private Exception CreateException(Type exceptionType)
        {
            if (exceptionType == typeof(ORMConcurrencyException)) return new ORMConcurrencyException("Foo", shipment);
            if (exceptionType == typeof(ObjectDeletedException)) return new ObjectDeletedException();
            if (exceptionType == typeof(SqlForeignKeyException)) return new SqlForeignKeyException();
            return new Exception();
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
