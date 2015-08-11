using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    public class FedExRatePackageSpecialServicesManipulatorTest
    {
        private FedExRatePackageSpecialServicesManipulator testObject;


        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private FedExAccountEntity account;

        public FedExRatePackageSpecialServicesManipulatorTest()
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


        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new RateReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenNoSignature_AndPackageDeclaredValueGreaterThan500_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.NoSignature;
            shipmentEntity.FedEx.Packages[1].DeclaredValue = 500.01M;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_SignatureOptionIsNull_WhenUsingServiceDefault_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.ServiceDefault;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.RequestedPackageLineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail == null));
        }

        [Fact]
        public void Manipulate_SignatureOptionIsNotNull_WhenNotUsingServiceDefaultSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.NoSignature;

            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.RequestedPackageLineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail != null));
        }

        [Fact]
        public void Manipulate_OptionTypeIsIndirect_WhenFedExPackageIsUsingIndirectSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.Indirect;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.INDIRECT));
        }

        [Fact]
        public void Manipulate_OptionTypeIsDirect_WhenFedExPackageIsUsingDirectSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.Direct;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.DIRECT));
        }

        [Fact]
        public void Manipulate_OptionTypeIsAdult_WhenFedExPackageIsUsingAdultSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.Adult;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.ADULT));
        }

        [Fact]
        public void Manipulate_OptionTypeIsNone_WhenFedExPackageIsUsingNoneRequiredSignature_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.NoSignature;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.NO_SIGNATURE_REQUIRED));
        }

        [Fact]
        public void Manipulate_SignatureReleaseNumberMatchesFedExAccount_WhenNotUsingServiceDefault_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.NoSignature;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.SignatureReleaseNumber == account.SignatureRelease));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsSignatureOption_WhenNotUsingServiceDefault_Test()
        {
            shipmentEntity.FedEx.Signature = (int)FedExSignatureType.NoSignature;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.SIGNATURE_OPTION)));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsNonStandardContainer_WhenShipmentIsHomeDelivery_AndNonStandardContainer_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = true;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsNonStandardContainer_WhenShipmentIsGround_AndNonStandardContainer_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = true;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsNotGroundOrHomeDelivery_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = true;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsGround_WithoutNonStandardContainer_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = false;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsHomeDelivery_WithoutNonStandardContainer_Test()
        {
            shipmentEntity.FedEx.NonStandardContainer = false;
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsAlcohol_WhenOnePackageContainsAlcohol_Test()
        {
            shipmentEntity.FedEx.Packages[0].ContainsAlcohol = true;
            shipmentEntity.FedEx.Packages[1].ContainsAlcohol = false;

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(1, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsAlcohol_WhenAllPackagesContainsAlcohol_Test()
        {
            foreach (FedExPackageEntity package in shipmentEntity.FedEx.Packages)
            {
                package.ContainsAlcohol = true;
            }

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(shipmentEntity.FedEx.Packages.Count, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }

        [Fact]
        public void Manipulate_SpecialServicesDoesNotContainAlcohol_WhenZeroPackagesContainAlcohol_Test()
        {
            foreach (FedExPackageEntity package in shipmentEntity.FedEx.Packages)
            {
                package.ContainsAlcohol = false;
            }

            testObject.Manipulate(carrierRequest.Object);

            RequestedPackageLineItem[] lineItems = nativeRequest.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }
    }
}
