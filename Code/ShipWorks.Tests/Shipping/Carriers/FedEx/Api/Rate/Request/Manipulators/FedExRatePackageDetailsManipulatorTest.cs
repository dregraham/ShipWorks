using System;
using System.Collections.Generic;
using Interapptive.Shared.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators
{
    [TestClass]
    public class FedExRatePackageDetailsManipulatorTest
    {
        private FedExRatePackageDetailsManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        
        [TestInitialize]
        public void Initiliaze()
        {
            shipmentEntity = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity { WeightUnitType = (int)WeightUnitOfMeasure.Pounds }
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

            shipmentEntity.FedEx.Packages.Add( new FedExPackageEntity()
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

            // Return a FedEx account that has been migrated
            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { CountryCode = "US" });

            testObject = new FedExRatePackageDetailsManipulator(new FedExSettings(settingsRepository.Object));
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
        public void Manipulate_SetsPackageCount_BasedOnFedExPackageCount_Test()
        {
            testObject.Manipulate(carrierRequest.Object);
            
            // Hard coding to two to match the count in the initialize method
            Assert.AreEqual("2", nativeRequest.RequestedShipment.PackageCount);
        }

        [TestMethod]
        public void Manipulate_PackageCountIsTwo_TwoPacakgesInShipment_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(carrierRequest.Object.ShipmentEntity.FedEx.Packages.Count, 2, "Test request Expected to have two packages");
            Assert.AreEqual(nativeRequest.RequestedShipment.PackageCount, "2");
        }

        [TestMethod]
        public void Manipulate_DimensionsSetProperly_TwoPackagesWithDimensionsInShipment_AndDimensionsIsInchesTest()
        {
            shipmentEntity.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.IN;
            shipmentEntity.FedEx.PackagingType = (int) FedExPackagingType.Custom;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual((int)FedExPackagingType.Custom, carrierRequest.Object.ShipmentEntity.FedEx.PackagingType, "Expecting Test Data PackagingType to be Custom");
            CompareDimensions(carrierRequest.Object.ShipmentEntity, carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [TestMethod]
        public void Manipulate_DimensionsSetProperly_TwoPackagesWithDimensionsInShipment_AndDimensionsIsCentimetersTest()
        {
            shipmentEntity.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.CM;
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Custom;

            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual((int)FedExPackagingType.Custom, carrierRequest.Object.ShipmentEntity.FedEx.PackagingType, "Expecting Test Data PackagingType to be Custom");
            CompareDimensions(carrierRequest.Object.ShipmentEntity, carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [TestMethod]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndUnitsIsLB_Test()
        {
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;

            testObject.Manipulate(carrierRequest.Object);

            ValidateWeight(carrierRequest.Object.ShipmentEntity, carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [TestMethod]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndUnitsIsKG_Test()
        {
            shipmentEntity.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Kilograms;
            testObject.Manipulate(carrierRequest.Object);

            ValidateWeight(carrierRequest.Object.ShipmentEntity, carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [TestMethod]
        public void Manipulate_InsuredValueSetProperly_TwoPacakgesWithInsuredValue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            ValidateValue(carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [TestMethod]
        public void Manipulate_InsuredValueAmountSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.AmountSpecified);
        }

        private void ValidateValue(FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackageLineItem)
        {
            Assert.AreEqual("USD", requestedPackageLineItem.InsuredValue.Currency.ToString());

            Assert.AreEqual(fedExPackageEntity.DeclaredValue, requestedPackageLineItem.InsuredValue.Amount);
        }

        /// <summary>
        /// Compare weight of request and entity
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="fedExPackageEntity">The fed ex package entity.</param>
        /// <param name="requestedPackageLineItem">The requested package line item.</param>
        private void ValidateWeight(ShipmentEntity shipment, FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackageLineItem)
        {
            decimal packageEntityWeight = FedExUtility.GetPackageTotalWeight(fedExPackageEntity);
            Assert.AreEqual(packageEntityWeight, requestedPackageLineItem.Weight.Value);

            if (shipment.FedEx.WeightUnitType == (int) WeightUnitOfMeasure.Pounds)
            {
                Assert.AreEqual(WeightUnits.LB, requestedPackageLineItem.Weight.Units);
            }
            else
            {
                Assert.AreEqual(WeightUnits.KG, requestedPackageLineItem.Weight.Units);
            }
        }

        /// <summary>
        /// Compare dimensions of request and entity
        /// </summary>
        private static void CompareDimensions(ShipmentEntity shipment, FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackage)
        {
            if (shipment.FedEx.LinearUnitType == (int)FedExLinearUnitOfMeasure.CM)
            {
                Assert.AreEqual(LinearUnits.CM, requestedPackage.Dimensions.Units);
            }
            else
            {
                Assert.AreEqual(LinearUnits.IN, requestedPackage.Dimensions.Units);
            }
            Assert.AreEqual(fedExPackageEntity.DimsLength.ToString(), requestedPackage.Dimensions.Length);
            Assert.AreEqual(fedExPackageEntity.DimsWidth.ToString(), requestedPackage.Dimensions.Width);
            Assert.AreEqual(fedExPackageEntity.DimsHeight.ToString(), requestedPackage.Dimensions.Height);
        }
    }
}
