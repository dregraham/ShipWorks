using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Actions;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps
{
    public class LabelPersistenceStepTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly LabelPersistenceStep testObject;
        private readonly Mock<ILabelRetrievalResult> labelResult1;
        private readonly List<ILabelRetrievalResult> input;
        private readonly ShipmentEntity shipment;
        private readonly StoreEntity store;

        public LabelPersistenceStepTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<LabelPersistenceStep>();

            shipment = Create.Shipment(new OrderEntity()).AsOther().Build();
            store = Create.Store<StoreEntity>().Build();

            labelResult1 = mock.CreateMock<ILabelRetrievalResult>(r =>
            {
                r.SetupGet(x => x.Success).Returns(true);
                r.SetupGet(x => x.OriginalShipment).Returns(shipment);
                r.SetupGet(x => x.Store).Returns(store);
            });

            input = new List<ILabelRetrievalResult>();
            input.Add(labelResult1.Object);
        }

        [Fact]
        public void SaveLabel_ReturnsCopiedResults_WhenPreviousPhaseWasNotSuccessful()
        {
            var exception = new ShippingException();
            var entityLock = mock.Build<IDisposable>();

            labelResult1.SetupGet(x => x.Success).Returns(false);
            labelResult1.SetupGet(x => x.Exception).Returns(exception);
            labelResult1.SetupGet(x => x.Canceled).Returns(true);
            labelResult1.SetupGet(x => x.EntityLock).Returns(entityLock);

            var result = testObject.SaveLabels(input);

            Assert.Equal(false, result.First().Success);
            Assert.Equal(exception, result.First().Exception);
            Assert.Equal(true, result.First().Canceled);
            Assert.Equal(entityLock, result.First().EntityLock);
        }

        [Fact]
        public void SaveLabel_DelegatesToDownloadedLabel()
        {
            var downloadedData = mock.Mock<IDownloadedLabelData>();
            labelResult1.SetupGet(x => x.LabelData).Returns(downloadedData);

            testObject.SaveLabels(input);

            downloadedData.Verify(x => x.Save());
        }

        [Fact]
        public void SaveLabel_ReturnsException_WhenSavingLabelDataFails()
        {
            var exception = new Exception();
            var downloadedData = mock.CreateMock<IDownloadedLabelData>(d => d.Setup(x => x.Save()).Throws(exception));
            labelResult1.SetupGet(x => x.LabelData).Returns(downloadedData);

            var result = testObject.SaveLabels(input);

            Assert.Equal(exception, result.First().Exception.GetBaseException());
        }

        [Fact]
        public void SaveLabel_InsuresShipment_IfCarrierIsInsuredByInsureShip()
        {
            var insurance = mock.Mock<IInsureShipService>();
            insurance.Setup(x => x.IsInsuredByInsureShip(shipment)).Returns(true);

            testObject.SaveLabels(input);

            insurance.Verify(x => x.Insure(shipment));
        }

        [Fact]
        public void SaveLabel_DoesNotInsureShipment_IfCarrierIsNotInsuredByInsureShip()
        {
            var insurance = mock.Mock<IInsureShipService>();
            insurance.Setup(x => x.IsInsuredByInsureShip(shipment)).Returns(false);

            testObject.SaveLabels(input);

            insurance.Verify(x => x.Insure(AnyShipment), Times.Never);
        }

        [Fact]
        public void SaveLabel_UpdatesFieldsOnShipment_FromClonedShipment()
        {
            List<ShipmentFieldIndex> fields = new List<ShipmentFieldIndex> { ShipmentFieldIndex.ShipCity };
            ShipmentEntity clone = new ShipmentEntity { ShipCity = "Foo" };
            shipment.ShipCity = "Bar";
            shipment.ShipPostalCode = "12345";

            labelResult1.SetupGet(x => x.Clone).Returns(clone);
            labelResult1.SetupGet(x => x.FieldsToRestore).Returns(fields);

            testObject.SaveLabels(input);

            Assert.Equal("Foo", shipment.ShipCity);
            Assert.False(shipment.Fields[(int) ShipmentFieldIndex.ShipCity].IsChanged);
            Assert.Equal("12345", shipment.ShipPostalCode);
        }

        [Fact]
        public void SaveLabel_UpdatesOriginalShipment()
        {
            DateTime now = new DateTime(2016, 11, 3, 5, 12, 34);
            mock.Mock<IDateTimeProvider>().Setup(x => x.UtcNow).Returns(now);
            mock.Mock<IUserSession>().SetupGet(x => x.Computer).Returns(new ComputerEntity { ComputerID = 5678 });
            mock.Mock<IUserSession>().SetupGet(x => x.User).Returns(new UserEntity { UserID = 1234 });

            testObject.SaveLabels(input);

            Assert.True(shipment.Processed);
            Assert.Equal(now, shipment.ProcessedDate);
            Assert.Equal(1234, shipment.ProcessedUserID);
            Assert.Equal(5678, shipment.ProcessedComputerID);
        }

        [Fact]
        public void SaveLabel_DelegatesToSqlAdapter_ToSaveShipment()
        {
            var adapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.CreateTransacted()).Returns(adapter);

            testObject.SaveLabels(input);

            adapter.Verify(x => x.SaveAndRefetch(shipment));
        }

        [Fact]
        public void SaveLabel_DispatchesActions_IfShipmentTypeDoesNotCompleteExternally()
        {
            var adapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.CreateTransacted()).Returns(adapter);

            shipment.ShipmentTypeCode = ShipmentTypeCode.Usps;

            testObject.SaveLabels(input);

            mock.Mock<IActionDispatcher>()
                .Verify(x => x.DispatchShipmentProcessed(shipment, It.IsAny<ISqlAdapter>()));
        }

        [Fact]
        public void SaveLabel_DoesNotDispatchActions_IfShipmentTypeCompletesExternally()
        {
            var adapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.CreateTransacted()).Returns(adapter);

            shipment.ShipmentTypeCode = ShipmentTypeCode.UpsWorldShip;

            testObject.SaveLabels(input);

            mock.Mock<IActionDispatcher>()
                .Verify(x => x.DispatchShipmentProcessed(shipment, It.IsAny<ISqlAdapter>()), Times.Never);
        }

        [Fact]
        public void SaveLabel_DelegatesToSqlAdapter_ToCommitTransaction()
        {
            var adapter = mock.Mock<ISqlAdapter>();
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.CreateTransacted()).Returns(adapter);

            testObject.SaveLabels(input);

            adapter.Verify(x => x.Commit());
        }

        [Fact]
        public void SaveLabel_ReturnsDataFromInput_WhenSavingSucceeds()
        {
            var entityLock = mock.Build<IDisposable>();

            labelResult1.SetupGet(x => x.EntityLock).Returns(entityLock);

            var result = testObject.SaveLabels(input);

            Assert.Equal(entityLock, result.First().EntityLock);
            Assert.Equal(store, result.First().Store);
            Assert.Equal(shipment, result.First().OriginalShipment);
        }

        [Fact]
        public void SaveLabel_ReturnsDataFromInput_WhenSavingFails()
        {
            var adapter = mock.CreateMock<ISqlAdapter>(a =>
                a.Setup(x => x.SaveAndRefetch(It.IsAny<ShipmentEntity>())).Throws<Exception>());
            mock.Mock<ISqlAdapterFactory>().Setup(x => x.CreateTransacted()).Returns(adapter);
            var entityLock = mock.Build<IDisposable>();

            labelResult1.SetupGet(x => x.EntityLock).Returns(entityLock);

            var result = testObject.SaveLabels(input);

            Assert.Equal(entityLock, result.First().EntityLock);
            Assert.Equal(store, result.First().Store);
            Assert.Equal(shipment, result.First().OriginalShipment);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
