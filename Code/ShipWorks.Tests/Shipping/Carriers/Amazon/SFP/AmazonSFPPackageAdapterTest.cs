using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon.SFP
{
    public class AmazonSFPPackageAdapterTest
    {
        private ShipmentEntity shipment;
        private IPackageAdapter testObject;

        public AmazonSFPPackageAdapterTest()
        {
            PopulateDefaultObjects();
        }

        /// <summary>
        /// To support ShipSense, we do not want to have PackagingType added to the hash of the shipment.Amazon adapter.
        /// </summary>
        [Fact]
        public void Hash_DoesNotContain_PackagingType_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                AmazonSFP = new AmazonSFPShipmentEntity()
            };

            AmazonSFPPackageAdapter testObject = new AmazonSFPPackageAdapter(shipment)
            {
                PackagingType = 999999,
                AdditionalWeight = 3.1,
                ApplyAdditionalWeight = true,
                Height = 2.2,
                Length = 3.3,
                Weight = 4.4,
                Width = 5.5
            };

            string firstTry = testObject.HashCode();

            testObject.PackagingType = 0;
            string secondTry = testObject.HashCode();

            // Make sure PackagingType WAS NOT PART OF THE HASH!
            Assert.Equal(firstTry, secondTry);
        }

        [Fact]
        public void Constructor_Throws_WhenAmazonShipmentEntityIsNull_Test()
        {
            shipment.AmazonSFP = null;

            Assert.Throws<ArgumentNullException>(nameof(shipment.AmazonSFP), () => new AmazonSFPPackageAdapter(shipment));
        }

        [Fact]
        public void Constructor_PopulatesValues_Correctly_Test()
        {
            Assert.Equal(1, testObject.Index);
            Assert.Equal(shipment.AmazonSFP.DimsLength, testObject.DimsLength);
            Assert.Equal(shipment.AmazonSFP.DimsWidth, testObject.DimsWidth);
            Assert.Equal(shipment.AmazonSFP.DimsHeight, testObject.DimsHeight);
            Assert.Equal(shipment.AmazonSFP.DimsWeight, testObject.AdditionalWeight);
            Assert.Equal(shipment.AmazonSFP.DimsAddWeight, testObject.ApplyAdditionalWeight);
            Assert.Equal(shipment.AmazonSFP.DimsProfileID, testObject.DimsProfileID);
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

            Assert.Equal(testObject.AdditionalWeight, shipment.AmazonSFP.DimsWeight);
        }

        [Fact]
        public void Changing_ApplyAdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.ApplyAdditionalWeight = !shipment.AmazonSFP.DimsAddWeight;

            Assert.Equal(testObject.ApplyAdditionalWeight, shipment.AmazonSFP.DimsAddWeight);
        }

        [Fact]
        public void Changing_Index_IsStillOne_Test()
        {
            testObject.Index = 3;

            Assert.Equal(1, testObject.Index);
        }

        [Fact]
        public void InsuranceChoice_PopulatesCorrectly_Test()
        {
            AmazonSFPInsuranceChoice expected = new AmazonSFPInsuranceChoice(shipment);

            Assert.Equal(expected.Insured, testObject.InsuranceChoice.Insured);
            Assert.Equal(expected.InsurancePennyOne, testObject.InsuranceChoice.InsurancePennyOne);
            Assert.Equal(expected.InsuranceProvider, testObject.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testObject.InsuranceChoice.InsuranceValue);
        }

        [Fact]
        public void InsuranceChoice_UpdatesCorrectly_Test()
        {
            AmazonSFPInsuranceChoice expected = new AmazonSFPInsuranceChoice(shipment);
            expected.Insured = !expected.Insured;
            expected.InsurancePennyOne = !expected.InsurancePennyOne;
            expected.InsuranceValue++;

            testObject.InsuranceChoice = expected;

            Assert.Equal(expected.Insured, testObject.InsuranceChoice.Insured);
            Assert.Equal(expected.InsurancePennyOne, testObject.InsuranceChoice.InsurancePennyOne);
            Assert.Equal(expected.InsuranceProvider, testObject.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testObject.InsuranceChoice.InsuranceValue);
        }

        [Fact]
        public void Changing_DimsLength_UpdatesCorrectly_Test()
        {
            double newValue = shipment.AmazonSFP.DimsLength + 2.1;
            testObject.DimsLength = shipment.AmazonSFP.DimsLength + newValue;

            Assert.Equal(testObject.DimsLength, shipment.AmazonSFP.DimsLength);
        }

        [Fact]
        public void Changing_DimsWidth_UpdatesCorrectly_Test()
        {
            double newValue = shipment.AmazonSFP.DimsWidth + 2.1;
            testObject.DimsWidth = shipment.AmazonSFP.DimsWidth + newValue;

            Assert.Equal(testObject.DimsWidth, shipment.AmazonSFP.DimsWidth);
        }

        [Fact]
        public void Changing_DimsHeight_UpdatesCorrectly_Test()
        {
            double newValue = shipment.AmazonSFP.DimsHeight + 2.1;
            testObject.DimsHeight = shipment.AmazonSFP.DimsHeight + newValue;

            Assert.Equal(testObject.DimsHeight, shipment.AmazonSFP.DimsHeight);
        }

        [Fact]
        public void Changing_DimsProfileID_UpdatesCorrectly_Test()
        {
            long newValue = shipment.AmazonSFP.DimsProfileID + 2;
            testObject.DimsProfileID = shipment.AmazonSFP.DimsProfileID + newValue;

            Assert.Equal(testObject.DimsProfileID, shipment.AmazonSFP.DimsProfileID);
        }

        private void PopulateDefaultObjects()
        {
            shipment = new ShipmentEntity()
            {
                ContentWeight = 3,
                Insurance = false,
                InsuranceProvider = (int) InsuranceProvider.Carrier,
                AmazonSFP = new AmazonSFPShipmentEntity()
                {
                    DimsLength = 6,
                    DimsWidth = 4,
                    DimsHeight = 1,
                    DimsWeight = 3,
                    DimsAddWeight = false,
                    DimsProfileID = 1049
                }
            };

            testObject = new AmazonSFPPackageAdapter(shipment);
        }
    }
}
