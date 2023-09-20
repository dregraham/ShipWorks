using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx
{
    public class FedExPackageAdapterTest
    {
        private ShipmentEntity shipment;
        private FedExPackageEntity package;
        private IPackageAdapter testPackageAdapter;

        public FedExPackageAdapterTest()
        {
            PopulateDefaultObjects();
        }

        /// <summary>
        /// To support ShipSense, we do not want to have PackagingType added to the hash of the package adapter.
        /// </summary>
        [Fact]
        public void Hash_DoesNotContain_PackagingType_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                FedEx = new FedExShipmentEntity()
            };

            FedExPackageEntity package = new FedExPackageEntity();

            FedExPackageAdapter testObject = new FedExPackageAdapter(shipment, package, 1)
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
        public void Constructor_Throws_WhenFedExShipmentEntityIsNull_Test()
        {
            shipment.FedEx = null;

            Assert.Throws<ArgumentNullException>(nameof(shipment.FedEx), () => new FedExPackageAdapter(shipment, package, 1));
        }

        [Fact]
        public void Constructor_PopulatesValues_Correctly_Test()
        {
            Assert.Equal(shipment.FedEx.PackagingType, testPackageAdapter.PackagingType);
            Assert.Equal(1, testPackageAdapter.Index);
            Assert.Equal(package.DimsLength, testPackageAdapter.DimsLength);
            Assert.Equal(package.DimsWidth, testPackageAdapter.DimsWidth);
            Assert.Equal(package.DimsHeight, testPackageAdapter.DimsHeight);
            Assert.Equal(package.DimsWeight, testPackageAdapter.AdditionalWeight);
            Assert.Equal(package.DimsAddWeight, testPackageAdapter.ApplyAdditionalWeight);
            Assert.Equal(package.DimsProfileID, testPackageAdapter.DimsProfileID);
        }

        [Fact]
        public void InsuranceChoice_PopulatesCorrectly_Test()
        {
            IInsuranceChoice expected = new InsuranceChoice(shipment, package, package, package);

            Assert.Equal(expected.Insured, testPackageAdapter.InsuranceChoice.Insured);
            Assert.Equal(expected.InsurancePennyOne, testPackageAdapter.InsuranceChoice.InsurancePennyOne);
            Assert.Equal(expected.InsuranceProvider, testPackageAdapter.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testPackageAdapter.InsuranceChoice.InsuranceValue);
        }

        [Fact]
        public void InsuranceChoice_UpdatesCorrectly_Test()
        {
            IInsuranceChoice expected = new InsuranceChoice(shipment, package, package, package);
            expected.Insured = !expected.Insured;
            expected.InsurancePennyOne = !expected.InsurancePennyOne;
            expected.InsuranceValue++;

            testPackageAdapter.InsuranceChoice = expected;

            Assert.Equal(expected.Insured, testPackageAdapter.InsuranceChoice.Insured);
            Assert.Equal(expected.InsurancePennyOne, testPackageAdapter.InsuranceChoice.InsurancePennyOne);
            Assert.Equal(expected.InsuranceProvider, testPackageAdapter.InsuranceChoice.InsuranceProvider);
            Assert.Equal(expected.InsuranceValue, testPackageAdapter.InsuranceChoice.InsuranceValue);
        }

        [Fact]
        public void Changing_Weight_ForSinglePackage_Updates_Correctly_Test()
        {
            testPackageAdapter.Weight = 88.98;

            Assert.Equal(testPackageAdapter.Weight, shipment.ContentWeight);
            Assert.Equal(testPackageAdapter.Weight, package.Weight);
        }

        [Fact]
        public void Changing_Weight_ForTwoPackages_Updates_Correctly_Test()
        {
            double originalShipmentWeight = shipment.ContentWeight;
            FedExPackageEntity secondPackage = new FedExPackageEntity()
            {
                Weight = 3,
                DimsLength = 6,
                DimsWidth = 4,
                DimsHeight = 1,
                DimsWeight = 3,
                DimsAddWeight = false,
                DimsProfileID = 1049
            };

            shipment.FedEx.Packages.Add(secondPackage);

            FedExPackageAdapter secondPackageAdapter = new FedExPackageAdapter(shipment, secondPackage, 2);
            const double secondPackageWeight = 5;
            secondPackageAdapter.Weight = secondPackageWeight;

            Assert.Equal(originalShipmentWeight + secondPackageWeight, shipment.ContentWeight);
            Assert.Equal(testPackageAdapter.Weight, package.Weight);
            Assert.Equal(secondPackageAdapter.Weight, secondPackage.Weight);
        }

        [Fact]
        public void Changing_AdditionalWeight_UpdatesCorrectly_Test()
        {
            testPackageAdapter.AdditionalWeight = 5.4;

            Assert.Equal(testPackageAdapter.AdditionalWeight, package.DimsWeight);
        }

        [Fact]
        public void Changing_ApplyAdditionalWeight_UpdatesCorrectly_Test()
        {
            testPackageAdapter.ApplyAdditionalWeight = !package.DimsAddWeight;

            Assert.Equal(testPackageAdapter.ApplyAdditionalWeight, package.DimsAddWeight);
        }

        [Fact]
        public void Changing_PackagingType_UpdatesCorrectly_WhenNotNull_Test()
        {
            shipment.FedEx.PackagingType = (int) FedExPackagingType.Box25Kg;
            testPackageAdapter.PackagingType = (int) FedExPackagingType.Custom;

            Assert.Equal((int) FedExPackagingType.Custom, shipment.FedEx.PackagingType);
        }

        [Fact]
        public void Changing_DimsLength_UpdatesCorrectly_Test()
        {
            double newValue = package.DimsLength + 2.1;
            testPackageAdapter.DimsLength = package.DimsLength + newValue;

            Assert.Equal(testPackageAdapter.DimsLength, package.DimsLength);
        }

        [Fact]
        public void Changing_DimsWidth_UpdatesCorrectly_Test()
        {
            double newValue = package.DimsWidth + 2.1;
            testPackageAdapter.DimsWidth = package.DimsWidth + newValue;

            Assert.Equal(testPackageAdapter.DimsWidth, package.DimsWidth);
        }

        [Fact]
        public void Changing_DimsHeight_UpdatesCorrectly_Test()
        {
            double newValue = package.DimsHeight + 2.1;
            testPackageAdapter.DimsHeight = package.DimsHeight + newValue;

            Assert.Equal(testPackageAdapter.DimsHeight, package.DimsHeight);
        }

        [Fact]
        public void Changing_DimsProfileID_UpdatesCorrectly_Test()
        {
            long newValue = package.DimsProfileID + 2;
            testPackageAdapter.DimsProfileID = package.DimsProfileID + newValue;

            Assert.Equal(testPackageAdapter.DimsProfileID, package.DimsProfileID);
        }

        private void PopulateDefaultObjects()
        {
            package = new FedExPackageEntity()
            {
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
                InsuranceProvider = (int) InsuranceProvider.Carrier,
                Insurance = package.Insurance,
                FedEx = new FedExShipmentEntity()
                {
                    PackagingType = (int) FedExPackagingType.Box25Kg,
                    Packages = { package },
                }
            };

            testPackageAdapter = new FedExPackageAdapter(shipment, package, 1);
        }
    }
}
