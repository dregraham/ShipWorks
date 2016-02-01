using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExPackageSpecialServicesManipulatorTest
    {
        private const string signatureRelease = "SigRelNum";
        private ShipmentEntity shipmentEntity;
        private FedExPackageSpecialServicesManipulator testObject;

        private ProcessShipmentRequest nativeRequest;
        private Mock<CarrierRequest> carrierRequest;
        private FedExAccountEntity account;
        
        public FedExPackageSpecialServicesManipulatorTest()
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

        [Fact]
        public void Manipulate_SignatureOptionAdded_FedExShipmentSignatureSetToNoSignature()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(
                FedExSignatureType.NoSignature,
                (FedExSignatureType)shipmentEntity.FedEx.Signature);

            Assert.Equal(
                SignatureOptionType.NO_SIGNATURE_REQUIRED,
                ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail.OptionType);

            Assert.Equal(
                signatureRelease,
                ((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail.SignatureReleaseNumber);

            Assert.True(((ProcessShipmentRequest) carrierRequest.Object.NativeRequest)
                .RequestedShipment
                .RequestedPackageLineItems[0]
                .SpecialServicesRequested
                .SpecialServiceTypes
                .Contains(PackageSpecialServiceType.SIGNATURE_OPTION));
        }

        [Fact]
        public void Manipulate_SignatureOptionNotAdded_FedExShipmentSignatureSetToServiceDefault()
        {
            shipmentEntity.FedEx.Signature = (int) FedExSignatureType.ServiceDefault;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(((ProcessShipmentRequest) carrierRequest.Object.NativeRequest).RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail);

            Assert.False(((ProcessShipmentRequest)carrierRequest.Object.NativeRequest)
                .RequestedShipment
                .RequestedPackageLineItems[0]
                .SpecialServicesRequested
                .SpecialServiceTypes
                .Contains(PackageSpecialServiceType.SIGNATURE_OPTION));
        }
        
        [Fact]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackageAndRequestSpecialServicesRequestedIsNull()
        {
            shipmentEntity.FedEx.Packages.RemoveAt(1);
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;

            nativeRequest.RequestedShipment = new RequestedShipment();
            nativeRequest.RequestedShipment.SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, AlcholCount());
        }

        [Fact]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackageAndSpecialServiceTypesIsNull()
        {
            shipmentEntity.FedEx.Packages.RemoveAt(1);
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;

            nativeRequest.RequestedShipment = new RequestedShipment();
            nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, AlcholCount());
        }

        [Fact]
        public void Manipulate_AlcoholPackageWithAlcoholIsZero_ShipmentPackageEntityHasZeroAlcoholPackage()
        {
            shipmentEntity.FedEx.Packages.RemoveAt(1);

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(0, AlcholCount());
        }

        [Fact]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackage()
        {
            shipmentEntity.FedEx.Packages.RemoveAt(1);
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, AlcholCount());
        }

        [Fact]
        public void Manipulate_AlcoholPackagesWithAlcoholIsZero_ShipmentPackageEntityHasZeroAlcoholPackages()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(0, AlcholCount());
        }

        [Fact]
        public void Manipulate_AlcoholPackagesWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackages()
        {
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(1, AlcholCount());
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
