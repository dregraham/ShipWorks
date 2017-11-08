using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRatePackageSpecialServicesManipulatorTest
    {
        private FedExRatePackageSpecialServicesManipulator testObject;
        private ShipmentEntity shipment;
        private readonly AutoMock mock;

        public FedExRatePackageSpecialServicesManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment().AsFedEx(f => f.WithPackage().WithPackage()).Build();

            testObject = mock.Create<FedExRatePackageSpecialServicesManipulator>();
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenNoSignature_AndPackageDeclaredValueGreaterThan500()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.NoSignature;
            shipment.FedEx.Packages[1].DeclaredValue = 500.01M;

            Assert.Throws<FedExException>(() => testObject.Manipulate(shipment, new RateRequest()));
        }

        [Fact]
        public void Manipulate_SignatureOptionIsNull_WhenUsingServiceDefault()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.ServiceDefault;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.RequestedPackageLineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail == null));
        }

        [Fact]
        public void Manipulate_SignatureOptionIsNotNull_WhenNotUsingServiceDefaultSignature()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.NoSignature;

            var result = testObject.Manipulate(shipment, new RateRequest());

            Assert.True(result.RequestedShipment.RequestedPackageLineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail != null));
        }

        [Fact]
        public void Manipulate_OptionTypeIsIndirect_WhenFedExPackageIsUsingIndirectSignature()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.Indirect;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.INDIRECT));
        }

        [Fact]
        public void Manipulate_OptionTypeIsDirect_WhenFedExPackageIsUsingDirectSignature()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.Direct;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.DIRECT));
        }

        [Fact]
        public void Manipulate_OptionTypeIsAdult_WhenFedExPackageIsUsingAdultSignature()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.Adult;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.ADULT));
        }

        [Fact]
        public void Manipulate_OptionTypeIsNone_WhenFedExPackageIsUsingNoneRequiredSignature()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.NoSignature;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.OptionType == SignatureOptionType.NO_SIGNATURE_REQUIRED));
        }

        [Fact]
        public void Manipulate_SignatureReleaseNumberMatchesFedExAccount_WhenNotUsingServiceDefault()
        {
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(shipment))
                .Returns(new FedExAccountEntity { SignatureRelease = "Foo" });

            shipment.FedEx.Signature = (int) FedExSignatureType.NoSignature;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SignatureOptionDetail.SignatureReleaseNumber == "Foo"));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsSignatureOption_WhenNotUsingServiceDefault()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.NoSignature;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.SIGNATURE_OPTION)));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsNonStandardContainer_WhenShipmentIsHomeDelivery_AndNonStandardContainer()
        {
            shipment.FedEx.NonStandardContainer = true;
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsNonStandardContainer_WhenShipmentIsGround_AndNonStandardContainer()
        {
            shipment.FedEx.NonStandardContainer = true;
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.True(lineItems.All(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsNotGroundOrHomeDelivery()
        {
            shipment.FedEx.NonStandardContainer = true;
            shipment.FedEx.Service = (int) FedExServiceType.PriorityOvernight;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsGround_WithoutNonStandardContainer()
        {
            shipment.FedEx.NonStandardContainer = false;
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesDoesNotContainNonStandardContainer_WhenShipmentIsHomeDelivery_WithoutNonStandardContainer()
        {
            shipment.FedEx.NonStandardContainer = false;
            shipment.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.NON_STANDARD_CONTAINER)));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsAlcohol_WhenOnePackageContainsAlcohol()
        {
            shipment.FedEx.Packages[0].ContainsAlcohol = true;
            shipment.FedEx.Packages[1].ContainsAlcohol = false;

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(1, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }

        [Fact]
        public void Manipulate_SpecialServicesContainsAlcohol_WhenAllPackagesContainsAlcohol()
        {
            foreach (FedExPackageEntity package in shipment.FedEx.Packages)
            {
                package.ContainsAlcohol = true;
            }

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(shipment.FedEx.Packages.Count, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }

        [Fact]
        public void Manipulate_SpecialServicesDoesNotContainAlcohol_WhenZeroPackagesContainAlcohol()
        {
            foreach (FedExPackageEntity package in shipment.FedEx.Packages)
            {
                package.ContainsAlcohol = false;
            }

            var result = testObject.Manipulate(shipment, new RateRequest());

            RequestedPackageLineItem[] lineItems = result.RequestedShipment.RequestedPackageLineItems;
            Assert.Equal(0, lineItems.Count(i => i.SpecialServicesRequested.SpecialServiceTypes.Contains(PackageSpecialServiceType.ALCOHOL)));
        }
    }
}
