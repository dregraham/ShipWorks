using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExPackageSpecialServicesManipulatorTest
    {
        private const string signatureRelease = "SigRelNum";
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly FedExPackageSpecialServicesManipulator testObject;
        private readonly FedExAccountEntity account;

        public FedExPackageSpecialServicesManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipment.FedEx.Packages[0].ContainsAlcohol = false;
            shipment.FedEx.Packages[1].ContainsAlcohol = false;

            account = new FedExAccountEntity() { SignatureRelease = signatureRelease };
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(account);

            testObject = mock.Create<FedExPackageSpecialServicesManipulator>();
        }

        [Theory]
        [InlineData(FedExServiceType.FedExFreightEconomy, false)]
        [InlineData(FedExServiceType.FedExFreightPriority, false)]
        [InlineData(FedExServiceType.FedExGround, true)]
        [InlineData(FedExServiceType.PriorityOvernight, true)]
        public void ShouldApply_ReturnsAppropriateValue_ForGivenInput(FedExServiceType service, bool expected)
        {
            shipment.FedEx.Service = (int) service;
            var result = testObject.ShouldApply(shipment, 0);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Manipulate_SignatureOptionAdded_FedExShipmentSignatureSetToNoSignature()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(
                FedExSignatureType.NoSignature,
                (FedExSignatureType) shipment.FedEx.Signature);

            Assert.Equal(
                SignatureOptionType.NO_SIGNATURE_REQUIRED,
                result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail.OptionType);

            Assert.Equal(
                signatureRelease,
                result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail.SignatureReleaseNumber);

            Assert.True(result.Value
                .RequestedShipment
                .RequestedPackageLineItems[0]
                .SpecialServicesRequested
                .SpecialServiceTypes
                .Contains(PackageSpecialServiceType.SIGNATURE_OPTION));
        }

        [Fact]
        public void Manipulate_SignatureOptionNotAdded_FedExShipmentSignatureSetToServiceDefault()
        {
            shipment.FedEx.Signature = (int) FedExSignatureType.ServiceDefault;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Null(result.Value.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SignatureOptionDetail);

            Assert.False(result.Value
                .RequestedShipment
                .RequestedPackageLineItems[0]
                .SpecialServicesRequested
                .SpecialServiceTypes
                .Contains(PackageSpecialServiceType.SIGNATURE_OPTION));
        }

        [Fact]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackageAndRequestSpecialServicesRequestedIsNull()
        {
            shipment.FedEx.Packages.RemoveAt(1);
            shipment.FedEx.Packages[0].ContainsAlcohol = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, AlcoholCount(result.Value));
        }

        [Fact]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackageAndSpecialServiceTypesIsNull()
        {
            shipment.FedEx.Packages.RemoveAt(1);
            shipment.FedEx.Packages[0].ContainsAlcohol = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, AlcoholCount(result.Value));
        }

        [Fact]
        public void Manipulate_AlcoholPackageWithAlcoholIsZero_ShipmentPackageEntityHasZeroAlcoholPackage()
        {
            shipment.FedEx.Packages.RemoveAt(1);

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(0, AlcoholCount(result.Value));
        }

        [Fact]
        public void Manipulate_AlcoholPackageWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackage()
        {
            shipment.FedEx.Packages.RemoveAt(1);
            shipment.FedEx.Packages[0].ContainsAlcohol = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, AlcoholCount(result.Value));
        }

        [Fact]
        public void Manipulate_AlcoholPackagesWithAlcoholIsZero_ShipmentPackageEntityHasZeroAlcoholPackages()
        {
            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(0, AlcoholCount(result.Value));
        }

        [Fact]
        public void Manipulate_AlcoholPackagesWithAlcoholIsOne_ShipmentPackageEntityHasOneAlcoholPackages()
        {
            shipment.FedEx.Packages[0].ContainsAlcohol = true;

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal(1, AlcoholCount(result.Value));
        }

        /// <summary>
        /// Determines the number of packages with Alcohol specified.
        /// </summary>
        /// <returns>Returns the number of packages with Alcohol specified.</returns>
        private int AlcoholCount(ProcessShipmentRequest request)
        {
            return request.RequestedShipment.RequestedPackageLineItems
                .Where(rpli => rpli.SpecialServicesRequested != null && rpli.SpecialServicesRequested.SpecialServiceTypes != null)
                .SelectMany(rpli => rpli.SpecialServicesRequested.SpecialServiceTypes)
                    .Count(sst => sst == PackageSpecialServiceType.ALCOHOL);
        }
    }
}
