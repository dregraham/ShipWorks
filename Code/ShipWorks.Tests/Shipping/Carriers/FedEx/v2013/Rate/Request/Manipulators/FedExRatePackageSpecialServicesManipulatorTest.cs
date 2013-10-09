using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRatePackageSpecialServicesManipulatorTest
    {
        private FedExRatePackageSpecialServicesManipulator testObject;


        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initiliaze()
        {
            account = new FedExAccountEntity
            {
                SignatureRelease = "release"
            };

            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity()
            {
                DimsLength = 2,
                DimsWidth = 4,
                DimsHeight = 8,
                // total weight should be 48
                DimsWeight = 16,
                DimsAddWeight = true,
                Weight = 32,
                DeclaredValue = 64
            });

            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity()
            {
                DimsLength = 3,
                DimsWidth = 6,
                DimsHeight = 12,
                // total weight should be 72
                DimsWeight = 24,
                DimsAddWeight = true,
                Weight = 48,
                DeclaredValue = 96
            });

            nativeRequest = new RateRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[0]
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExRatePackageSpecialServicesManipulator();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new RateReply());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenNoSignature_AndPackageDeclaredValueGreaterThan500_Test()
        {
            shipmentEntity.FedEx.Signature = (int) FedExSignatureType.NoSignature;
            shipmentEntity.FedEx.Packages[1].DeclaredValue = 500.01M;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SignatureOptionIsNull_WhenUsingServiceDefault_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.ServiceDefault;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.RequestedPackageLineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail == null));
        }

        [TestMethod]
        public void Manipulate_SignatureOptionIsNotNull_WhenNotUsingServiceDefaultSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int) FedExSignatureType.NoSignature;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.RequestedPackageLineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail != null));
        }

        [TestMethod]
        public void Manipulate_OptionTypeIsIndirect_WhenFedExPackageIsUsingIndirectSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.Indirect;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.IsTrue(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.INDIRECT));
        }

        [TestMethod]
        public void Manipulate_OptionTypeIsDirect_WhenFedExPackageIsUsingDirectSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.Direct;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.IsTrue(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.DIRECT));
        }

        [TestMethod]
        public void Manipulate_OptionTypeIsAdult_WhenFedExPackageIsUsingAdultSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.Adult;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.IsTrue(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.ADULT));
        }

        [TestMethod]
        public void Manipulate_OptionTypeIsNone_WhenFedExPackageIsUsingNoneRequiredSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.NoSignature;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.IsTrue(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.NO_SIGNATURE_REQUIRED));
        }

        [TestMethod]
        public void Manipulate_SignatureReleaseNumberMatchesFedExAccount_WhenNotUsingServiceDefault_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.NoSignature;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.IsTrue(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.SignatureReleaseNumber == account.SignatureRelease));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesContainsSignatureOption_WhenNotUsingServiceDefault_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.NoSignature;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.IsTrue(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.SIGNATURE_OPTION)));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesContainsNonStandardContainer_WhenShipmentIsHomeDelivery_AndNonStandardContainer_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = true;
            shipmentEntity.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.IsTrue(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesContainsNonStandardContainer_WhenShipmentIsGround_AndNonStandardContainer_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = true;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.IsTrue(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsNotGroundOrHomeDelivery_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = true;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.AreEqual(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsGround_WithoutNonStandardContainer_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = false;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.AreEqual(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsHomeDelivery_WithoutNonStandardContainer_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = false;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.AreEqual(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesContainsAlcohol_WhenOnePackageContainsAlcohol_Test()
        {
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;
            shipmentEntity.FedEx.Packages[1].ContainsAlcohol = false;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.AreEqual(1, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesContainsAlcohol_WhenAllPackagesContainsAlcohol_Test()
        {
            foreach (FedExPackageEntity package in shipmentEntity.FedEx.Packages)
            {
                package.ContainsAlcohol = true;
            }

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.AreEqual(shipmentEntity.FedEx.Packages.Count, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }

        [TestMethod]
        public void Manipulate_SpecialServicesDoesNotContainAlcohol_WhenZeroPackagesContainAlcohol_Test()
        {
            foreach (FedExPackageEntity package in shipmentEntity.FedEx.Packages)
            {
                package.ContainsAlcohol = false;
            }

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.AreEqual(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }
    }
}
