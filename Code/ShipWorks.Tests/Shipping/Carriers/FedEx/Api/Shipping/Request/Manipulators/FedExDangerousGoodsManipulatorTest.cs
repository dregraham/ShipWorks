using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    public class FedExDangerousGoodsManipulatorTest
    {
        private FedExDangerousGoodsManipulator testObject;

        private ShipmentEntity shipmentEntity;
        private ProcessShipmentRequest nativeRequest;
        private Mock<CarrierRequest> carrierRequest;

        public FedExDangerousGoodsManipulatorTest()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            FedExPackageEntity package = new FedExPackageEntity
            {
                DangerousGoodsEnabled = true,
                DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Accessible,
                DangerousGoodsEmergencyContactPhone = "555-555-5555",
                DangerousGoodsOfferor = "some offeror",
                DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.Batteries,
                DangerousGoodsCargoAircraftOnly = false,
            };

            // Add the package containing dangerous goods to the fedex shipment
            shipmentEntity.FedEx.Packages.Add(package);


            // Create an "empty" process shipment request to use for our tests
            nativeRequest = new ProcessShipmentRequest
            {
                RequestedShipment = new RequestedShipment
                {
                    RequestedPackageLineItems = new RequestedPackageLineItem[1]
                    {
                        new RequestedPackageLineItem
                        {
                            SpecialServicesRequested = new PackageSpecialServicesRequested()
                        }
                    }
                }
            };

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);
            carrierRequest.Object.SequenceNumber = 0;

            testObject = new FedExDangerousGoodsManipulator();
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new object());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedPackageLineItems()
        {
            // Setup the test by configuring the native request to have a null requested package line items
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [Fact]
        public void Manipulate_AccountsForEmptyRequestedPackageLineItems()
        {
            // Setup the test by configuring the native request to have an empty arrary for the requested 
            // package line items property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should have one item in the array
            Assert.Equal(1, nativeRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [Fact]
        public void Manipulate_AccountsForNullSpecialServicesRequested()
        {
            // Setup the test by configuring the native request to a null value for the customer references
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The special services property should be created now
            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested);
        }

        [Fact]
        public void Manipulate_DangerousGoodsDetailPropertyIsNotNull_WhenDangerousGoodsEnabledIsTrue()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail);
        }

        [Fact]
        public void Manipulate_DangerousGoodsDetailPropertyIsNull_WhenDangerousGoodsEnabledIsFalse()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsEnabled = false;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Null(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail);
        }

        [Fact]
        public void Manipulate_AddsDangerousGoodsOption()
        {
            testObject.Manipulate(carrierRequest.Object);

            PackageSpecialServicesRequested servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Equal(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsDangerousGoodsOption_WhenSpecialServicesRequestedIsNull()
        {
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            PackageSpecialServicesRequested servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Equal(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }


        [Fact]
        public void Manipulate_AddsDangerousGoodsOption_WhenServiceTypesIsNull()
        {
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(carrierRequest.Object);

            PackageSpecialServicesRequested servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Equal(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }

        [Fact]
        public void Manipulate_AddsDangerousGoodsOption_WhenServiceTypesIsNotEmpty()
        {
            // Add a few service types for this test
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = new PackageSpecialServiceType[]
            {
                PackageSpecialServiceType.COD,
                PackageSpecialServiceType.DRY_ICE
            };

            testObject.Manipulate(carrierRequest.Object);

            // Check that the last service type is the dangerous goods
            PackageSpecialServicesRequested servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Equal(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[2]);
        }

        [Fact]
        public void Manipulate_AddsDangerousGoodsOption_AndRetainsExistingServiceTypes_WhenServiceTypesIsNotEmpty()
        {
            // Add a few service types for this test
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = new PackageSpecialServiceType[]
            {
                PackageSpecialServiceType.COD,
                PackageSpecialServiceType.DRY_ICE
            };

            testObject.Manipulate(carrierRequest.Object);

            // Check that the previous service types are retained
            PackageSpecialServicesRequested servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.Equal(PackageSpecialServiceType.COD, servicesRequested.SpecialServiceTypes[0]);
            Assert.Equal(PackageSpecialServiceType.DRY_ICE, servicesRequested.SpecialServiceTypes[1]);
            Assert.Equal(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[2]);
        }

        [Fact]
        public void Manipulate_AccessibilityTypeIsAccessible_WhenDangerousGoodsTypeIsLithiumBatteries()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.Batteries;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Accessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(DangerousGoodsAccessibilityType.ACCESSIBLE, dangerousGoods.Accessibility);
        }

        [Fact]
        public void Manipulate_AccessibilityTypeIsInAccessible_WhenDangerousGoodsTypeIsLithiumBatteries()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.Batteries;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(DangerousGoodsAccessibilityType.INACCESSIBLE, dangerousGoods.Accessibility);
        }

        [Fact]
        public void Manipulate_AccessibilitySpecifiedIsTrue_WhenDangerousGoodsTypeIsLithiumBatteries()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.Batteries;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.AccessibilitySpecified);
        }

        [Fact]
        public void Manipulate_AccessibilityTypeIsAccessible_WhenDangerousGoodsTypeIsORMD()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.OrmD;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Accessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(DangerousGoodsAccessibilityType.ACCESSIBLE, dangerousGoods.Accessibility);
        }

        [Fact]
        public void Manipulate_AccessibilityTypeIsInAccessible_WhenDangerousGoodsTypeIsORMD()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.OrmD;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(DangerousGoodsAccessibilityType.INACCESSIBLE, dangerousGoods.Accessibility);
        }

        [Fact]
        public void Manipulate_AccessibilitySpecifiedIsFalse_WhenDangerousGoodsTypeIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.False(dangerousGoods.AccessibilitySpecified);
        }

        [Fact]
        public void Manipulate_AccessibilitySpecifiedIsTrue_WhenDangerousGoodsTypeIsORMD()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.OrmD;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.AccessibilitySpecified);
        }

        [Fact]
        public void Manipulate_CargoAircraftIsTrue()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsCargoAircraftOnly = true;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.CargoAircraftOnly);
        }

        [Fact]
        public void Manipulate_CargoAircraftIsFalse()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsCargoAircraftOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.False(dangerousGoods.CargoAircraftOnly);
        }

        [Fact]
        public void Manipulate_CargoAircraftSpecifiedIsTrue()
        {
            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.CargoAircraftOnlySpecified);
        }

        [Fact]
        public void Manipulate_EmergencyContactNumber()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsEmergencyContactPhone = "123-4565-7890";

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("123-4565-7890", dangerousGoods.EmergencyContactNumber);
        }

        [Fact]
        public void Manipulate_Offeror()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsOfferor = "the offeror";

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("the offeror", dangerousGoods.Offeror);
        }

        [Fact]
        public void Manipulate_OptionArrayHasOneItem()
        {
            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(1, dangerousGoods.Options.Length);
        }

        [Fact]
        public void Manipulate_OptionIsNotApplicable()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.NotApplicable;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Null(dangerousGoods.Options);
        }

        [Fact]
        public void Manipulate_OptionIsLithiumBattery()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.Batteries;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityOptionType.BATTERY, dangerousGoods.Options[0]);
        }

        [Fact]
        public void Manipulate_OptionIsOrmD()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.OrmD;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityOptionType.ORM_D, dangerousGoods.Options[0]);
        }

        [Fact]
        public void Manipulate_OptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityOptionType.HAZARDOUS_MATERIALS, dangerousGoods.Options[0]);
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsHome()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.GroundHomeDelivery;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsOneDayFreight()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx1DayFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsTwoDay()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx2Day;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsTwoDayAM()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx2DayAM;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsTwoDayFreight()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx2DayFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsThreeDayFreight()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx3DayFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsExpressSaver()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExExpressSaver;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsFirstInternationalPriorityEurope()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExEuropeFirstInternationalPriority;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsOvernight()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FirstOvernight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalEconomy()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalEconomy;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalEconomyFreight()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalEconomyFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalFirst()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalFirst;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalPriority()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalPriority;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalPriorityFreight()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalPriorityFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsPriorityOvernight()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsSmartPost()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.SmartPost;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsStandardOvernight()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.StandardOvernight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenUnrecognizedOptionTypeIsProvided()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = 23;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ContainerIsNull_WhenOptionIsNotHazardousMaterials()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.Batteries;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Null(dangerousGoods.Containers);
        }

        [Fact]
        public void Manipulate_OP900Set_WhenDangerousGoodsSet()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.ShippingDocumentSpecification.ShippingDocumentTypes.Contains(RequestedShippingDocumentType.OP_900));
            Assert.Equal(ShippingDocumentImageType.PDF, nativeRequest.RequestedShipment.ShippingDocumentSpecification.Op900Detail.Format.ImageType);
            Assert.Equal(ShippingDocumentStockType.OP_900_LL_B, nativeRequest.RequestedShipment.ShippingDocumentSpecification.Op900Detail.Format.StockType);
            Assert.True(nativeRequest.RequestedShipment.ShippingDocumentSpecification.Op900Detail.Format.ImageTypeSpecified);
            Assert.True(nativeRequest.RequestedShipment.ShippingDocumentSpecification.Op900Detail.Format.StockTypeSpecified);
        }

        [Fact]
        public void Manipulate_ContainerContainsOneItem_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(1, dangerousGoods.Containers.Length);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesContainsOneItem_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(1, dangerousGoods.Containers[0].HazardousCommodities.Length);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesContentDescriptionIsNotNull_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.NotNull(dangerousGoods.Containers[0].HazardousCommodities[0]);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesContentDescriptionId_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialNumber = "UN2533";
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("UN2533", dangerousGoods.Containers[0].HazardousCommodities[0].Description.Id);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesContentDescriptionHazardClass_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialClass = "6.1";
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("6.1", dangerousGoods.Containers[0].HazardousCommodities[0].Description.HazardClass);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesProperShippingName_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialProperName = "Methyl trichloroacetate";
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("Methyl trichloroacetate", dangerousGoods.Containers[0].HazardousCommodities[0].Description.ProperShippingName);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsDefault_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.Default;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityPackingGroupType.DEFAULT, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsI_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.I;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityPackingGroupType.I, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsII_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.II;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityPackingGroupType.II, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsIII_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.III;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(HazardousCommodityPackingGroupType.III, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupIsNotSpecified_WhenOptionIsHazardousMaterials_AndPackingGroupIsNotApplicable()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.NotApplicable;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.False(dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroupSpecified);
        }

        [Fact]
        public void Manipulate_ThrowsInvalidOperationException_WhenHazardousCommoditiesPackingGroupIsNotRecognized_AndOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = 45;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            Assert.Throws<InvalidOperationException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_HazardousCommoditiesPackingGroupSpecifiedIsTrue_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.I;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroupSpecified);
        }

        [Fact]
        public void Manipulate_QuantityAmount_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(2.4M, dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.Amount);
        }

        [Fact]
        public void Manipulate_QuantityAmountSpecifiedIsTrue_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.True(dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.AmountSpecified);
        }

        [Fact]
        public void Manipulate_QuantityUnits_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuanityUnits = (int)FedExHazardousMaterialsQuantityUnits.Kilogram;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("KG", dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.Units);
        }

        [Fact]
        public void Manipulate_PackagingCount_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsPackagingCount = 1;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal("1", dangerousGoods.Packaging.Count);
        }

        [Fact]
        public void Manipulate_PackagingUnits_WhenOptionIsHazardousMaterials()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuanityUnits = (int)FedExHazardousMaterialsQuantityUnits.Liters;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsPackagingCount = 1;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Equal(EnumHelper.GetDescription(FedExHazardousMaterialsQuantityUnits.Liters), dangerousGoods.Packaging.Units);
        }

        [Fact]
        public void Manipulate_PackagingIsNull_WhenOptionIsNotHazardousMaterials()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.Batteries;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Null(dangerousGoods.Packaging);
        }


        [Fact]
        public void Manipulate_UsesSequenceNumberOnRequest_WhenDangerousGoodsEnabled()
        {
            // Setup the FedEx shipment to contain multiple packages to test that the 
            // manipulator process the correct package in the shipment when the 
            // sequence number is not zero
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = true });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });

            carrierRequest.Object.SequenceNumber = 2;

            testObject.Manipulate(carrierRequest.Object);

            // Since the sequence number is two, we should have a non-null value for the dangerous goods
            // property since the dangerous goods enabled flag is set to true for the third item in the package list
            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.NotNull(dangerousGoods);
        }

        [Fact]
        public void Manipulate_UsesSequenceNumberOnRequest_WhenDangerousGoodsNotEnabled()
        {
            // Setup the FedEx shipment to contain multiple packages to test that the 
            // manipulator process the correct package in the shipment when the 
            // sequence number is not zero
            shipmentEntity.FedEx.Packages.Clear();
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = true });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });
            shipmentEntity.FedEx.Packages.Add(new FedExPackageEntity { DangerousGoodsEnabled = false });

            carrierRequest.Object.SequenceNumber = 1;

            testObject.Manipulate(carrierRequest.Object);

            // Since the sequence number is one, we should have a non-null value for the dangerous goods
            // property since the dangerous goods enabled flag is set to false for the second item in the package list
            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.Null(dangerousGoods);
        }
    }
}
