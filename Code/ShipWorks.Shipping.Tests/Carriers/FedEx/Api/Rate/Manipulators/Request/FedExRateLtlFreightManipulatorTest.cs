using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.FedEx;


namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateLtlFreightManipulatorTest
    {
        private FedExRateLtlFreightManipulator testObject;

        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private Mock<IFedExSettingsRepository> settingsRepository;
        private FedExAccountEntity account;

        public FedExRateLtlFreightManipulatorTest()
        {
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
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExFreightEconomy;
            shipmentEntity.FedEx.FreightRole = FedExFreightShipmentRoleType.Consignee;
            shipmentEntity.FedEx.FreightClass = FedExFreightClassType.CLASS_050;
            shipmentEntity.FedEx.FreightCollectTerms = FedExFreightCollectTermsType.NonRecourseShipperSigned;
            shipmentEntity.FedEx.FreightSpecialServices = (int) FedExFreightSpecialServicesType.Poison;

            nativeRequest = new RateRequest();

            // Return a FedEx account that has been migrated
            settingsRepository = new Mock<IFedExSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);
            settingsRepository.Setup(r => r.GetAccountReadOnly(It.IsAny<ShipmentEntity>())).Returns(account);
            settingsRepository.Setup(r => r.IsInterapptiveUser).Returns(false);

            testObject = new FedExRateLtlFreightManipulator(settingsRepository.Object);
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

            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity()
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

            nativeRequest = new RateRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[0]
                }
            };
        }

        [Fact]
        public void Manipulate_FedExLtlFreightManipulator_ReturnsRequestedShipment()
        {
            testObject.Manipulate(shipmentEntity, nativeRequest);

            Assert.IsAssignableFrom<RequestedShipment>(nativeRequest.RequestedShipment);
            Assert.Null(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_FedExLtlFreightManipulator_ReturnsNoLtlFreight_WhenServiceIsNotFreight()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;
            testObject.Manipulate(shipmentEntity, nativeRequest);

            Assert.Null(nativeRequest.RequestedShipment);
        }

        [Theory]
        [InlineData(FedExServiceType.FedExFreightEconomy, true)]
        [InlineData(FedExServiceType.FedExFreightPriority, true)]
        [InlineData(FedExServiceType.FedEx1DayFreight, false)]
        [InlineData(FedExServiceType.FedEx2DayFreight, false)]
        [InlineData(FedExServiceType.FedEx3DayFreight, false)]
        [InlineData(FedExServiceType.FedExNextDayFreight, false)]
        [InlineData(FedExServiceType.FedExGround, false)]
        public void ShouldApply_ReturnsCorrectValue_ForService(FedExServiceType service, bool expectedValue)
        {
            shipmentEntity.FedEx.Service = (int) service;
            bool result = testObject.ShouldApply(shipmentEntity, FedExRateRequestOptions.None);

            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void Manipulate_FedExLtlFreightManipulator_ReturnsRequestedUSValues()
        {
            shipmentEntity.ShipCountryCode = "US";
            shipmentEntity.OriginCountryCode = "US";

            FedExShipmentEntity fedEx = shipmentEntity.FedEx;
            fedEx.Service = (int)FedExServiceType.FedExFreightEconomy;

            testObject.Manipulate(shipmentEntity, nativeRequest);

            FreightShipmentDetail freightShipmentDetail = nativeRequest.RequestedShipment.FreightShipmentDetail;

            Assert.Equal(account.AccountNumber, freightShipmentDetail.FedExFreightAccountNumber);
            Assert.Equal(account.CountryCode, freightShipmentDetail.FedExFreightBillingContactAndAddress.Address.CountryCode);
            Assert.Equal(EnumHelper.GetApiValue<FreightShipmentRoleType>(fedEx.FreightRole), freightShipmentDetail.Role);
            Assert.True(freightShipmentDetail.RoleSpecified);
            Assert.Equal(EnumHelper.GetApiValue<FreightCollectTermsType>(fedEx.FreightCollectTerms), freightShipmentDetail.CollectTermsType);
            Assert.True(freightShipmentDetail.CollectTermsTypeSpecified);
            Assert.Equal(fedEx.FreightTotalHandlinUnits.ToString(), freightShipmentDetail.TotalHandlingUnits);

            Assert.Equal(fedEx.Packages.Count, freightShipmentDetail.LineItems.Length);
            int packageIndex = 1;
            foreach (IFedExPackageEntity package in fedEx.Packages)
            {
                FreightShipmentLineItem lineItem = freightShipmentDetail.LineItems.Where(li => li.Description == $"Freight Package {packageIndex}").FirstOrDefault();

                Assert.NotNull(lineItem);
                Assert.Equal(EnumHelper.GetApiValue<FreightClassType>(fedEx.FreightClass), lineItem.FreightClass);
                Assert.True(lineItem.FreightClassSpecified);
                Assert.NotNull(lineItem.Dimensions);
                Assert.Equal(EnumHelper.GetApiValue<PhysicalPackagingType>(package.FreightPackaging), lineItem.Packaging);
                Assert.True(lineItem.PackagingSpecified);
                Assert.Equal(package.FreightPieces.ToString(), lineItem.Pieces);
                Assert.NotNull(lineItem.Weight);

                packageIndex++;
            }

            List<ShipmentSpecialServiceType> specialServiceTypes = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ToList();

            Assert.Equal(1, specialServiceTypes.Count);
            Assert.Equal(1, specialServiceTypes.Count(sst => sst == ShipmentSpecialServiceType.POISON));
        }
    }
}
