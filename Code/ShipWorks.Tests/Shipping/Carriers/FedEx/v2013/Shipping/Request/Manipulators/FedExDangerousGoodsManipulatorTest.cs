﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx;
using Interapptive.Shared.Utility;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Shipping.Request.Manipulators
{
    [TestClass]
    public class FedExDangerousGoodsManipulatorTest
    {
        private FedExDangerousGoodsManipulator testObject;

        private ShipmentEntity shipmentEntity;
        private ProcessShipmentRequest nativeRequest;
        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            FedExPackageEntity package = new FedExPackageEntity
            {
                DangerousGoodsEnabled = true,
                DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible,
                DangerousGoodsEmergencyContactPhone = "555-555-5555",
                DangerousGoodsOfferor = "some offeror",
                DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.LithiumBatteries,
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotProcessShipmentRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new CancelPendingShipmentRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedShipment_Test()
        {
            // Setup the test by configuring the native request to have a null requested shipment property and re-initialize
            // the carrier request with the updated native request
            nativeRequest.RequestedShipment = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested shipment property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullRequestedPackageLineItems_Test()
        {
            // Setup the test by configuring the native request to have a null requested package line items
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems);
        }

        [TestMethod]
        public void Manipulate_AccountsForEmptyRequestedPackageLineItems_Test()
        {
            // Setup the test by configuring the native request to have an empty arrary for the requested 
            // package line items property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[0];
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The requested package line items property should have one item in the array
            Assert.AreEqual(1, nativeRequest.RequestedShipment.RequestedPackageLineItems.Length);
        }

        [TestMethod]
        public void Manipulate_AccountsForNullSpecialServicesRequested_Test()
        {
            // Setup the test by configuring the native request to a null value for the customer references
            // property and re-initialize the carrier request with the updated native request
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = null;
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, nativeRequest);

            testObject.Manipulate(carrierRequest.Object);

            // The special services property should be created now
            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested);
        }

        [TestMethod]
        public void Manipulate_DangerousGoodsDetailPropertyIsNotNull_WhenDangerousGoodsEnabledIsTrue_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsEnabled = true;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail);
        }

        [TestMethod]
        public void Manipulate_DangerousGoodsDetailPropertyIsNull_WhenDangerousGoodsEnabledIsFalse_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsEnabled = false;

            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNull(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail);
        }

        [TestMethod]
        public void Manipulate_AddsDangerousGoodsOption_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            PackageSpecialServicesRequested servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.AreEqual(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }

        [TestMethod]
        public void Manipulate_AddsDangerousGoodsOption_WhenSpecialServicesRequestedIsNull_Test()
        {
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested = null;

            testObject.Manipulate(carrierRequest.Object);

            PackageSpecialServicesRequested servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.AreEqual(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }


        [TestMethod]
        public void Manipulate_AddsDangerousGoodsOption_WhenServiceTypesIsNull_Test()
        {
            nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes = null;

            testObject.Manipulate(carrierRequest.Object);

            PackageSpecialServicesRequested servicesRequested = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested;
            Assert.AreEqual(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[0]);
        }

        [TestMethod]
        public void Manipulate_AddsDangerousGoodsOption_WhenServiceTypesIsNotEmpty_Test()
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
            Assert.AreEqual(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[2]);
        }

        [TestMethod]
        public void Manipulate_AddsDangerousGoodsOption_AndRetainsExistingServiceTypes_WhenServiceTypesIsNotEmpty_Test()
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
            Assert.AreEqual(PackageSpecialServiceType.COD, servicesRequested.SpecialServiceTypes[0]);
            Assert.AreEqual(PackageSpecialServiceType.DRY_ICE, servicesRequested.SpecialServiceTypes[1]);
            Assert.AreEqual(PackageSpecialServiceType.DANGEROUS_GOODS, servicesRequested.SpecialServiceTypes[2]);
        }

        [TestMethod]
        public void Manipulate_AccessibilityTypeIsAccessible_WhenDangerousGoodsTypeIsLithiumBatteries_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.LithiumBatteries;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int) FedExDangerousGoodsAccessibilityType.Accessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(DangerousGoodsAccessibilityType.ACCESSIBLE, dangerousGoods.Accessibility);
        }

        [TestMethod]
        public void Manipulate_AccessibilityTypeIsInAccessible_WhenDangerousGoodsTypeIsLithiumBatteries_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.LithiumBatteries;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(DangerousGoodsAccessibilityType.INACCESSIBLE, dangerousGoods.Accessibility);
        }

        [TestMethod]
        public void Manipulate_AccessibilitySpecifiedIsTrue_WhenDangerousGoodsTypeIsLithiumBatteries_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.LithiumBatteries;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsTrue(dangerousGoods.AccessibilitySpecified);
        }

        [TestMethod]
        public void Manipulate_AccessibilityTypeIsAccessible_WhenDangerousGoodsTypeIsORMD_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.OrmD;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Accessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(DangerousGoodsAccessibilityType.ACCESSIBLE, dangerousGoods.Accessibility);
        }

        [TestMethod]
        public void Manipulate_AccessibilityTypeIsInAccessible_WhenDangerousGoodsTypeIsORMD_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.OrmD;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(DangerousGoodsAccessibilityType.INACCESSIBLE, dangerousGoods.Accessibility);
        }

        [TestMethod]
        public void Manipulate_AccessibilitySpecifiedIsFalse_WhenDangerousGoodsTypeIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsFalse(dangerousGoods.AccessibilitySpecified);
        }
        
        [TestMethod]
        public void Manipulate_AccessibilitySpecifiedIsTrue_WhenDangerousGoodsTypeIsORMD_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.OrmD;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsAccessibilityType = (int)FedExDangerousGoodsAccessibilityType.Inaccessible;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsTrue(dangerousGoods.AccessibilitySpecified);
        }

        [TestMethod]
        public void Manipulate_CargoAircraftIsTrue_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsCargoAircraftOnly = true;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsTrue(dangerousGoods.CargoAircraftOnly);
        }

        [TestMethod]
        public void Manipulate_CargoAircraftIsFalse_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsCargoAircraftOnly = false;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsFalse(dangerousGoods.CargoAircraftOnly);
        }

        [TestMethod]
        public void Manipulate_CargoAircraftSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsTrue(dangerousGoods.CargoAircraftOnlySpecified);
        }

        [TestMethod]
        public void Manipulate_EmergencyContactNumber_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsEmergencyContactPhone = "123-4565-7890";

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual("123-4565-7890", dangerousGoods.EmergencyContactNumber);
        }

        [TestMethod]
        public void Manipulate_Offeror_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsOfferor = "the offeror";

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual("the offeror", dangerousGoods.Offeror);
        }

        [TestMethod]
        public void Manipulate_OptionArrayHasOneItem_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(1, dangerousGoods.Options.Length);
        }

        [TestMethod]
        public void Manipulate_OptionIsNotApplicable_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.NotApplicable;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsNull(dangerousGoods.Options);
        }

        [TestMethod]
        public void Manipulate_OptionIsLithiumBattery_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int) FedExDangerousGoodsMaterialType.LithiumBatteries;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(HazardousCommodityOptionType.LITHIUM_BATTERY_EXCEPTION, dangerousGoods.Options[0]);
        }

        [TestMethod]
        public void Manipulate_OptionIsOrmD_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.OrmD;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(HazardousCommodityOptionType.ORM_D, dangerousGoods.Options[0]);
        }

        [TestMethod]
        public void Manipulate_OptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(HazardousCommodityOptionType.HAZARDOUS_MATERIALS, dangerousGoods.Options[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsHome_Test()
        {
            shipmentEntity.FedEx.Service = (int) FedExServiceType.GroundHomeDelivery;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsOneDayFreight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx1DayFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsTwoDay_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx2Day;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsTwoDayAM_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx2DayAM;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsTwoDayFreight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx2DayFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsThreeDayFreight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedEx3DayFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsExpressSaver_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExExpressSaver;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsFirstInternationalPriorityEurope_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExEuropeFirstInternationalPriority;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsOvernight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FirstOvernight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalEconomy_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalEconomy;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalEconomyFreight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalEconomyFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalFirst_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalFirst;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalPriority_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalPriority;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsInternationalPriorityFreight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.InternationalPriorityFreight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsPriorityOvernight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.PriorityOvernight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsSmartPost_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.SmartPost;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(FedExException))]
        public void Manipulate_ThrowsFedExException_WhenOptionIsHazardousMaterialsAndServiceIsStandardOvernight_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.StandardOvernight;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenUnrecognizedOptionTypeIsProvided_Test()
        {

            shipmentEntity.FedEx.Packages[0].DangerousGoodsType= 23;

            testObject.Manipulate(carrierRequest.Object);  
        }

        [TestMethod]
        public void Manipulate_ContainerIsNull_WhenOptionIsNotHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.LithiumBatteries;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsNull(dangerousGoods.Containers);
        }

        [TestMethod]
        public void Manipulate_ContainerContainsOneItem_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(1, dangerousGoods.Containers.Length);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesContainsOneItem_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;           
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(1, dangerousGoods.Containers[0].HazardousCommodities.Length);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesContentDescriptionIsNotNull_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsNotNull(dangerousGoods.Containers[0].HazardousCommodities[0]);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesContentDescriptionId_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialNumber = "UN2533";
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual("UN2533", dangerousGoods.Containers[0].HazardousCommodities[0].Description.Id);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesContentDescriptionHazardClass_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialClass = "6.1";
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual("6.1", dangerousGoods.Containers[0].HazardousCommodities[0].Description.HazardClass);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesProperShippingName_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;            
            shipmentEntity.FedEx.Packages[0].HazardousMaterialProperName = "Methyl trichloroacetate";
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual("Methyl trichloroacetate", dangerousGoods.Containers[0].HazardousCommodities[0].Description.ProperShippingName);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesPackingGroupIsDefault_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.Default;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(HazardousCommodityPackingGroupType.DEFAULT, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesPackingGroupIsI_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.I;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(HazardousCommodityPackingGroupType.I, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesPackingGroupIsII_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.II;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(HazardousCommodityPackingGroupType.II, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesPackingGroupIsIII_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.III;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(HazardousCommodityPackingGroupType.III, dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroup);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Manipulate_ThrowsInvalidOperationException_WhenHazardousCommoditiesPackingGroupIsNotRecognized_AndOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = 45;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_HazardousCommoditiesPackingGroupSpecifiedIsTrue_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.I;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsTrue(dangerousGoods.Containers[0].HazardousCommodities[0].Description.PackingGroupSpecified);
        }

        [TestMethod]
        public void Manipulate_QuantityAmount_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(2.4M, dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.Amount);
        }

        [TestMethod]
        public void Manipulate_QuantityAmountSpecifiedIsTrue_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsTrue(dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.AmountSpecified);
        }

        [TestMethod]
        public void Manipulate_QuantityUnits_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuantityValue = 2.4;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuanityUnits = (int) FedExHazardousMaterialsQuantityUnits.Kilogram;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual("KG", dangerousGoods.Containers[0].HazardousCommodities[0].Quantity.Units);
        }

        [TestMethod]
        public void Manipulate_PackagingCount_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsPackagingCount = 1;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual("1", dangerousGoods.Packaging.Count);
        }

        [TestMethod]
        public void Manipulate_PackagingUnits_WhenOptionIsHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExGround;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.HazardousMaterials;
            shipmentEntity.FedEx.Packages[0].HazardousMaterialQuanityUnits = (int)FedExHazardousMaterialsQuantityUnits.Liters;
            shipmentEntity.FedEx.Packages[0].DangerousGoodsPackagingCount = 1;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.AreEqual(EnumHelper.GetDescription(FedExHazardousMaterialsQuantityUnits.Liters), dangerousGoods.Packaging.Units);
        }

        [TestMethod]
        public void Manipulate_PackagingIsNull_WhenOptionIsNotHazardousMaterials_Test()
        {
            shipmentEntity.FedEx.Packages[0].DangerousGoodsType = (int)FedExDangerousGoodsMaterialType.LithiumBatteries;

            testObject.Manipulate(carrierRequest.Object);

            DangerousGoodsDetail dangerousGoods = nativeRequest.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.DangerousGoodsDetail;
            Assert.IsNull(dangerousGoods.Packaging);
        }


        [TestMethod]
        public void Manipulate_UsesSequenceNumberOnRequest_WhenDangerousGoodsEnabled_Test()
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
            Assert.IsNotNull(dangerousGoods);
        }

        [TestMethod]
        public void Manipulate_UsesSequenceNumberOnRequest_WhenDangerousGoodsNotEnabled_Test()
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
            Assert.IsNull(dangerousGoods);
        }
    }
}
