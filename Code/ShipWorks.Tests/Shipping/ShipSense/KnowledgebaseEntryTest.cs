﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Customs;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Tests.Shipping.ShipSense
{
    [TestClass]
    public class KnowledgebaseEntryTest
    {
        private readonly KnowledgebaseEntry testObject;

        private List<IPackageAdapter> adapters;
        private readonly Mock<IPackageAdapter> adapter1;
        private readonly Mock<IPackageAdapter> adapter2;

        private const long testStoreID = 2005;

        public KnowledgebaseEntryTest()
        {
            adapter1 = new Mock<IPackageAdapter>();
            adapter1.SetupAllProperties();

            adapter1.Object.AdditionalWeight = 1.2;
            adapter1.Object.Height = 9.31;
            adapter1.Object.Length = 11;
            adapter1.Object.Weight = 34.2;
            adapter1.Object.Width = 4;
            adapter1.Object.ApplyAdditionalWeight = false;
            
            
            adapter2 = new Mock<IPackageAdapter>();
            adapter2.SetupAllProperties();

            adapter2.Object.AdditionalWeight = 5.4;
            adapter2.Object.Height = 4.2;
            adapter2.Object.Length = 1;
            adapter2.Object.Weight = 12.9;
            adapter2.Object.Width = 34;
            adapter2.Object.ApplyAdditionalWeight = false;

            adapters = new List<IPackageAdapter>()
            {
                adapter1.Object,
                adapter2.Object
            };

            testObject = new KnowledgebaseEntry(testStoreID);
            testObject.Packages = new List<KnowledgebasePackage>()
            {
                new KnowledgebasePackage
                {
                    AdditionalWeight = 2.5,
                    Height = 2,
                    Length = 4,
                    Weight = 4.5,
                    Width = 6,
                    ApplyAdditionalWeight = true,
                    Hash = "kbpackage1"
                },
                new KnowledgebasePackage
                {
                    AdditionalWeight = 2.5,
                    Height = 4,
                    Length = 5,
                    Weight = 6,
                    Width = 7,
                    ApplyAdditionalWeight = true,
                    Hash = "kbpackage2"
                }
            };
        }

        [TestMethod]
        public void Constructor_HydratingFromJson_Test()
        {
            testObject.ConsolidateMultiplePackagesIntoSinglePackage = true;

            KnowledgebaseEntry hydratedEntry = new KnowledgebaseEntry(testObject.ToJson());

            Assert.AreEqual(testObject.Packages.Count(), hydratedEntry.Packages.Count());
            for (int i = 0; i < hydratedEntry.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).AdditionalWeight, hydratedEntry.Packages.ElementAt(i).AdditionalWeight);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Height, hydratedEntry.Packages.ElementAt(i).Height);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Length, hydratedEntry.Packages.ElementAt(i).Length);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Weight, hydratedEntry.Packages.ElementAt(i).Weight);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Width, hydratedEntry.Packages.ElementAt(i).Width);
                Assert.AreEqual(testObject.Packages.ElementAt(i).ApplyAdditionalWeight, hydratedEntry.Packages.ElementAt(i).ApplyAdditionalWeight);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Hash, hydratedEntry.Packages.ElementAt(i).Hash);
            }

            Assert.IsFalse(hydratedEntry.ConsolidateMultiplePackagesIntoSinglePackage);
            Assert.AreEqual(0, hydratedEntry.CustomsItems.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ApplyTo_ThrowsInvalidOperationException_WhenPackageCountDoesNotMatchAdapterCount_AndConsolidateIsFalse_Test()
        {
            adapters = new List<IPackageAdapter>
            {
                adapter1.Object
            };

            testObject.ApplyTo(adapters);
        }

        [TestMethod]
        public void ApplyTo_AssignsAdditionalWeightOfEachAdapter_WhenConsolidateIsFalse_Test()
        {
            testObject.ApplyTo(adapters);
            
            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.AdditionalWeight = testObject.Packages.ElementAt(0).AdditionalWeight, Times.Once());
            adapter2.VerifySet(a => a.AdditionalWeight = testObject.Packages.ElementAt(1).AdditionalWeight, Times.Once());
        }

        [TestMethod]
        public void ApplyTo_AssignsHeightOfEachAdapter_WhenConsolidateIsFalse_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.Height = testObject.Packages.ElementAt(0).Height, Times.Once());
            adapter2.VerifySet(a => a.Height = testObject.Packages.ElementAt(1).Height, Times.Once());
        }

        [TestMethod]
        public void ApplyTo_AssignsLengthOfEachAdapter_WhenConsolidateIsFalse_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.Length = testObject.Packages.ElementAt(0).Length, Times.Once());
            adapter2.VerifySet(a => a.Length = testObject.Packages.ElementAt(1).Length, Times.Once());
        }

        [TestMethod]
        public void ApplyTo_AssignsWeightOfEachAdapter_WhenConsolidateIsFalse_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.Weight = testObject.Packages.ElementAt(0).Weight, Times.Once());
            adapter2.VerifySet(a => a.Weight = testObject.Packages.ElementAt(1).Weight, Times.Once());
        }

        [TestMethod]
        public void ApplyTo_AssignsApplyAdditionalWeightOfEachAdapter_WhenConsolidateIsFalse_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.ApplyAdditionalWeight = testObject.Packages.ElementAt(0).ApplyAdditionalWeight, Times.Once());
            adapter2.VerifySet(a => a.ApplyAdditionalWeight = testObject.Packages.ElementAt(1).ApplyAdditionalWeight, Times.Once());
        }
        
        [TestMethod]
        public void ApplyTo_AssignsWidthOfEachAdapter_WhenConsolidateIsFalse_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.Width = testObject.Packages.ElementAt(0).Width, Times.Once());
            adapter2.VerifySet(a => a.Width = testObject.Packages.ElementAt(1).Width, Times.Once());
        }
        
        [TestMethod]
        public void ApplyTo_AssignsSummedAdditionalWeightToPackageAdapter_WhenConsolidateIsTrue_AndPackageAdapterCountIsOne_Test()
        {
            testObject.ConsolidateMultiplePackagesIntoSinglePackage = true;

            testObject.ApplyTo(adapters.Take(1));

            Assert.AreEqual(testObject.Packages.Sum(p => p.AdditionalWeight), adapter1.Object.AdditionalWeight);
        }

        [TestMethod]
        public void ApplyTo_AssignsHeightOfEachAdapter_WhenConsolidateIsTrue_Test()
        {
            testObject.ConsolidateMultiplePackagesIntoSinglePackage = true;

            testObject.ApplyTo(adapters.Take(1));

            Assert.AreEqual(testObject.Packages.ElementAt(0).Height, adapter1.Object.Height);
        }

        [TestMethod]
        public void ApplyTo_AssignsLengthOfEachAdapter_WhenConsolidateIsTrue_Test()
        {
            testObject.ConsolidateMultiplePackagesIntoSinglePackage = true;

            testObject.ApplyTo(adapters.Take(1));

            Assert.AreEqual(testObject.Packages.ElementAt(0).Length, adapter1.Object.Length);
        }

        [TestMethod]
        public void ApplyTo_AssignsSummedWeightToPackageAdapter_WhenConsolidateIsTrue_AndPackageAdapterCountIsOne_Test()
        {
            testObject.ConsolidateMultiplePackagesIntoSinglePackage = true;

            testObject.ApplyTo(adapters.Take(1));

            Assert.AreEqual(testObject.Packages.Sum(p => p.Weight), adapter1.Object.Weight);
        }

        [TestMethod]
        public void ApplyTo_AssignsWidthOfEachAdapter_WhenConsolidateIsTrue_Test()
        {
            testObject.ConsolidateMultiplePackagesIntoSinglePackage = true;

            testObject.ApplyTo(adapters.Take(1));

            Assert.AreEqual(testObject.Packages.ElementAt(0).Width, adapter1.Object.Width);
        }
        
        [TestMethod]
        public void ApplyFrom_RegeneratesPackages_Test()
        {
            adapters = new List<IPackageAdapter>
            {
                adapter1.Object
            };

            testObject.ApplyFrom(adapters);

            // The number of packages on our test object should now be one instead of two
            Assert.AreEqual(1, testObject.Packages.Count());
        }

        [TestMethod]
        public void ApplyFrom_AssignsAdditionalWeightOfPackage_Test()
        {
            testObject.ApplyFrom(adapters);

            // Check that the Get property was called to confirm that the values are not equal 
            // because the adapter value was assigned to
            adapter1.VerifyGet(a => a.AdditionalWeight, Times.Once());
            adapter2.VerifyGet(a => a.AdditionalWeight, Times.Once());

            for (int i = 0; i < testObject.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).AdditionalWeight, adapters[i].AdditionalWeight);
            }

        }

        [TestMethod]
        public void ApplyFrom_AssignsApplyAdditionalWeightOfPackage_Test()
        {
            testObject.ApplyFrom(adapters);

            // Check that the Get property was called to confirm that the values are not equal 
            // because the adapter value was assigned to
            adapter1.VerifyGet(a => a.ApplyAdditionalWeight, Times.Once());
            adapter2.VerifyGet(a => a.ApplyAdditionalWeight, Times.Once());

            for (int i = 0; i < testObject.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).ApplyAdditionalWeight, adapters[i].ApplyAdditionalWeight);
            }

        }

        [TestMethod]
        public void ApplyFrom_AssignsHeightOfPackage_Test()
        {
            testObject.ApplyFrom(adapters);
            
            // Check that the Get property was called to confirm that the values are not equal 
            // because the adapter value was assigned to
            adapter1.VerifyGet(a => a.Height, Times.Once());
            adapter2.VerifyGet(a => a.Height, Times.Once());

            for (int i = 0; i < testObject.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).Height, adapters[i].Height);
            }
        }

        [TestMethod]
        public void ApplyFrom_AssignsLengthOfPackage_Test()
        {
            testObject.ApplyFrom(adapters);

            // Check that the Get property was called to confirm that the values are not equal 
            // because the adapter value was assigned to
            adapter1.VerifyGet(a => a.Length, Times.Once());
            adapter2.VerifyGet(a => a.Length, Times.Once());

            for (int i = 0; i < testObject.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).Length, adapters[i].Length);
            }
        }

        [TestMethod]
        public void ApplyFrom_AssignsWeightOfPackage_Test()
        {
            testObject.ApplyFrom(adapters);

            // Check that the Get property was called to confirm that the values are not equal 
            // because the adapter value was assigned to
            adapter1.VerifyGet(a => a.Weight, Times.Once());
            adapter2.VerifyGet(a => a.Weight, Times.Once());

            for (int i = 0; i < testObject.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).Weight, adapters[i].Weight);
            }
        }

        [TestMethod]
        public void ApplyFrom_AssignsWidthOfPackage_Test()
        {
            testObject.ApplyFrom(adapters);

            // Check that the Get property was called to confirm that the values are not equal 
            // because the adapter value was assigned to
            adapter1.VerifyGet(a => a.Width, Times.Once());
            adapter2.VerifyGet(a => a.Width, Times.Once());

            for (int i = 0; i < testObject.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).Width, adapters[i].Width);
            }
        }

        [TestMethod]
        public void ToJson_WithoutCustomsItems_Test()
        {
            const string ExpectedJson = "{\"StoreID\":2005,\"Packages\":[{\"Hash\":\"kbpackage1\",\"Length\":4.0,\"Width\":6.0,\"Height\":2.0,\"Weight\":4.5,\"ApplyAdditionalWeight\":true,\"AdditionalWeight\":2.5},{\"Hash\":\"kbpackage2\",\"Length\":5.0,\"Width\":7.0,\"Height\":4.0,\"Weight\":6.0,\"ApplyAdditionalWeight\":true,\"AdditionalWeight\":2.5}],\"CustomsItems\":[]}";
            
            string actualJson = testObject.ToJson();
            Assert.IsTrue(Newtonsoft.Json.Linq.JToken.DeepEquals(ExpectedJson, actualJson));
        }

        [TestMethod]
        public void ToJson_WithCustomsItems_Test()
        {
            const string ExpectedJson = "{\"StoreID\":2005,\"Packages\":[{\"Hash\":\"kbpackage1\",\"Length\":4.0,\"Width\":6.0,\"Height\":2.0,\"Weight\":4.5,\"ApplyAdditionalWeight\":true,\"AdditionalWeight\":2.5},{\"Hash\":\"kbpackage2\",\"Length\":5.0,\"Width\":7.0,\"Height\":4.0,\"Weight\":6.0,\"ApplyAdditionalWeight\":true,\"AdditionalWeight\":2.5}],\"CustomsItems\":[{\"Hash\":\"KZHcRlI3eB+7sWn+kA+oHhelYnS0hA9G6mgr8u9c5Rk=\",\"Description\":\"Test description\",\"Quantity\":2.3,\"Weight\":1.1,\"UnitValue\":4.32,\"CountryOfOrigin\":\"US\",\"HarmonizedCode\":\"ABC123\",\"NumberOfPieces\":4,\"UnitPriceAmount\":2.35},{\"Hash\":\"2apI6Az7OYvxln1DLCS59AVHDRdewe5gcWoSqYj4TVQ=\",\"Description\":\"Another test description\",\"Quantity\":2.0,\"Weight\":0.1,\"UnitValue\":6.22,\"CountryOfOrigin\":\"CA\",\"HarmonizedCode\":\"XYZ789\",\"NumberOfPieces\":1,\"UnitPriceAmount\":9.31}]}";
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>
            {
                new KnowledgebaseCustomsItem
                {
                    Description = "Test description",
                    Quantity = 2.3,
                    Weight = 1.1,
                    UnitValue = 4.32M,
                    CountryOfOrigin = "US",
                    HarmonizedCode = "ABC123",
                    NumberOfPieces = 4,
                    UnitPriceAmount = 2.35M
                },
                new KnowledgebaseCustomsItem
                {
                    Description = "Another test description",
                    Quantity = 2,
                    Weight = 0.1,
                    UnitValue = 6.22M,
                    CountryOfOrigin = "CA",
                    HarmonizedCode = "XYZ789",
                    NumberOfPieces = 1,
                    UnitPriceAmount = 9.31M
                }
            };

            string actualJson = testObject.ToJson();
            Assert.IsTrue(Newtonsoft.Json.Linq.JToken.DeepEquals(ExpectedJson, actualJson));
        }

        [TestMethod]
        public void ApplyFrom_AddsKbCustomsInfo_Test()
        {
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            List<ShipmentCustomsItemEntity> shipmentCustomsItems = new List<ShipmentCustomsItemEntity>()
            {
                new ShipmentCustomsItemEntity()
                {
                    Description = "Another test description",
                    Quantity = 2,
                    Weight = 0.1,
                    UnitValue = 6.22M,
                    CountryOfOrigin = "CA",
                    HarmonizedCode = "XYZ789",
                    NumberOfPieces = 1,
                    UnitPriceAmount = 9.31M
                },
                new ShipmentCustomsItemEntity
                {
                    Description = "Another test description",
                    Quantity = 2,
                    Weight = 0.1,
                    UnitValue = 6.22M,
                    CountryOfOrigin = "CA",
                    HarmonizedCode = "XYZ789",
                    NumberOfPieces = 1,
                    UnitPriceAmount = 9.31M
                }
            };

            testObject.ApplyFrom(adapters, shipmentCustomsItems);

            // Quick check some of the adapter stuff
            // Check that the Get property was called to confirm that the values are not equal 
            // because the adapter value was assigned to
            adapter1.VerifyGet(a => a.Width, Times.Once());
            adapter2.VerifyGet(a => a.Width, Times.Once());

            for (int i = 0; i < testObject.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).Width, adapters[i].Width);
            }

            // Now check the customs stuff
            Assert.AreEqual(2, testObject.CustomsItems.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ToJson_ThrowsInvalidOperation_WhenStoreIDIsNotSet_Test()
        {
            testObject.StoreID = -1;
            testObject.ToJson();
        }

        [TestMethod]
        public void Matches_ReturnsTrue_Test()
        {
            ShipmentEntity shipment = CreateMatchingShipment();

            // Make the values on the entry match the adapters and the customs items of the shipment
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            Assert.IsTrue(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenStoreIDDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();
            shipment.Order.StoreID += 1000;

            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenPackageCountsDoNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();


            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            // Add another package to the shipment, but not the test object
            shipment.FedEx.Packages.Add(new FedExPackageEntity());

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenPackageWeightDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();


            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            // Change one of the packages, so the hash no longer are in sync with the test object
            shipment.FedEx.Packages[0].Weight = 1000;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenPackageHeightDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();


            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            // Change one of the packages, so the hash no longer are in sync with the test object
            shipment.FedEx.Packages[0].DimsHeight = 1000;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenPackageLengthDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();


            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            // Change one of the packages, so the hash no longer are in sync with the test object
            shipment.FedEx.Packages[0].DimsLength = 1000;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenPackageWidthDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();


            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            // Change one of the packages, so the hash no longer are in sync with the test object
            shipment.FedEx.Packages[0].DimsWidth = 1000;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenPackageAdditionalWeightDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();


            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            // Change one of the packages, so the hash no longer are in sync with the test object
            shipment.FedEx.Packages[0].DimsWeight = 1000;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenPackageApplyAdditionalWeightDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();


            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            // Change one of the packages, so the hash no longer are in sync with the test object
            shipment.FedEx.Packages[0].DimsAddWeight = true;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenCustomItemsCountsDoNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();


            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem>();

            // Add another package to the shipment, but not the test object
            shipment.CustomsItems.Add(new ShipmentCustomsItemEntity());

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenCustomsItemDescriptionDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();
            AddCustomsItem(shipment);

            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem> { new KnowledgebaseCustomsItem(EntityUtility.CloneEntity(shipment.CustomsItems[0])) };

            // Change the weight of the customs item on the shipment, so the hash is no longer 
            // in sync with the test object
            shipment.CustomsItems[0].Description = "Something different";

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenCustomsItemQuantityDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();
            AddCustomsItem(shipment);

            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem> { new KnowledgebaseCustomsItem(EntityUtility.CloneEntity(shipment.CustomsItems[0])) };

            // Change the weight of the customs item on the shipment, so the hash is no longer 
            // in sync with the test object
            shipment.CustomsItems[0].Quantity = 400;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenCustomsItemUnitValueDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();
            AddCustomsItem(shipment);

            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem> { new KnowledgebaseCustomsItem(EntityUtility.CloneEntity(shipment.CustomsItems[0])) };

            // Change the weight of the customs item on the shipment, so the hash is no longer 
            // in sync with the test object
            shipment.CustomsItems[0].UnitValue = 400.74M;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenCustomsItemCountryOfOriginDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();
            AddCustomsItem(shipment);

            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem> { new KnowledgebaseCustomsItem(EntityUtility.CloneEntity(shipment.CustomsItems[0])) };

            // Change the weight of the customs item on the shipment, so the hash is no longer 
            // in sync with the test object
            shipment.CustomsItems[0].CountryOfOrigin = "CA";

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenCustomsItemHarmonizedCodeDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();
            AddCustomsItem(shipment);

            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem> { new KnowledgebaseCustomsItem(EntityUtility.CloneEntity(shipment.CustomsItems[0])) };

            // Change the weight of the customs item on the shipment, so the hash is no longer 
            // in sync with the test object
            shipment.CustomsItems[0].HarmonizedCode = "ABC";

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenCustomsItemNumberOfPiecesDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();
            AddCustomsItem(shipment);

            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem> { new KnowledgebaseCustomsItem(EntityUtility.CloneEntity(shipment.CustomsItems[0])) };

            // Change the weight of the customs item on the shipment, so the hash is no longer 
            // in sync with the test object
            shipment.CustomsItems[0].NumberOfPieces = 5;

            Assert.IsFalse(testObject.Matches(shipment));
        }

        [TestMethod]
        public void Matches_ReturnsFalse_WhenCustomsItemUnitPriceAmountDoesNotMatch_Test()
        {
            // Change the store ID of the matching shipment
            ShipmentEntity shipment = CreateMatchingShipment();
            AddCustomsItem(shipment);

            // Make the values on the entry match the adapters and the customs items of the shipment
            // Use the adapters from the FedEx shipment type, so the HashCode gets applied
            testObject.ApplyFrom(new FedExShipmentType().GetPackageAdapters(shipment));
            testObject.CustomsItems = new List<KnowledgebaseCustomsItem> { new KnowledgebaseCustomsItem(EntityUtility.CloneEntity(shipment.CustomsItems[0])) };

            // Change the weight of the customs item on the shipment, so the hash is no longer 
            // in sync with the test object
            shipment.CustomsItems[0].UnitPriceAmount = 400.50M;

            Assert.IsFalse(testObject.Matches(shipment));
        }


        private ShipmentEntity CreateMatchingShipment()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = new OrderEntity
                {
                    StoreID = testStoreID
                },
                ShipmentType = (int)ShipmentTypeCode.FedEx,
                FedEx = new FedExShipmentEntity()
            };

            shipment.FedEx.Packages.Add(new FedExPackageEntity());
            shipment.FedEx.Packages[0].Weight = adapter1.Object.Weight;
            shipment.FedEx.Packages[0].DimsHeight = adapter1.Object.Height;
            shipment.FedEx.Packages[0].DimsWidth = adapter1.Object.Width;
            shipment.FedEx.Packages[0].DimsLength = adapter1.Object.Length;
            shipment.FedEx.Packages[0].DimsWeight = adapter1.Object.AdditionalWeight;
            shipment.FedEx.Packages[0].DimsAddWeight = adapter1.Object.ApplyAdditionalWeight;

            shipment.FedEx.Packages.Add(new FedExPackageEntity());
            shipment.FedEx.Packages[1].Weight = adapter2.Object.Weight;
            shipment.FedEx.Packages[1].DimsHeight = adapter2.Object.Height;
            shipment.FedEx.Packages[1].DimsWidth = adapter2.Object.Width;
            shipment.FedEx.Packages[1].DimsLength = adapter2.Object.Length;
            shipment.FedEx.Packages[1].DimsWeight = adapter2.Object.AdditionalWeight;
            shipment.FedEx.Packages[1].DimsAddWeight = adapter2.Object.ApplyAdditionalWeight;

            return shipment;
        }

        private void AddCustomsItem(ShipmentEntity shipment)
        {
            ShipmentCustomsItemEntity customsEntity = new ShipmentCustomsItemEntity
            {
                Description = "test",
                Quantity = 1.0,
                Weight = 2.4,
                UnitValue = 1.54M,
                CountryOfOrigin = "US",
                HarmonizedCode = "123",
                NumberOfPieces = 1,
                UnitPriceAmount = 3.21M
            };

            shipment.CustomsItems.Add(customsEntity);
        }
    }
}


