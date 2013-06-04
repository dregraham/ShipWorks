using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRatePackagingTypeManipulatorTest
    {
        private FedExRatePackagingTypeManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity{ FedEx = new FedExShipmentEntity() };

            nativeRequest = new RateRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExRatePackagingTypeManipulator();
        }

        [TestMethod]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box;

            testObject.Manipulate(carrierRequest.Object);
            
            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_BOX);
        }

        [TestMethod]
        public void Manipulate_SetFedExPackagingType_WhenPackageIs10KgBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box10Kg;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_10KG_BOX);
        }

        [TestMethod]
        public void Manipulate_SetFedExPackagingType_WhenPackageIs25KgBox_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box25Kg;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_25KG_BOX);
        }

        [TestMethod]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsCustom_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Custom;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.YOUR_PACKAGING);
        }

        [TestMethod]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsEnvelope_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Envelope;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_ENVELOPE);
        }

        [TestMethod]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsPak_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Pak;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_PAK);
        }

        [TestMethod]
        public void Manipulate_SetFedExPackagingType_WhenPackageIsTube_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Tube;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_TUBE);
        }

        [TestMethod]
        public void Manipulate_PackagingTypeSpecifiedIsTrue_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Tube;

            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.IsTrue(nativeRequest.RequestedShipment.PackagingTypeSpecified);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsException_WhenPackageTypeIsUnknown_Test()
        {
            shipmentEntity.FedEx.PackagingType = 43;

            testObject.Manipulate(carrierRequest.Object);
        }
    }

}
