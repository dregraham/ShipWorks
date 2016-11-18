using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.GetLabel;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps
{
    public class GetLabelStepTest : IDisposable
    {
        private readonly AutoMock mock;
        private GetLabelStep testObject;
        private readonly Mock<IPrepareShipmentResult> getLabelInputMock;
        private readonly IPrepareShipmentResult getLabelInput;
        private readonly ShipmentEntity shipment;
        private readonly List<ShipmentEntity> shipments;

        public GetLabelStepTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment(new OrderEntity()).AsOther().Build();
            shipments = new List<ShipmentEntity> { shipment };

            getLabelInputMock = mock.Mock<IPrepareShipmentResult>();
            getLabelInputMock.SetupGet(x => x.OriginalShipment).Returns(shipment);
            getLabelInputMock.SetupGet(x => x.Shipments).Returns(shipments);
            getLabelInputMock.SetupGet(x => x.Success).Returns(true);
            getLabelInput = getLabelInputMock.Object;

            // This is needed because mocks can't be cloned, and the GetLabel process clones shipments
            mock.Mock<IApplyOrderedManipulators<IGetLabelManipulator, ShipmentEntity>>()
                .Setup(x => x.Apply(It.IsAny<ShipmentEntity>()))
                .Returns((ShipmentEntity x) => x);

            testObject = mock.Create<GetLabelStep>();
        }

        [Fact]
        public void GetLabel_ReturnsException_WhenInputHasException()
        {
            IDisposable entityLock = mock.Create<IDisposable>();
            ShippingException exception = new ShippingException();

            getLabelInputMock.SetupGet(x => x.Success).Returns(false);
            getLabelInputMock.SetupGet(x => x.EntityLock).Returns(entityLock);
            getLabelInputMock.SetupGet(x => x.Exception).Returns(exception);

            var result = testObject.GetLabel(getLabelInput);

            Assert.Equal(exception, result.Exception);
            Assert.Equal(entityLock, result.EntityLock);
        }

        [Fact]
        public void GetLabel_DelegatesToApplyManipulators_ForEachShipment()
        {
            mock.Mock<IApplyValidators<IGetLabelValidator, ShipmentEntity>>()
                .Setup(x => x.Apply(It.IsAny<ShipmentEntity>()))
                .Returns(new ApplyValidatorsResult(false, new[] { "Foo" }));

            shipments.Add(Create.Shipment(new OrderEntity()).AsOnTrac().Build());

            var result = testObject.GetLabel(getLabelInput);

            mock.Mock<IApplyOrderedManipulators<IGetLabelManipulator, ShipmentEntity>>()
                .Verify(x => x.Apply(shipments[0]));
            mock.Mock<IApplyOrderedManipulators<IGetLabelManipulator, ShipmentEntity>>()
                .Verify(x => x.Apply(shipments[1]));
        }

        [Fact]
        public void GetLabel_ReturnsSecondShipment_WhenFirstHasLicenseError()
        {
            mock.Mock<IApplyValidators<IGetLabelValidator, ShipmentEntity>>()
                .Setup(x => x.Apply(shipment))
                .Returns(new ApplyValidatorsResult(false, new[] { "Foo" }));

            var secondShipment = Create.Shipment(new OrderEntity()).AsOnTrac().Build();
            shipments.Add(secondShipment);

            var result = testObject.GetLabel(getLabelInput);

            Assert.Equal(secondShipment, result.OriginalShipment);
        }

        [Fact]
        public void GetLabel_DelegatesToOverrideShipmentDetails()
        {
            var storeType = mock.Mock<StoreType>();
            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(getLabelInput.Store))
                .Returns(storeType);

            var result = testObject.GetLabel(getLabelInput);

            storeType.Verify(x => x.OverrideShipmentDetails(shipment));
        }

        [Fact]
        public void GetLabel_ReturnsListOfFieldsToRestore_WhenLabelIsSuccessful()
        {
            var fields = new List<ShipmentFieldIndex>();
            var storeType = mock.Mock<StoreType>();
            storeType.Setup(x => x.OverrideShipmentDetails(It.IsAny<ShipmentEntity>()))
                .Returns(fields);

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(getLabelInput.Store))
                .Returns(storeType);

            var result = testObject.GetLabel(getLabelInput);

            Assert.Equal(fields, result.FieldsToRestore);
        }

        [Fact]
        public void GetLabel_DelegatesToLabelService()
        {
            var labelService = mock.Mock<ILabelService>();
            mock.Mock<ILabelServiceFactory>()
                .Setup(x => x.Create(ShipmentTypeCode.Other))
                .Returns(labelService);

            var result = testObject.GetLabel(getLabelInput);

            labelService.Verify(x => x.Create(shipment));
        }

        [Fact]
        public void GetLabel_ReturnsLabelData_WhenLabelIsSuccessful()
        {
            var labelData = mock.Create<IDownloadedLabelData>();
            var labelService = mock.Mock<ILabelService>();
            labelService.Setup(x => x.Create(It.IsAny<ShipmentEntity>()))
                .Returns(labelData);

            mock.Mock<ILabelServiceFactory>()
                .Setup(x => x.Create(It.IsAny<ShipmentTypeCode>()))
                .Returns(labelService);

            var result = testObject.GetLabel(getLabelInput);

            Assert.Equal(labelData, result.LabelData);
        }

        [Fact]
        public void GetLabel_ReturnsClonedShipment_WhenLabelIsSuccessful()
        {
            shipment.ShipmentID = 9876;

            var result = testObject.GetLabel(getLabelInput);

            Assert.NotEqual(shipment, result.Clone);
            Assert.Equal(9876, result.Clone.ShipmentID);
        }

        [Fact]
        public void GetLabel_ReturnsInputData_WhenLabelIsSuccessful()
        {
            var result = testObject.GetLabel(getLabelInput);

            Assert.Equal(getLabelInput.EntityLock, result.EntityLock);
            Assert.Equal(getLabelInput.OriginalShipment, result.OriginalShipment);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
