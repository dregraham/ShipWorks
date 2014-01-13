using System.Collections.Generic;
using System.Linq;
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
    public class FedExPackageSpecialServicesManipulatorTest
    {
        private const string signatureRelease = "SigRelNum";
        private ShipmentEntity shipmentEntity;
        private FedExPackageSpecialServicesManipulator testObject;

        private ProcessShipmentRequest nativeRequest;
        private Mock<CarrierRequest> carrierRequest;
        private FedExAccountEntity account;
        
        [TestInitialize]
        public void Initiliaze()
        {
            shipmentEntity = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = false;
            shipmentEntity.FedEx.Packages[1].ContainsAlcohol = false;


            account = new FedExAccountEntity() {SignatureRelease = signatureRelease};

            nativeRequest = new ProcessShipmentRequest();
            
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExPackageSpecialServicesManipulator();
        }

        [TestMethod]
        public void Manipulate_SignatureOptionAdded_FedExShipmentSignatureSetToNoSignature_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(
                FedExSignatureType.NoSignature,
                (FedExSignatureType)shipmentEntity.FedEx.Signature, "Test Shipment built by BuildFedExShipmentEntity not initialized to no signature");

            Assert.AreEqual(
                SignatureOptionType.NO_SIGNATURE_REQUIRED,
                ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail.OptionType);

            Assert.AreEqual(
                signatureRelease,
                ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail.SignatureReleaseNumber);

            Assert.IsTrue(((ProcessShipmentRequest) carrierRequest.Object.NativeRequest)
                .RequestedShipment
                .RequestedPackageLineItems[0]
                .SpecialServicesRequested
                .SpecialServiceTypes
                .Contains(PackageSpecialServiceType.SIGNATURE_OPTION));
        }

        [TestMethod]
        public void Manipulate_SignatureOptionNotAdded_FedExShipmentSignatureSetToServiceDefault_Test()
        {
            shipmentEntity.FedEx.Signature = (int) FedExSignatureType.ServiceDefault;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail);

            Assert.IsFalse(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest)
                .RequestedShipment
                .RequestedPackageLineItems[0]
                .SpecialServicesRequested
                .SpecialServiceTypes
                .Contains(PackageSpecialServiceType.SIGNATURE_OPTION));
        }
        
        [TestMethod]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackageAndRequestSpecialServicesRequestedIsNull_Test()
        {
            shipmentEntity.FedEx.Packages.RemoveAt(1);
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;

            nativeRequest.RequestedShipment = new RequestedShipment();
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, AlcholCount());
        }

        [TestMethod]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackageAndSpecialServiceTypesIsNull_Test()
        {
            shipmentEntity.FedEx.Packages.RemoveAt(1);
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;

            nativeRequest.RequestedShipment = new RequestedShipment();
            nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, AlcholCount());
        }

        [TestMethod]
        public void Manipulate_AlcoholPackageWithAlcoholIsZero_ShipmentPackageEntityHasZeroAlcoholPackage_Test()
        {
            shipmentEntity.FedEx.Packages.RemoveAt(1);

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(0, AlcholCount());
        }

        [TestMethod]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackage_Test()
        {
            shipmentEntity.FedEx.Packages.RemoveAt(1);
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, AlcholCount());
        }

        [TestMethod]
        public void Manipulate_AlcoholPackagesWithAlcoholIsZero_ShipmentPackageEntityHasZeroAlcoholPackages_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(0, AlcholCount());
        }

        [TestMethod]
        public void Manipulate_AlcoholPackagesWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackages_Test()
        {
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(1, AlcholCount());
        }

        /// <summary>
        /// Determines the number of packages with Alchol specified.
        /// </summary>
        /// <returns>Returns the number of packages with Alchol specified.</returns>
        private int AlcholCount()
        {
            return nativeRequest.RequestedShipment.RequestedPackageLineItems
                .Where(rpli => rpli.SpecialServicesRequested != null && rpli.SpecialServicesRequested.SpecialServiceTypes != null)
                .SelectMany(rpli => rpli.SpecialServicesRequested.SpecialServiceTypes)
                    .Count(sst => sst == PackageSpecialServiceType.ALCOHOL);
        }
    }
}
