using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal
{
    public class PostalPackageAdapterTest
    {
        private ShipmentEntity shipment;
        private IPackageAdapter testObject;

        public PostalPackageAdapterTest()
        {
            PopulateDefaultObjects();
        }

        /// <summary>
        /// To support ShipSense, we do not want to have PackagingType added to the hash of the shipment.Postal adapter.
        /// </summary>
        [Fact]
        public void Hash_DoesNotContain_PackagingType_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
            };

            PostalPackageAdapter testObject = new PostalPackageAdapter(shipment)
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
        public void Changing_Index_IsStillOne_Test()
        {
            testObject.Index = 3;

            Assert.Equal(1, testObject.Index);
        }

        [Fact]
        public void Constructor_PopulatesValues_Correctly_Test()
        {
            Assert.Equal(shipment.Postal.PackagingType, testObject.PackagingType.PackageTypeID);
            Assert.Equal(1, testObject.Index);
            Assert.Equal(shipment.Postal.DimsLength, testObject.DimsLength);
            Assert.Equal(shipment.Postal.DimsWidth, testObject.DimsWidth);
            Assert.Equal(shipment.Postal.DimsHeight, testObject.DimsHeight);
            Assert.Equal(shipment.Postal.DimsWeight, testObject.AdditionalWeight);
            Assert.Equal(shipment.Postal.DimsAddWeight, testObject.ApplyAdditionalWeight);
            Assert.Equal(shipment.Postal.DimsProfileID, testObject.DimsProfileID);
        }

        [Fact]
        public void Changing_Weight_ForSinglePackage_Updates_Correctly_Test()
        {
            testObject.Weight = 88.98;

            Assert.Equal(testObject.Weight, shipment.ContentWeight);
            Assert.Equal(testObject.Weight, shipment.Postal.DimsWeight);
        }

        [Fact]
        public void Changing_AdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.AdditionalWeight = 5.4;

            Assert.Equal(testObject.AdditionalWeight, shipment.Postal.DimsWeight);
        }

        [Fact]
        public void Changing_ApplyAdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.ApplyAdditionalWeight = !shipment.Postal.DimsAddWeight;

            Assert.Equal(testObject.ApplyAdditionalWeight, shipment.Postal.DimsAddWeight);
        }

        [Fact]
        public void Changing_PackagingType_UpdatesCorrectly_WhenNotNull_Test()
        {
            shipment.Postal.PackagingType = (int)PostalPackagingType.Package;
            testObject.PackagingType = new PackageTypeBinding() { PackageTypeID = (int)PostalPackagingType.FlatRateLargeBox };

            Assert.Equal(testObject.PackagingType.PackageTypeID, shipment.Postal.PackagingType);
        }

        [Fact]
        public void Changing_PackagingType_UpdatesCorrectly_WhenNull_Test()
        {
            shipment.Postal.PackagingType = (int)PostalPackagingType.Package;
            testObject.PackagingType = null;

            Assert.Null(testObject.PackagingType);
            Assert.Equal((int)PostalPackagingType.Package, shipment.Postal.PackagingType);
        }

        [Fact]
        public void Changing_DimsLength_UpdatesCorrectly_Test()
        {
            double newValue = shipment.Postal.DimsLength + 2.1;
            testObject.DimsLength = shipment.Postal.DimsLength + newValue;

            Assert.Equal(testObject.DimsLength, shipment.Postal.DimsLength);
        }

        [Fact]
        public void Changing_DimsWidth_UpdatesCorrectly_Test()
        {
            double newValue = shipment.Postal.DimsWidth + 2.1;
            testObject.DimsWidth = shipment.Postal.DimsWidth + newValue;

            Assert.Equal(testObject.DimsWidth, shipment.Postal.DimsWidth);
        }

        [Fact]
        public void Changing_DimsHeight_UpdatesCorrectly_Test()
        {
            double newValue = shipment.Postal.DimsHeight + 2.1;
            testObject.DimsHeight = shipment.Postal.DimsHeight + newValue;

            Assert.Equal(testObject.DimsHeight, shipment.Postal.DimsHeight);
        }

        [Fact]
        public void Changing_DimsProfileID_UpdatesCorrectly_Test()
        {
            long newValue = shipment.Postal.DimsProfileID + 2;
            testObject.DimsProfileID = shipment.Postal.DimsProfileID + newValue;

            Assert.Equal(testObject.DimsProfileID, shipment.Postal.DimsProfileID);
        }

        private void PopulateDefaultObjects()
        {
            shipment = new ShipmentEntity()
            {
                ContentWeight = 3,
                Postal = new PostalShipmentEntity()
                {
                    DimsLength = 6,
                    DimsWidth = 4,
                    DimsHeight = 1,
                    DimsWeight = 3,
                    DimsAddWeight = false,
                    DimsProfileID = 1049
                }
            };

            testObject = new PostalPackageAdapter(shipment);
        }
    }
}
