using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon
{
    public class AmazonPackageAdapterTest
    {
        private ShipmentEntity shipment;
        private IPackageAdapter testObject;

        public AmazonPackageAdapterTest()
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
                Amazon = new AmazonShipmentEntity()
            };

            AmazonPackageAdapter testObject = new AmazonPackageAdapter(shipment)
            {
                PackagingType = new PackageTypeBinding() { PackageTypeID = 999999 },
                AdditionalWeight = 3.1,
                ApplyAdditionalWeight = true,
                Height = 2.2,
                Length = 3.3,
                Weight = 4.4,
                Width = 5.5
            };

            string firstTry = testObject.HashCode();

            testObject.PackagingType = new PackageTypeBinding() { PackageTypeID = 0 };
            string secondTry = testObject.HashCode();

            // Make sure PackagingType WAS NOT PART OF THE HASH!
            Assert.Equal(firstTry, secondTry);
        }

        [Fact]
        public void Constructor_Throws_WhenAmazonShipmentEntityIsNull_Test()
        {
            shipment.Amazon = null;

            Assert.Throws<ArgumentNullException>(nameof(shipment.Amazon), () => new AmazonPackageAdapter(shipment));
        }

        [Fact]
        public void Constructor_PopulatesValues_Correctly_Test()
        {
            Assert.Null(testObject.PackagingType);
            Assert.Equal(1, testObject.Index);
            Assert.Equal(shipment.Amazon.DimsLength, testObject.DimsLength);
            Assert.Equal(shipment.Amazon.DimsWidth, testObject.DimsWidth);
            Assert.Equal(shipment.Amazon.DimsHeight, testObject.DimsHeight);
            Assert.Equal(shipment.Amazon.DimsWeight, testObject.AdditionalWeight);
            Assert.Equal(shipment.Amazon.DimsAddWeight, testObject.ApplyAdditionalWeight);
            Assert.Equal(shipment.Amazon.DimsProfileID, testObject.DimsProfileID);
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

            Assert.Equal(testObject.AdditionalWeight, shipment.Amazon.DimsWeight);
        }

        [Fact]
        public void Changing_ApplyAdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.ApplyAdditionalWeight = !shipment.Amazon.DimsAddWeight;

            Assert.Equal(testObject.ApplyAdditionalWeight, shipment.Amazon.DimsAddWeight);
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
            AmazonInsuranceChoice expected = new AmazonInsuranceChoice(shipment);

            Assert.Equal(expected.Insured, testObject.InsuranceChoice.Insured);
            Assert.Equal(expected.InsurancePennyOne, testObject.InsuranceChoice.InsurancePennyOne);
            Assert.Equal(expected.InsuranceProvider, testObject.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testObject.InsuranceChoice.InsuranceValue);
        }

        [Fact]
        public void InsuranceChoice_UpdatesCorrectly_Test()
        {
            AmazonInsuranceChoice expected = new AmazonInsuranceChoice(shipment);
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
        public void Changing_PackagingType_IsStillNull_Test()
        {
            testObject.PackagingType = new PackageTypeBinding();

            Assert.Null(testObject.PackagingType);
        }

        [Fact]
        public void Changing_DimsLength_UpdatesCorrectly_Test()
        {
            double newValue = shipment.Amazon.DimsLength + 2.1;
            testObject.DimsLength = shipment.Amazon.DimsLength + newValue;

            Assert.Equal(testObject.DimsLength, shipment.Amazon.DimsLength);
        }

        [Fact]
        public void Changing_DimsWidth_UpdatesCorrectly_Test()
        {
            double newValue = shipment.Amazon.DimsWidth + 2.1;
            testObject.DimsWidth = shipment.Amazon.DimsWidth + newValue;

            Assert.Equal(testObject.DimsWidth, shipment.Amazon.DimsWidth);
        }

        [Fact]
        public void Changing_DimsHeight_UpdatesCorrectly_Test()
        {
            double newValue = shipment.Amazon.DimsHeight + 2.1;
            testObject.DimsHeight = shipment.Amazon.DimsHeight + newValue;

            Assert.Equal(testObject.DimsHeight, shipment.Amazon.DimsHeight);
        }

        [Fact]
        public void Changing_DimsProfileID_UpdatesCorrectly_Test()
        {
            long newValue = shipment.Amazon.DimsProfileID + 2;
            testObject.DimsProfileID = shipment.Amazon.DimsProfileID + newValue;

            Assert.Equal(testObject.DimsProfileID, shipment.Amazon.DimsProfileID);
        }

        private void PopulateDefaultObjects()
        {
            shipment = new ShipmentEntity()
            {
                ContentWeight = 3,
                Insurance = false,
                InsuranceProvider = (int)InsuranceProvider.Carrier,
                Amazon = new AmazonShipmentEntity()
                {
                    DimsLength = 6,
                    DimsWidth = 4,
                    DimsHeight = 1,
                    DimsWeight = 3,
                    DimsAddWeight = false,
                    DimsProfileID = 1049
                }
            };

            testObject = new AmazonPackageAdapter(shipment);
        }
    }
}
