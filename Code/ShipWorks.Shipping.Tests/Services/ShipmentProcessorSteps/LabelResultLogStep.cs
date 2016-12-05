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
    public class LabelResultLogStep : IDisposable
    {
        readonly AutoMock mock;
        Mock<ILabelPersistenceResult> input;
        ShipmentEntity shipment;
        StoreEntity store;
        private readonly Shipping.Services.ShipmentProcessorSteps.LabelResultLogStep testObject;

        public LabelResultLogStep()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment(new OrderEntity()).AsOther().Set(x => x.ShipmentID, 1234).Build();
            store = Create.Store<StoreEntity>().Build();

            input = mock.Mock<ILabelPersistenceResult>();
            input.SetupGet(x => x.OriginalShipment).Returns(shipment);
            input.SetupGet(x => x.Store).Returns(store);
            input.SetupGet(x => x.Success).Returns(true);

            testObject = mock.Create<Shipping.Services.ShipmentProcessorSteps.LabelResultLogStep>();
        }

        [Fact]
        public void Finish_ClearsErrors_ForShipment()
        {
            testObject.Complete(input.Object);

            mock.Mock<IShippingErrorManager>().Verify(x => x.Remove(1234));
        }

        [Fact]
        public void Finish_LogsShipmentToTango_WhenShipmentTypeDoesNotFinishExternally()
        {
            testObject.Complete(input.Object);

            mock.Mock<ITangoWebClient>().Verify(x => x.LogShipment(store, shipment, false));
        }

        [Fact]
        public void Finish_SavesShipmentToDatabase_WhenShipmentTypeDoesNotFinishExternally()
        {
            var sqlAdapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.Create()).Returns(sqlAdapter);

            testObject.Complete(input.Object);

            sqlAdapter.Verify(x => x.SaveAndRefetch(shipment));
            sqlAdapter.Verify(x => x.Commit());
        }

        [Theory]
        [InlineData(typeof(ORMConcurrencyException))]
        [InlineData(typeof(ObjectDeletedException))]
        [InlineData(typeof(SqlForeignKeyException))]
        public void Finish_SetsError_WhenInputWasNotSuccessful(Type exceptionType)
        {
            var exception = CreateException(exceptionType);
            input.SetupGet(x => x.Exception).Returns(exception);
            input.SetupGet(x => x.Success).Returns(false);

            testObject.Complete(input.Object);

            mock.Mock<IShippingErrorManager>().Verify(x => x.SetShipmentErrorMessage(1234, exception, "processed"));
        }

        [Theory]
        [InlineData(typeof(ORMConcurrencyException))]
        [InlineData(typeof(ObjectDeletedException))]
        [InlineData(typeof(SqlForeignKeyException))]
        public void Finish_SetsError_WhenSavingThrows(Type exceptionType)
        {
            var exception = CreateException(exceptionType);

            var sqlAdapter = mock.CreateMock<ISqlAdapter>(a => a.Setup(x => x.Commit()).Throws(exception));
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.Create()).Returns(sqlAdapter);

            testObject.Complete(input.Object);

            mock.Mock<IShippingErrorManager>().Verify(x => x.SetShipmentErrorMessage(1234, exception, "processed"));
        }

        [Fact]
        public void Finish_SetsError_WhenInputHasShippingException()
        {
            var exception = new ShippingException();
            input.SetupGet(x => x.Exception).Returns(exception);
            input.SetupGet(x => x.Success).Returns(false);

            testObject.Complete(input.Object);

            mock.Mock<IShippingErrorManager>().Verify(x => x.SetShipmentErrorMessage(1234, exception));
        }

        [Fact]
        public void Finish_SetsError_WhenFinishingThrowsShippingException()
        {
            var exception = new ShippingException();

            var sqlAdapter = mock.CreateMock<ISqlAdapter>(a => a.Setup(x => x.Commit()).Throws(exception));
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.Create()).Returns(sqlAdapter);

            testObject.Complete(input.Object);

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

            input.SetupGet(x => x.Exception).Returns(new ShippingException("Foo", exception));
            input.SetupGet(x => x.Success).Returns(false);

            var result = testObject.Complete(input.Object);

            Assert.Equal<object>(outOfFundsException, result.OutOfFundsException);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Finish_ReturnsTermsAndConditionsException_WhenInputExceptionContainsTermsAndConditionsException(int levels)
        {
            Exception termsException = new UspsGlobalPostTermsAndConditionsException(new UspsAccountEntity(), "Foo");
            Exception exception = termsException;
            for (int i = 0; i < levels; i++)
            {
                exception = new Exception($"Level {i}", exception);
            }

            input.SetupGet(x => x.Exception).Returns(new ShippingException("Foo", exception));
            input.SetupGet(x => x.Success).Returns(false);

            var result = testObject.Complete(input.Object);

            Assert.Equal<object>(termsException, result.TermsAndConditionsException);
        }

        [Fact]
        public void Finish_DisposesEntityLock_IfSet()
        {
            Mock<IDisposable> entityLock = mock.Mock<IDisposable>();
            input.SetupGet(x => x.EntityLock).Returns(entityLock);

            testObject.Complete(input.Object);

            entityLock.Verify(x => x.Dispose());
        }

        [Fact]
        public void Finish_DelegatesToKnowledgebase()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager(s =>
                s.SetupGet(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Other));
            var result = testObject.Complete(input.Object);

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
            input.SetupGet(x => x.OriginalShipment).Returns(newShipment);

            var result = testObject.Complete(input.Object);

            Assert.Equal(expected, result.WorldshipExported);
        }

        [Fact]
        public void Finish_DelegatesToShippingManager_ToRefreshShipment()
        {
            testObject.Complete(input.Object);

            mock.Mock<IShippingManager>().Verify(x => x.RefreshShipment(shipment));
        }

        [Fact]
        public void Finish_SetsErrorMessage_WhenRefreshShipmentFailsAndThereIsNoExistingError()
        {
            mock.Mock<IShippingManager>()
                .Setup(x => x.RefreshShipment(It.IsAny<ShipmentEntity>()))
                .Throws<ObjectDeletedException>();

            var result = testObject.Complete(input.Object);

            mock.Mock<IShippingErrorManager>().Verify(x => x.SetShipmentErrorMessage(1234, It.IsAny<Exception>()));
            Assert.Contains("deleted", result.ErrorMessage);
        }

        [Fact]
        public void Finish_DoesNotSetErrorMessage_WhenRefreshShipmentFailsAndThereIsAnExistingError()
        {
            mock.Mock<IShippingManager>()
                .Setup(x => x.RefreshShipment(It.IsAny<ShipmentEntity>()))
                .Throws<ObjectDeletedException>();

            input.Setup(x => x.Exception).Returns(new Exception("Foo"));
            input.Setup(x => x.Success).Returns(false);

            var result = testObject.Complete(input.Object);

            mock.Mock<IShippingErrorManager>()
                .Verify(x => x.SetShipmentErrorMessage(It.IsAny<long>(), It.IsAny<Exception>()), Times.Never);
            Assert.DoesNotContain("deleted", result.ErrorMessage);
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
