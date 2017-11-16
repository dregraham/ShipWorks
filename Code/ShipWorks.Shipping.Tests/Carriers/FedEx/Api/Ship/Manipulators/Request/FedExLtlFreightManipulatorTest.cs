using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExLtlFreightManipulatorTest
    {
        private FedExLtlFreightManipulator testObject;
        private ShipmentEntity shipmentEntity;
        private readonly AutoMock mock;
        private FedExAccountEntity account;

        public FedExLtlFreightManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            account = new FedExAccountEntity()
            {
                CountryCode = "US",
                Street1 = "1 street 1",
                Street2 = "Suite 1",
                City = "Saint Louis",
                StateProvCode = "MO",
                PostalCode = "63102",
            };

            SetupShipment();

            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExFreightEconomy;
            shipmentEntity.FedEx.FreightRole = FedExFreightShipmentRoleType.Consignee;
            shipmentEntity.FedEx.FreightClass = FedExFreightClassType.CLASS_050;
            shipmentEntity.FedEx.FreightCollectTerms = FedExFreightCollectTermsType.NonRecourseShipperSigned;
            shipmentEntity.FedEx.FreightSpecialServices = (int) FedExFreightSpecialServicesType.Poison;

            mock.Mock<IFedExSettingsRepository>()
                .Setup(r => r.GetAccountReadOnly(AnyIShipment))
                .Returns(account);

            testObject = mock.Create<FedExLtlFreightManipulator>();
        }

        private void SetupShipment()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity { WeightUnitType = (int) WeightUnitOfMeasure.Pounds }
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
                DeclaredValue = 64,
                FreightPieces = 1,
                FreightPackaging = FedExFreightPhysicalPackagingType.Bag
            });
        }

        [Fact]
        public void Manipulate_FedExLtlFreightManipulator_ReturnsRequestedShipment()
        {
            var result = testObject.Manipulate(shipmentEntity, new ProcessShipmentRequest(), 0);

            Assert.IsAssignableFrom<RequestedShipment>(result.Value.RequestedShipment);
            Assert.Null(result.Value.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_FedExLtlFreightManipulator_ReturnsLtlFreight_WhenServiceIsNotFreight()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;
            var result = testObject.Manipulate(shipmentEntity, new ProcessShipmentRequest(), 0);

            Assert.NotNull(result.Value.RequestedShipment);
        }

        [Theory]
        [InlineData(FedExServiceType.FedExFreightEconomy, true)]
        [InlineData(FedExServiceType.FedExFreightPriority, true)]
        [InlineData(FedExServiceType.PriorityOvernight, false)]
        [InlineData(FedExServiceType.GroundHomeDelivery, false)]
        [InlineData(FedExServiceType.FedEx1DayFreight, false)]
        [InlineData(FedExServiceType.FirstFreight, false)]
        public void ShouldApply_ReturnsCorrectValue_ForFedExProcessShipmentRequestOption(FedExServiceType service, bool expectedValue)
        {
            shipmentEntity.FedEx.Service = (int) service;

            bool result = testObject.ShouldApply(shipmentEntity, 0);

            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void Manipulate_FedExLtlFreightManipulator_ReturnsRequestedUSValues()
        {
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "US";

            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int) FedExServiceType.FedExFreightEconomy;

            var result = testObject.Manipulate(shipmentEntity, new ProcessShipmentRequest(), 0);

            FreightShipmentDetail freightShipmentDetail = result.Value.RequestedShipment.FreightShipmentDetail;

            Assert.Equal(account.AccountNumber, freightShipmentDetail.FedExFreightAccountNumber);
            Assert.Equal(account.CountryCode, freightShipmentDetail.FedExFreightBillingContactAndAddress.Address.CountryCode);
            Assert.Equal(EnumHelper.GetApiValue<FreightShipmentRoleType>(fedEx.FreightRole), freightShipmentDetail.Role);
            Assert.True(freightShipmentDetail.RoleSpecified);
            Assert.Equal(EnumHelper.GetApiValue<FreightCollectTermsType>(fedEx.FreightCollectTerms), freightShipmentDetail.CollectTermsType);
            Assert.True(freightShipmentDetail.CollectTermsTypeSpecified);
            Assert.Equal(fedEx.FreightTotalHandlinUnits.ToString(), freightShipmentDetail.TotalHandlingUnits);

            List<ShipmentSpecialServiceType> specialServiceTypes = result.Value.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();

            Assert.Equal(1, specialServiceTypes.Count);
            Assert.Equal(1, specialServiceTypes.Count(sst => sst == ShipmentSpecialServiceType.POISON));
        }

        [Fact]
        public void Manipulate_FedExLtlFreightManipulator_AppliesSinglePackage()
        {
            int sequence = 0;
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "US";

            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int) FedExServiceType.FedExFreightEconomy;

            var result = testObject.Manipulate(shipmentEntity, new ProcessShipmentRequest(), sequence);

            FreightShipmentDetail freightShipmentDetail = result.Value.RequestedShipment.FreightShipmentDetail;

            Assert.Equal(1, freightShipmentDetail.LineItems.Length);

            var package = fedEx.Packages.ElementAt(sequence);
            FreightShipmentLineItem lineItem = freightShipmentDetail.LineItems.Where(li => li.Description == $"Freight Package {sequence + 1}").FirstOrDefault();

            Assert.NotNull(lineItem);
            Assert.Equal(EnumHelper.GetApiValue<FreightClassType>(fedEx.FreightClass), lineItem.FreightClass);
            Assert.True(lineItem.FreightClassSpecified);
            Assert.NotNull(lineItem.Dimensions);
            Assert.Equal(EnumHelper.GetApiValue<PhysicalPackagingType>(package.FreightPackaging), lineItem.Packaging);
            Assert.True(lineItem.PackagingSpecified);
            Assert.Equal(package.FreightPieces.ToString(), lineItem.Pieces);
            Assert.NotNull(lineItem.Weight);
        }

        [Fact]
        public void Manipulate_FedExLtlFreightManipulator_AppliesMultiplePackages()
        {
            int sequence = 0;
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "US";

            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Packages.Add(new FedExPackageEntity()
            {
                DimsLength = 3,
                DimsWidth = 6,
                DimsHeight = 12,
                // total weight should be 72
                DimsWeight = 24,
                DimsAddWeight = true,
                Weight = 48,
                DeclaredValue = 96,
                FreightPieces = 10,
                FreightPackaging = FedExFreightPhysicalPackagingType.Pail
            });

            fedEx.Service = (int)FedExServiceType.FedExFreightEconomy;

            var result = testObject.Manipulate(shipmentEntity, new ProcessShipmentRequest(), sequence);

            FreightShipmentDetail freightShipmentDetail = result.Value.RequestedShipment.FreightShipmentDetail;

            Assert.Equal(2, freightShipmentDetail.LineItems.Length);

            var package = fedEx.Packages.ElementAt(sequence);
            FreightShipmentLineItem lineItem = freightShipmentDetail.LineItems.Where(li => li.Description == $"Freight Package {sequence + 1}").FirstOrDefault();

            Assert.NotNull(lineItem);
            Assert.Equal(EnumHelper.GetApiValue<FreightClassType>(fedEx.FreightClass), lineItem.FreightClass);
            Assert.True(lineItem.FreightClassSpecified);
            Assert.NotNull(lineItem.Dimensions);
            Assert.Equal(EnumHelper.GetApiValue<PhysicalPackagingType>(package.FreightPackaging), lineItem.Packaging);
            Assert.True(lineItem.PackagingSpecified);
            Assert.Equal(package.FreightPieces.ToString(), lineItem.Pieces);
            Assert.NotNull(lineItem.Weight);

            sequence++;
            package = fedEx.Packages.ElementAt(sequence);
            lineItem = freightShipmentDetail.LineItems.Where(li => li.Description == $"Freight Package {sequence + 1}").FirstOrDefault();

            Assert.NotNull(lineItem);
            Assert.Equal(EnumHelper.GetApiValue<FreightClassType>(fedEx.FreightClass), lineItem.FreightClass);
            Assert.True(lineItem.FreightClassSpecified);
            Assert.NotNull(lineItem.Dimensions);
            Assert.Equal(EnumHelper.GetApiValue<PhysicalPackagingType>(package.FreightPackaging), lineItem.Packaging);
            Assert.True(lineItem.PackagingSpecified);
            Assert.Equal(package.FreightPieces.ToString(), lineItem.Pieces);
            Assert.NotNull(lineItem.Weight);
        }
    }
}




