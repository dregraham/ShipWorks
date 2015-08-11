using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRatePackagingTypeManipulatorTest
    {
        private FedExRatePackagingTypeManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        public FedExRatePackagingTypeManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity { FedEx = new FedExShipmentEntity() };

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRatePackagingTypeManipulator();
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_BOX);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIs10KgBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box10Kg;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_10KG_BOX);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIs25KgBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box25Kg;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_25KG_BOX);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsCustom_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Custom;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.YOUR_PACKAGING);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsEnvelope_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Envelope;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_ENVELOPE);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsPak_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Pak;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_PAK);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsTube_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Tube;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_TUBE);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsSmallBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.SmallBox;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_SMALL_BOX);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsMediumBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.MediumBox;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_MEDIUM_BOX);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsLargeBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.LargeBox;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_LARGE_BOX);
        }

        [Fact]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsExtraLargeBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.ExtraLargeBox;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.Equal(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_EXTRA_LARGE_BOX);
        }

        [Fact]
        public void Manipulate_PackagingTypeSpecifiedIsTrue_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Tube;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.True(nativeRequest.RequestedShipment.PackagingTypeSpecified);
        }

        [Fact]
        public void Manipulate_ThrowsException_WhenPackageTypeIsUnknown_Test()
        {
            shipmentEntity.FedEx.PackagingType = 43;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }
    }

}
