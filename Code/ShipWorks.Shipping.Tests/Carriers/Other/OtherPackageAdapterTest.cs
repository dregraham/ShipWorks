using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Other
{
    public class OtherPackageAdapterTest
    {
        private ShipmentEntity shipment;
        private IPackageAdapter testObject;

        public OtherPackageAdapterTest()
        {
            PopulateDefaultObjects();
        }

        /// <summary>
        /// To support ShipSense, we do not want to have PackagingType added to the hash of the package adapter.
        /// </summary>
        [Fact]
        public void Hash_DoesNotContain_PackagingType_Test()
        {
            shipment = new ShipmentEntity()
            {
                Other = new OtherShipmentEntity()
            };

            testObject = new OtherPackageAdapter(shipment)
            {
                PackagingType = new PackageTypeBinding() {PackageTypeID = 999999},
                AdditionalWeight = 3.1,
                ApplyAdditionalWeight = true,
                DimsHeight = 2.2,
                DimsLength = 3.3,
                Weight = 4.4,
                DimsWidth = 5.5
            };

            string firstTry = testObject.HashCode();

            testObject.PackagingType = new PackageTypeBinding() { PackageTypeID = 0 };
            string secondTry = testObject.HashCode();

            // Make sure PackagingType WAS NOT PART OF THE HASH!
            Assert.Equal(firstTry, secondTry);
        }

        [Fact]
        public void Constructor_Throws_WhenOtherShipmentEntityIsNull_Test()
        {
            shipment.Other = null;

            Assert.Throws<ArgumentNullException>(nameof(shipment.Other), () => new OtherPackageAdapter(shipment));
        }

        [Fact]
        public void Constructor_PopulatesValues_Correctly_Test()
        {
            Assert.Null(testObject.PackagingType);
            Assert.Equal(1, testObject.Index);
            Assert.Equal(0, testObject.DimsLength);
            Assert.Equal(0, testObject.DimsWidth);
            Assert.Equal(0, testObject.DimsHeight);
            Assert.Equal(0, testObject.AdditionalWeight);
            Assert.Equal(true, testObject.ApplyAdditionalWeight);
            Assert.Equal(0, testObject.DimsProfileID);
        }

        [Fact]
        public void Changing_Index_IsStillOne_Test()
        {
            testObject.Index = 3;

            Assert.Equal(1, testObject.Index);
        }

        [Fact]
        public void Changing_PackagingType_IsStillNull_Test()
        {
            testObject.PackagingType = new PackageTypeBinding();

            Assert.Null(testObject.PackagingType);
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

            Assert.Equal(0, testObject.AdditionalWeight);
        }

        [Fact]
        public void Changing_ApplyAdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.ApplyAdditionalWeight = false;

            Assert.True(testObject.ApplyAdditionalWeight);
        }

        [Fact]
        public void Changing_DimsLength_UpdatesCorrectly_Test()
        {
            double newValue = 2.1;
            testObject.DimsLength = newValue;

            Assert.Equal(0, testObject.DimsLength);
        }

        [Fact]
        public void Changing_DimsWidth_UpdatesCorrectly_Test()
        {
            double newValue = 2.1;
            testObject.DimsWidth = newValue;

            Assert.Equal(0, testObject.DimsWidth);
        }

        [Fact]
        public void Changing_DimsHeight_UpdatesCorrectly_Test()
        {
            double newValue = 2.1;
            testObject.DimsHeight = newValue;

            Assert.Equal(0, testObject.DimsHeight);
        }

        [Fact]
        public void Changing_DimsProfileID_UpdatesCorrectly_Test()
        {
            long newValue = 2;
            testObject.DimsProfileID = newValue;

            Assert.Equal(0, testObject.DimsProfileID);
        }
        [Fact]
        public void InsuranceChoice_PopulatesCorrectly_Test()
        {
            IInsuranceChoice expected = new InsuranceChoice(shipment, shipment, shipment.Other, null);

            Assert.Equal(expected.Insured, testObject.InsuranceChoice.Insured);
            Assert.Equal(expected.InsuranceProvider, testObject.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testObject.InsuranceChoice.InsuranceValue);
        }

        [Fact]
        public void InsuranceChoice_UpdatesCorrectly_Test()
        {
            IInsuranceChoice expected = new InsuranceChoice(shipment, shipment, shipment.Other, null);
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
                Other = new OtherShipmentEntity()
                {
                    InsuranceValue = 5.5M
                }
            };

            testObject = new OtherPackageAdapter(shipment);
        }
    }
}
