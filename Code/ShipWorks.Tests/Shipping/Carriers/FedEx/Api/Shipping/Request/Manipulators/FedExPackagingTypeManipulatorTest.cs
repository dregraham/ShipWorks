using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExPackagingTypeManipulatorTest
    {
        private FedExPackagingTypeManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private ProcessShipmentRequest nativeRequest;
        private ShipmentEntity shipmentEntity;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Box;

            nativeRequest = new ProcessShipmentRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject = new FedExPackagingTypeManipulator();
        }

        [TestMethod]
        public void Manipulate_FedExPackagingTypeManipulator_ReturnsPackagingType_Test()
        {
            testObject.Manipulate(carrierRequest.Object);
            
            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_BOX);
        }

        [TestMethod]
        public void Manipulate_FedExPackagingTypeManipulator_ReturnsExtraLargeBoxPackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.ExtraLargeBox;
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_EXTRA_LARGE_BOX);
        }

        [TestMethod]
        public void Manipulate_FedExPackagingTypeManipulator_ReturnsSmallBoxPackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.SmallBox;
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_SMALL_BOX);
        }

        [TestMethod]
        public void Manipulate_FedExPackagingTypeManipulator_ReturnsLargeBoxPackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.LargeBox;
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_LARGE_BOX);
        }

        [TestMethod]
        public void Manipulate_FedExPackagingTypeManipulator_ReturnsMediumPackagingType_Test()
        {
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.MediumBox;
            testObject.Manipulate(carrierRequest.Object);

            // Make sure we got a the same values back
            Assert.AreEqual(nativeRequest.RequestedShipment.PackagingType, PackagingType.FEDEX_MEDIUM_BOX);
        }
    }
}
