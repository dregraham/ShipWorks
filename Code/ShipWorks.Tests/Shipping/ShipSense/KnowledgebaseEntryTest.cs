using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Shipping.ShipSense;
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

        public KnowledgebaseEntryTest()
        {
            adapter1 = new Mock<IPackageAdapter>();
            adapter1.SetupAllProperties();

            adapter1.Object.AdditionalWeight = 1.2;
            adapter1.Object.Height = 9.31;
            adapter1.Object.Length = 11;
            adapter1.Object.Weight = 34.2;
            adapter1.Object.Width = 4;
            
            
            
            adapter2 = new Mock<IPackageAdapter>();
            adapter2.SetupAllProperties();

            adapter2.Object.AdditionalWeight = 5.4;
            adapter2.Object.Height = 4.2;
            adapter2.Object.Length = 1;
            adapter2.Object.Weight = 12.9;
            adapter2.Object.Width = 34;

            adapters = new List<IPackageAdapter>()
            {
                adapter1.Object,
                adapter2.Object
            };

            testObject = new KnowledgebaseEntry();
            testObject.Packages = new List<KnowledgebasePackage>()
            {
                new KnowledgebasePackage
                {
                    AdditionalWeight = 2.5,
                    Height = 2,
                    Length = 4,
                    Weight = 4.5,
                    Width = 6
                },
                new KnowledgebasePackage
                {
                    AdditionalWeight = 2.5,
                    Height = 4,
                    Length = 5,
                    Weight = 6,
                    Width = 7
                }
            };
        }

        [TestMethod]
        public void Constructor_HydratingFromJson_Test()
        {
            KnowledgebaseEntry hydratedEntry = new KnowledgebaseEntry(testObject.ToJson());

            Assert.AreEqual(testObject.Packages.Count(), hydratedEntry.Packages.Count());

            for (int i = 0; i < hydratedEntry.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).AdditionalWeight, hydratedEntry.Packages.ElementAt(i).AdditionalWeight);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Height, hydratedEntry.Packages.ElementAt(i).Height);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Length, hydratedEntry.Packages.ElementAt(i).Length);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Weight, hydratedEntry.Packages.ElementAt(i).Weight);
                Assert.AreEqual(testObject.Packages.ElementAt(i).Width, hydratedEntry.Packages.ElementAt(i).Width);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ApplyTo_ThrowsInvalidOperationException_WhenPackageCountDoesNotMatchAdapterCount_Test()
        {
            adapters = new List<IPackageAdapter>
            {
                adapter1.Object
            };

            testObject.ApplyTo(adapters);
        }

        [TestMethod]
        public void ApplyTo_AssignsAdditionalWeightOfEachAdapter_Test()
        {
            testObject.ApplyTo(adapters);
            
            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.AdditionalWeight = testObject.Packages.ElementAt(0).AdditionalWeight, Times.Once());
            adapter2.VerifySet(a => a.AdditionalWeight = testObject.Packages.ElementAt(1).AdditionalWeight, Times.Once());
        }

        [TestMethod]
        public void ApplyTo_AssignsHeightOfEachAdapter_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.Height = testObject.Packages.ElementAt(0).Height, Times.Once());
            adapter2.VerifySet(a => a.Height = testObject.Packages.ElementAt(1).Height, Times.Once());
        }

        [TestMethod]
        public void ApplyTo_AssignsLengthOfEachAdapter_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.Length = testObject.Packages.ElementAt(0).Length, Times.Once());
            adapter2.VerifySet(a => a.Length = testObject.Packages.ElementAt(1).Length, Times.Once());
        }

        [TestMethod]
        public void ApplyTo_AssignsWeightOfEachAdapter_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.Weight = testObject.Packages.ElementAt(0).Weight, Times.Once());
            adapter2.VerifySet(a => a.Weight = testObject.Packages.ElementAt(1).Weight, Times.Once());
        }
        
        [TestMethod]
        public void ApplyTo_AssignsWidthOfEachAdapter_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.Width = testObject.Packages.ElementAt(0).Width, Times.Once());
            adapter2.VerifySet(a => a.Width = testObject.Packages.ElementAt(1).Width, Times.Once());
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
        public void ToJson_Test()
        {
            const string ExpectedJson = "{\"Packages\":[{\"Length\":4.0,\"Width\":6.0,\"Height\":2.0,\"Weight\":4.5,\"AdditionalWeight\":2.5},{\"Length\":5.0,\"Width\":7.0,\"Height\":4.0,\"Weight\":6.0,\"AdditionalWeight\":2.5}]}";
            
            string actualJson = testObject.ToJson();
            Assert.IsTrue(Newtonsoft.Json.Linq.JToken.DeepEquals(ExpectedJson, actualJson));
        }


    }
}


