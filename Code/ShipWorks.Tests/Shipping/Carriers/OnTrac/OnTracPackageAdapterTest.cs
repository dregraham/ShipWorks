using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac
{
    public class OnTracPackageAdapterTest
    {
        private ShipmentEntity shipment;
        private IPackageAdapter testObject;

        public OnTracPackageAdapterTest()
        {
            PopulateDefaultObjects();
        }

        /// <summary>
        /// To support ShipSense, we do not want to have PackagingType added to the hash of the shipment.OnTrac adapter.
        /// </summary>
        [Fact]
        public void Hash_DoesNotContain_PackagingType_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity()
            {
                OnTrac = new OnTracShipmentEntity()
            };

            OnTracPackageAdapter testObject = new OnTracPackageAdapter(shipment)
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
        public void Constructor_PopulatesValues_Correctly_Test()
        {
            Assert.Equal(shipment.OnTrac.PackagingType, testObject.PackagingType.PackageTypeID);
            Assert.Equal(1, testObject.Index);
            Assert.Equal(shipment.OnTrac.DimsLength, testObject.DimsLength);
            Assert.Equal(shipment.OnTrac.DimsWidth, testObject.DimsWidth);
            Assert.Equal(shipment.OnTrac.DimsHeight, testObject.DimsHeight);
            Assert.Equal(shipment.OnTrac.DimsWeight, testObject.AdditionalWeight);
            Assert.Equal(shipment.OnTrac.DimsAddWeight, testObject.ApplyAdditionalWeight);
            Assert.Equal(shipment.OnTrac.DimsProfileID, testObject.DimsProfileID);
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

            Assert.Equal(testObject.AdditionalWeight, shipment.OnTrac.DimsWeight);
        }

        [Fact]
        public void Changing_ApplyAdditionalWeight_UpdatesCorrectly_Test()
        {
            testObject.ApplyAdditionalWeight = !shipment.OnTrac.DimsAddWeight;

            Assert.Equal(testObject.ApplyAdditionalWeight, shipment.OnTrac.DimsAddWeight);
        }

        [Fact]
        public void Changing_PackagingType_UpdatesCorrectly_WhenNotNull_Test()
        {
            shipment.OnTrac.PackagingType = (int)OnTracPackagingType.Package;
            testObject.PackagingType = new PackageTypeBinding() { PackageTypeID = (int)OnTracPackagingType.Letter };

            Assert.Equal(testObject.PackagingType.PackageTypeID, shipment.OnTrac.PackagingType);
        }

        [Fact]
        public void Changing_PackagingType_UpdatesCorrectly_WhenNull_Test()
        {
            shipment.OnTrac.PackagingType = (int)OnTracPackagingType.Package;
            testObject.PackagingType = null;

            Assert.Null(testObject.PackagingType);
            Assert.Equal((int)OnTracPackagingType.Package, shipment.OnTrac.PackagingType);
        }

        [Fact]
        public void Changing_DimsLength_UpdatesCorrectly_Test()
        {
            double newValue = shipment.OnTrac.DimsLength + 2.1;
            testObject.DimsLength = shipment.OnTrac.DimsLength + newValue;

            Assert.Equal(testObject.DimsLength, shipment.OnTrac.DimsLength);
        }

        [Fact]
        public void Changing_DimsWidth_UpdatesCorrectly_Test()
        {
            double newValue = shipment.OnTrac.DimsWidth + 2.1;
            testObject.DimsWidth = shipment.OnTrac.DimsWidth + newValue;

            Assert.Equal(testObject.DimsWidth, shipment.OnTrac.DimsWidth);
        }

        [Fact]
        public void Changing_DimsHeight_UpdatesCorrectly_Test()
        {
            double newValue = shipment.OnTrac.DimsHeight + 2.1;
            testObject.DimsHeight = shipment.OnTrac.DimsHeight + newValue;

            Assert.Equal(testObject.DimsHeight, shipment.OnTrac.DimsHeight);
        }

        [Fact]
        public void Changing_DimsProfileID_UpdatesCorrectly_Test()
        {
            long newValue = shipment.OnTrac.DimsProfileID + 2;
            testObject.DimsProfileID = shipment.OnTrac.DimsProfileID + newValue;

            Assert.Equal(testObject.DimsProfileID, shipment.OnTrac.DimsProfileID);
        }

        private void PopulateDefaultObjects()
        {
            shipment = new ShipmentEntity()
            {
                ContentWeight = 3,
                OnTrac = new OnTracShipmentEntity()
                {
                    PackagingType = (int)OnTracPackagingType.Package,
                    DimsLength = 6,
                    DimsWidth = 4,
                    DimsHeight = 1,
                    DimsWeight = 3,
                    DimsAddWeight = false,
                    DimsProfileID = 1049
                }
            };

            testObject = new OnTracPackageAdapter(shipment);
        }
    }
}
