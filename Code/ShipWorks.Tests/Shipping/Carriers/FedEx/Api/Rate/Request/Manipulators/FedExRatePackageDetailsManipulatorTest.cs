using System;
using System.Collections.Generic;
using Interapptive.Shared.Enums;
using Xunit;
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
    public class FedExRatePackageDetailsManipulatorTest
    {
        private FedExRatePackageDetailsManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private RateRequest nativeRequest;
        private ShipmentEntity shipmentEntity;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        public FedExRatePackageDetailsManipulatorTest()
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

            // Return a FedEx account that has been migrated
            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() { CountryCode = "US" });

            testObject = new FedExRatePackageDetailsManipulator(new FedExSettings(settingsRepository.Object));
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRateRequest()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), shipmentEntity, new RateReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }


        [Fact]
        public void Manipulate_SetsPackageCount_BasedOnFedExPackageCount()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Hard coding to two to match the count in the initialize method
            Assert.Equal("2", nativeRequest.RequestedShipment.PackageCount);
        }

        [Fact]
        public void Manipulate_PackageCountIsTwo_TwoPacakgesInShipment()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(carrierRequest.Object.ShipmentEntity.FedEx.Packages.Count, 2);
            Assert.Equal(nativeRequest.RequestedShipment.PackageCount, "2");
        }

        [Fact]
        public void Manipulate_DimensionsSetProperly_TwoPackagesWithDimensionsInShipment_AndDimensionsIsInchesTest()
        {
            shipmentEntity.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.IN;
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Custom;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal((int)FedExPackagingType.Custom, carrierRequest.Object.ShipmentEntity.FedEx.PackagingType);
            CompareDimensions(carrierRequest.Object.ShipmentEntity, carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_DimensionsSetProperly_TwoPackagesWithDimensionsInShipment_AndDimensionsIsCentimetersTest()
        {
            shipmentEntity.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.CM;
            shipmentEntity.FedEx.PackagingType = (int)FedExPackagingType.Custom;

            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal((int)FedExPackagingType.Custom, carrierRequest.Object.ShipmentEntity.FedEx.PackagingType);
            CompareDimensions(carrierRequest.Object.ShipmentEntity, carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndUnitsIsLB()
        {
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Pounds;

            testObject.Manipulate(carrierRequest.Object);

            ValidateWeight(carrierRequest.Object.ShipmentEntity, carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_SetsWeightValueToZeroPointOne_WhenValueIsZero()
        {
            foreach (FedExPackageEntity package in shipmentEntity.FedEx.Packages)
            {
                package.Weight = 0;
                package.DimsWeight = 0;
            }

            testObject.Manipulate(carrierRequest.Object);

            foreach (RequestedPackageLineItem lineItem in ((RateRequest)carrierRequest.Object.NativeRequest).RequestedShipment.RequestedPackageLineItems)
            {
                Assert.Equal(0.1m, lineItem.Weight.Value);
            }

        }

        [Fact]
        public void Manipulate_WeightSetProperly_TwoPackagesWithWeightInShipment_AndUnitsIsKG()
        {
            shipmentEntity.FedEx.WeightUnitType = (int)WeightUnitOfMeasure.Kilograms;
            testObject.Manipulate(carrierRequest.Object);

            ValidateWeight(carrierRequest.Object.ShipmentEntity, carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_InsuredValueSetProperly_TwoPacakgesWithInsuredValue()
        {
            testObject.Manipulate(carrierRequest.Object);

            ValidateValue(carrierRequest.Object.ShipmentEntity.FedEx.Packages[0], nativeRequest.RequestedShipment.RequestedPackageLineItems[0]);
        }

        [Fact]
        public void Manipulate_InsuredValueAmountSpecifiedIsTrue()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.AmountSpecified);
        }

        private void ValidateValue(FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackageLineItem)
        {
            Assert.Equal("USD", requestedPackageLineItem.InsuredValue.Currency.ToString());

            Assert.Equal(fedExPackageEntity.DeclaredValue, requestedPackageLineItem.InsuredValue.Amount);
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
            Assert.Equal(packageEntityWeight, requestedPackageLineItem.Weight.Value);

            if (shipment.FedEx.WeightUnitType == (int)WeightUnitOfMeasure.Pounds)
            {
                Assert.Equal(WeightUnits.LB, requestedPackageLineItem.Weight.Units);
            }
            else
            {
                Assert.Equal(WeightUnits.KG, requestedPackageLineItem.Weight.Units);
            }
        }

        /// <summary>
        /// Compare dimensions of request and entity
        /// </summary>
        private static void CompareDimensions(ShipmentEntity shipment, FedExPackageEntity fedExPackageEntity, RequestedPackageLineItem requestedPackage)
        {
            if (shipment.FedEx.LinearUnitType == (int)FedExLinearUnitOfMeasure.CM)
            {
                Assert.Equal(LinearUnits.CM, requestedPackage.Dimensions.Units);
            }
            else
            {
                Assert.Equal(LinearUnits.IN, requestedPackage.Dimensions.Units);
            }
            Assert.Equal(fedExPackageEntity.DimsLength.ToString(), requestedPackage.Dimensions.Length);
            Assert.Equal(fedExPackageEntity.DimsWidth.ToString(), requestedPackage.Dimensions.Width);
            Assert.Equal(fedExPackageEntity.DimsHeight.ToString(), requestedPackage.Dimensions.Height);
        }
    }
}
