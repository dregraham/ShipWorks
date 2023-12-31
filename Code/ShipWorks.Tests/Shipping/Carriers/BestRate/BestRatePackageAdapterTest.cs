﻿using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.BestRate
{
    public class BestRatePackageAdapterTest
    {
        private ShipmentEntity shipment;
        private IPackageAdapter testObject;

        public BestRatePackageAdapterTest()
        {
            PopulateDefaultObjects();
        }

        /// <summary>
        /// To support ShipSense, we do not want to have PackagingType added to the hash of the shipment.BestRate adapter.
        /// </summary>
        [Fact]
        public void Hash_DoesNotContain_PackagingType_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                BestRate = new BestRateShipmentEntity()
            };

            BestRatePackageAdapter testObject = new BestRatePackageAdapter(shipment)
            {
                PackagingType = 999999,
                AdditionalWeight = 3.1,
                ApplyAdditionalWeight = true,
                DimsHeight = 2.2,
                DimsLength = 3.3,
                Weight = 4.4,
                DimsWidth = 5.5
            };

            string firstTry = testObject.HashCode();

            testObject.PackagingType = 0;
            string secondTry = testObject.HashCode();

            // Make sure PackagingType WAS NOT PART OF THE HASH!
            Assert.Equal(firstTry, secondTry);
        }

        [Fact]
        public void Constructor_Throws_WhenBestRateShipmentEntityIsNull_Test()
        {
            shipment.BestRate = null;

            Assert.Throws<ArgumentNullException>(nameof(shipment.BestRate), () => new BestRatePackageAdapter(shipment));
        }

        [Fact]
        public void Constructor_PopulatesValues_Correctly_Test()
        {
            Assert.Equal(1, testObject.Index);
            Assert.Equal(shipment.BestRate.DimsLength, testObject.DimsLength);
            Assert.Equal(shipment.BestRate.DimsWidth, testObject.DimsWidth);
            Assert.Equal(shipment.BestRate.DimsHeight, testObject.DimsHeight);
            Assert.Equal(shipment.BestRate.DimsWeight, testObject.AdditionalWeight);
            Assert.Equal(shipment.BestRate.DimsAddWeight, testObject.ApplyAdditionalWeight);
            Assert.Equal(shipment.BestRate.DimsProfileID, testObject.DimsProfileID);
        }

        [Fact]
        public void Changing_Index_IsStillOne_Test()
        {
            testObject.Index = 3;

            Assert.Equal(1, testObject.Index);
        }

        [Fact]
        public void Changing_Weight_ForSinglePackage_Updates_Correctly_Test()
        {
            testObject.Weight = 88.98;

            Assert.Equal(testObject.Weight, shipment.ContentWeight);
        }

        [Fact]
        public void Changing_AdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.AdditionalWeight = 5.4;

            Assert.Equal(testObject.AdditionalWeight, shipment.BestRate.DimsWeight);
        }

        [Fact]
        public void Changing_ApplyAdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.ApplyAdditionalWeight = !shipment.BestRate.DimsAddWeight;

            Assert.Equal(testObject.ApplyAdditionalWeight, shipment.BestRate.DimsAddWeight);
        }

        [Fact]
        public void Changing_DimsLength_UpdatesCorrectly_Test()
        {
            double newValue = shipment.BestRate.DimsLength + 2.1;
            testObject.DimsLength = shipment.BestRate.DimsLength + newValue;

            Assert.Equal(testObject.DimsLength, shipment.BestRate.DimsLength);
        }

        [Fact]
        public void Changing_DimsWidth_UpdatesCorrectly_Test()
        {
            double newValue = shipment.BestRate.DimsWidth + 2.1;
            testObject.DimsWidth = shipment.BestRate.DimsWidth + newValue;

            Assert.Equal(testObject.DimsWidth, shipment.BestRate.DimsWidth);
        }

        [Fact]
        public void Changing_DimsHeight_UpdatesCorrectly_Test()
        {
            double newValue = shipment.BestRate.DimsHeight + 2.1;
            testObject.DimsHeight = shipment.BestRate.DimsHeight + newValue;

            Assert.Equal(testObject.DimsHeight, shipment.BestRate.DimsHeight);
        }

        [Fact]
        public void Changing_DimsProfileID_UpdatesCorrectly_Test()
        {
            long newValue = shipment.BestRate.DimsProfileID + 2;
            testObject.DimsProfileID = shipment.BestRate.DimsProfileID + newValue;

            Assert.Equal(testObject.DimsProfileID, shipment.BestRate.DimsProfileID);
        }

        [Fact]
        public void InsuranceChoice_PopulatesCorrectly_Test()
        {
            IInsuranceChoice expected = new InsuranceChoice(shipment, shipment.BestRate, shipment.BestRate, null);

            Assert.Equal(expected.Insured, testObject.InsuranceChoice.Insured);
            Assert.Equal(expected.InsurancePennyOne, testObject.InsuranceChoice.InsurancePennyOne);
            Assert.Equal(expected.InsuranceProvider, testObject.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testObject.InsuranceChoice.InsuranceValue);
        }

        [Fact]
        public void InsuranceChoice_UpdatesCorrectly_Test()
        {
            IInsuranceChoice expected = new InsuranceChoice(shipment, shipment.BestRate, shipment.BestRate, null);
            expected.Insured = !expected.Insured;
            expected.InsuranceValue++;

            testObject.InsuranceChoice = expected;

            Assert.Equal(expected.Insured, testObject.InsuranceChoice.Insured);
            Assert.Equal(expected.InsuranceProvider, testObject.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testObject.InsuranceChoice.InsuranceValue);
        }

        private void PopulateDefaultObjects()
        {
            shipment = new ShipmentEntity()
            {
                ContentWeight = 3,
                InsuranceProvider = (int) InsuranceProvider.ShipWorks,
                Insurance = false,
                BestRate = new BestRateShipmentEntity()
                {
                    DimsLength = 6,
                    DimsWidth = 4,
                    DimsHeight = 1,
                    DimsWeight = 3,
                    DimsAddWeight = false,
                    DimsProfileID = 1049,
                    Insurance = false,
                    InsuranceValue = 5.5M
                }
            };

            testObject = new BestRatePackageAdapter(shipment);
        }
    }
}
