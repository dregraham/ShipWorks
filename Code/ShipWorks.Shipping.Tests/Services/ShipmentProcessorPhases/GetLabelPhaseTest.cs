using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Editions;
using ShipWorks.Shipping.Services.ShipmentProcessorPhases;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorPhases
{
    public class GetLabelPhaseTest : IDisposable
    {
        readonly AutoMock mock;
        readonly GetLabelPhase testObject;
        readonly PrepareShipmentResult defaultInput;
        readonly ShipmentEntity shipment;
        readonly List<ShipmentEntity> shipments;
        readonly StoreEntity store;

        public GetLabelPhaseTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            store = Create.Store<StoreEntity>().Build();
            shipment = Create.Shipment(new OrderEntity()).AsOther().Build();
            shipments = new List<ShipmentEntity> { shipment };
            defaultInput = new PrepareShipmentResult(null, new ProcessShipmentState(shipment, null, null), shipments, store);
            testObject = mock.Create<GetLabelPhase>();
        }

        [Fact]
        public void GetLabel_ReturnsException_WhenInputHasException()
        {
            IDisposable entityLock = mock.Create<IDisposable>();
            ShippingException exception = new ShippingException();

            var result = testObject.GetLabel(new PrepareShipmentResult(entityLock, new ProcessShipmentState(null), exception));

            Assert.Equal(exception, result.Exception);
            Assert.Equal(entityLock, result.EntityLock);
        }

        [Fact]
        public void GetLabel_DelegatesToEnsureShipmentTypeIsAllowed_ForEachShipment()
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(EditionFeature.ProcessShipment, ShipmentTypeCode.Other))
                .Returns(EditionRestrictionLevel.Forbidden);

            shipments.Add(Create.Shipment(new OrderEntity()).AsOnTrac().Build());

            var result = testObject.GetLabel(new PrepareShipmentResult(null, new ProcessShipmentState(shipment, null, null), shipments, store));

            mock.Mock<ILicenseService>()
                .Verify(x => x.CheckRestriction(EditionFeature.ProcessShipment, ShipmentTypeCode.Other));
            mock.Mock<ILicenseService>()
                .Verify(x => x.CheckRestriction(EditionFeature.ProcessShipment, ShipmentTypeCode.OnTrac));
        }

        [Fact]
        public void GetLabel_ReturnsSecondShipment_WhenFirstHasLicenseError()
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(EditionFeature.ProcessShipment, ShipmentTypeCode.Other))
                .Returns(EditionRestrictionLevel.Forbidden);

            var secondShipment = Create.Shipment(new OrderEntity()).AsOnTrac().Build();
            shipments.Add(secondShipment);

            var result = testObject.GetLabel(new PrepareShipmentResult(null, new ProcessShipmentState(shipment, null, null), shipments, store));

            Assert.Equal(secondShipment, result.OriginalShipment);
        }

        [Fact]
        public void GetLabel_ReturnsCanceled_WhenShipmentTypeReturnsNull()
        {
            mock.Mock<IShipmentTypeManager>()
                .Setup(x => x.Get(shipment))
                .Returns<ShipmentType>(null);

            var result = testObject.GetLabel(defaultInput);

            Assert.True(result.Canceled);
        }

        [Fact]
        public void GetLabel_DelegatesToEnsureShipmentLoaded()
        {
            var result = testObject.GetLabel(defaultInput);

            mock.Mock<IShippingManager>().Verify(x => x.EnsureShipmentLoaded(shipment));
        }

        [Fact]
        public void GetLabel_DelegatesToUpdateDynamicShipmentData()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager();

            var result = testObject.GetLabel(defaultInput);

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void GetLabel_GetsShippingSettings_WhenPhoneIsBlank(string phone)
        {
            shipment.ShipPhone = phone;

            var result = testObject.GetLabel(defaultInput);

            mock.Mock<IShippingSettings>().Verify(x => x.FetchReadOnly());
        }

        [Fact]
        public void GetLabel_DoesNotGetShippingSettings_WhenPhoneIsNotBlank()
        {
            shipment.ShipPhone = "314-555-6309";

            var result = testObject.GetLabel(defaultInput);

            mock.Mock<IShippingSettings>().Verify(x => x.FetchReadOnly(), Times.Never);
        }

        [Theory]
        [InlineData(ShipmentBlankPhoneOption.SpecifiedPhone, "314-555-1234")]
        [InlineData(ShipmentBlankPhoneOption.ShipperPhone, "314-555-5678")]
        public void GetLabel_UpdatesPhoneNumber_WhenPhoneIsBlank(ShipmentBlankPhoneOption option, string expected)
        {
            shipment.OriginPhone = "314-555-5678";
            shipment.ShipPhone = string.Empty;

            var settings = mock.CreateMock<IShippingSettingsEntity>(s =>
            {
                s.SetupGet(x => x.BlankPhoneNumber).Returns("314-555-1234");
                s.SetupGet(x => x.BlankPhoneOption).Returns((int) option);
            });
            mock.Mock<IShippingSettings>().Setup(x => x.FetchReadOnly()).Returns(settings);

            var result = testObject.GetLabel(defaultInput);

            Assert.Equal(expected, result.OriginalShipment.ShipPhone);
        }

        [Fact]
        public void GetLabel_UpdatesResidentialStatus_IfShipmentTypeRequires()
        {
            shipment.ResidentialResult = false;

            var shipmentType = mock.WithShipmentTypeFromShipmentManager(s =>
                s.Setup(x => x.IsResidentialStatusRequired(shipment)).Returns(true));
            mock.Mock<IResidentialDeterminationService>()
                .Setup(x => x.IsResidentialAddress(shipment)).Returns(true);

            var result = testObject.GetLabel(defaultInput);

            Assert.True(shipment.ResidentialResult);
        }

        [Fact]
        public void GetLabel_DoesNotUpdateResidentialStatus_IfShipmentTypeDoesNotRequire()
        {
            shipment.ResidentialResult = false;

            var shipmentType = mock.WithShipmentTypeFromShipmentManager(s =>
                s.Setup(x => x.IsResidentialStatusRequired(shipment)).Returns(false));

            var result = testObject.GetLabel(defaultInput);

            mock.Mock<IResidentialDeterminationService>()
                .Verify(x => x.IsResidentialAddress(It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void GetLabel_ThrowsShippingException_WhenApoRestrictionCheckFails()
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(EditionFeature.PostalApoFpoPoboxOnly, shipment))
                .Returns(EditionRestrictionLevel.Forbidden);

            var result = testObject.GetLabel(defaultInput);

            Assert.IsType<ShippingException>(result.Exception);
            Assert.Contains("APO", result.Exception.Message);
        }

        [Fact]
        public void GetLabel_ThrowsShippingException_WhenShipmentTypeRestrictionCheckFails()
        {
            mock.Mock<ILicenseService>()
                .Setup(x => x.CheckRestriction(EditionFeature.ShipmentType, ShipmentTypeCode.Other))
                .Returns(EditionRestrictionLevel.Forbidden);

            var shipmentType = mock.WithShipmentTypeFromShipmentManager(s =>
                s.SetupGet(x => x.ShipmentTypeCode).Returns(ShipmentTypeCode.Other));

            var result = testObject.GetLabel(defaultInput);

            Assert.IsType<ShippingException>(result.Exception);
            Assert.Contains("does not support", result.Exception.Message);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetLabel_SetsReturnShipmentToFalse_WhenShipmentTypeDoesNotSupportReturns(bool input)
        {
            shipment.ReturnShipment = input;

            var shipmentType = mock.WithShipmentTypeFromShipmentManager(s =>
                s.SetupGet(x => x.SupportsReturns).Returns(false));

            var result = testObject.GetLabel(defaultInput);

            Assert.False(shipment.ReturnShipment);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetLabel_DoesNotChangeReturnValue_WhenShipmentTypeSupportsReturns(bool input)
        {
            shipment.ReturnShipment = input;

            var shipmentType = mock.WithShipmentTypeFromShipmentManager(s =>
                s.SetupGet(x => x.SupportsReturns).Returns(true));

            var result = testObject.GetLabel(defaultInput);

            Assert.Equal(input, shipment.ReturnShipment);
        }

        [Fact]
        public void GetLabel_DelegatesToOverrideShipmentDetails()
        {
            var storeType = mock.Mock<StoreType>();
            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(defaultInput.Store))
                .Returns(storeType);

            var result = testObject.GetLabel(defaultInput);

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
                .Setup(x => x.GetType(defaultInput.Store))
                .Returns(storeType);

            var result = testObject.GetLabel(defaultInput);

            Assert.Equal(fields, result.FieldsToRestore);
        }

        [Fact]
        public void GetLabel_DelegatesToLabelService()
        {
            var labelService = mock.Mock<ILabelService>();
            mock.Mock<ILabelServiceFactory>()
                .Setup(x => x.Create(ShipmentTypeCode.Other))
                .Returns(labelService);

            var result = testObject.GetLabel(defaultInput);

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

            var result = testObject.GetLabel(defaultInput);

            Assert.Equal(labelData, result.LabelData);
        }

        [Fact]
        public void GetLabel_ReturnsClonedShipment_WhenLabelIsSuccessful()
        {
            shipment.ShipmentID = 9876;

            var result = testObject.GetLabel(defaultInput);

            Assert.NotEqual(shipment, result.Clone);
            Assert.Equal(9876, result.Clone.ShipmentID);
        }

        [Fact]
        public void GetLabel_ReturnsInputData_WhenLabelIsSuccessful()
        {
            var result = testObject.GetLabel(defaultInput);

            Assert.Equal(defaultInput.EntityLock, result.EntityLock);
            Assert.Equal(defaultInput.OriginalShipment, result.OriginalShipment);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
