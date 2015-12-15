using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsPackageAdapterTest
    {
        private ShipmentEntity shipment;
        private UpsPackageEntity package;
        private IPackageAdapter testObject;

        public UpsPackageAdapterTest()
        {
            PopulateDefaultObjects();
        }

        /// <summary>
        /// To support ShipSense, we do not want to have PackagingType added to the hash of the package adapter.
        /// </summary>
        [Fact]
        public void Hash_DoesNotContain_PackagingType_Test()
        {
            testObject = new UpsPackageAdapter(shipment, package, 1)
            {
                PackagingType = new PackageTypeBinding() { PackageTypeID = 999999 },
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
        public void Constructor_Throws_WhenUpsShipmentEntityIsNull_Test()
        {
            shipment.Ups = null;

            Assert.Throws<ArgumentNullException>(nameof(shipment.Ups), () => new UpsPackageAdapter(shipment, package, 1));
        }

        [Fact]
        public void Constructor_PopulatesValues_Correctly_Test()
        {
            Assert.Equal(package.PackagingType, testObject.PackagingType.PackageTypeID);
            Assert.Equal(1, testObject.Index);
            Assert.Equal(package.DimsLength, testObject.DimsLength);
            Assert.Equal(package.DimsWidth, testObject.DimsWidth);
            Assert.Equal(package.DimsHeight, testObject.DimsHeight);
            Assert.Equal(package.DimsWeight, testObject.AdditionalWeight);
            Assert.Equal(package.DimsAddWeight, testObject.ApplyAdditionalWeight);
            Assert.Equal(package.DimsProfileID, testObject.DimsProfileID);
        }

        [Fact]
        public void Changing_Weight_ForSinglePackage_Updates_Correctly_Test()
        {
            testObject.Weight = 88.98;

            Assert.Equal(testObject.Weight, shipment.ContentWeight);
            Assert.Equal(testObject.Weight, package.Weight);
        }

        [Fact]
        public void Changing_Weight_ForTwoPackages_Updates_Correctly_Test()
        {
            double originalShipmentWeight = shipment.ContentWeight;
            UpsPackageEntity secondPackage = new UpsPackageEntity()
            {
                PackagingType = (int)UpsPackagingType.Box25Kg,
                Weight = 3,
                DimsLength = 6,
                DimsWidth = 4,
                DimsHeight = 1,
                DimsWeight = 3,
                DimsAddWeight = false,
                DimsProfileID = 1049
            };

            shipment.Ups.Packages.Add(secondPackage);

            UpsPackageAdapter secondPackageAdapter = new UpsPackageAdapter(shipment, secondPackage, 2);
            secondPackageAdapter.Weight = 5;

            Assert.Equal(originalShipmentWeight, shipment.ContentWeight);
            Assert.Equal(testObject.Weight, package.Weight);
            Assert.Equal(secondPackageAdapter.Weight, secondPackage.Weight);
        }

        [Fact]
        public void Changing_AdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.AdditionalWeight = 5.4;

            Assert.Equal(testObject.AdditionalWeight, package.DimsWeight);
        }

        [Fact]
        public void Changing_ApplyAdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.ApplyAdditionalWeight = !package.DimsAddWeight;

            Assert.Equal(testObject.ApplyAdditionalWeight, package.DimsAddWeight);
        }

        [Fact]
        public void Changing_PackagingType_UpdatesCorrectly_WhenNotNull_Test()
        {
            package.PackagingType = (int) UpsPackagingType.Box25Kg;
            testObject.PackagingType = new PackageTypeBinding() { PackageTypeID = (int)UpsPackagingType.Custom };

            Assert.Equal(testObject.PackagingType.PackageTypeID, package.PackagingType);
        }

        [Fact]
        public void Changing_PackagingType_UpdatesCorrectly_WhenNull_Test()
        {
            package.PackagingType = (int)UpsPackagingType.Box25Kg;
            testObject.PackagingType = null;

            Assert.Null(testObject.PackagingType);
            Assert.Equal((int)UpsPackagingType.Box25Kg, package.PackagingType);
        }

        [Fact]
        public void Changing_DimsLength_UpdatesCorrectly_Test()
        {
            double newValue = package.DimsLength + 2.1;
            testObject.DimsLength = package.DimsLength + newValue;

            Assert.Equal(testObject.DimsLength, package.DimsLength);
        }

        [Fact]
        public void Changing_DimsWidth_UpdatesCorrectly_Test()
        {
            double newValue = package.DimsWidth + 2.1;
            testObject.DimsWidth = package.DimsWidth + newValue;

            Assert.Equal(testObject.DimsWidth, package.DimsWidth);
        }

        [Fact]
        public void Changing_DimsHeight_UpdatesCorrectly_Test()
        {
            double newValue = package.DimsHeight + 2.1;
            testObject.DimsHeight = package.DimsHeight + newValue;

            Assert.Equal(testObject.DimsHeight, package.DimsHeight);
        }

        [Fact]
        public void Changing_DimsProfileID_UpdatesCorrectly_Test()
        {
            long newValue = package.DimsProfileID + 2;
            testObject.DimsProfileID = package.DimsProfileID + newValue;

            Assert.Equal(testObject.DimsProfileID, package.DimsProfileID);
        }

        [Fact]
        public void InsuranceChoice_PopulatesCorrectly_Test()
        {
            IInsuranceChoice expected = new InsuranceChoice(shipment, package, package, package);

            Assert.Equal(expected.Insured, testObject.InsuranceChoice.Insured);
            Assert.Equal(expected.InsurancePennyOne, testObject.InsuranceChoice.InsurancePennyOne);
            Assert.Equal(expected.InsuranceProvider, testObject.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testObject.InsuranceChoice.InsuranceValue);
        }

        [Fact]
        public void InsuranceChoice_UpdatesCorrectly_Test()
        {
            IInsuranceChoice expected = new InsuranceChoice(shipment, package, package, package);
            expected.Insured = !expected.Insured;
            expected.InsurancePennyOne = !expected.InsurancePennyOne;
            expected.InsuranceValue++;

            testObject.InsuranceChoice = expected;

            Assert.Equal(expected.Insured, testObject.InsuranceChoice.Insured);
            Assert.Equal(expected.InsurancePennyOne, testObject.InsuranceChoice.InsurancePennyOne);
            Assert.Equal(expected.InsuranceProvider, testObject.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testObject.InsuranceChoice.InsuranceValue);
        }

        private void PopulateDefaultObjects()
        {
            package = new UpsPackageEntity()
            {
                PackagingType = (int)UpsPackagingType.Box25Kg,
                Weight = 3,
                DimsLength = 6,
                DimsWidth = 4,
                DimsHeight = 1,
                DimsWeight = 3,
                DimsAddWeight = false,
                DimsProfileID = 1049,
                Insurance = false,
                InsurancePennyOne = false,
                InsuranceValue = 5.55M
            };

            shipment = new ShipmentEntity()
            {
                ContentWeight = 3,
                InsuranceProvider = (int)InsuranceProvider.Carrier,
                Insurance = package.Insurance,
                Ups = new UpsShipmentEntity()
                {
                    Packages = { package}
                }
            };

            testObject = new UpsPackageAdapter(shipment, package, 1);
        }
    }
}
