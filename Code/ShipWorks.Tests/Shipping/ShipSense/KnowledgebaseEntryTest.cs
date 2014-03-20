using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Tests.Shipping.ShipSense
{
    [TestClass]
    public class KnowledgebaseEntryTest
    {
        private readonly KnowledgebaseEntry testObject;

        private List<IPackageAdapter> adapters;
        private Mock<IPackageAdapter> adapter1;
        private Mock<IPackageAdapter> adapter2;

        public KnowledgebaseEntryTest()
        {
            adapter1 = new Mock<IPackageAdapter>();
            adapter1.SetupAllProperties();

            adapter1.Object.AdditionalWeight = 1.2;
            adapter1.Object.Height = 9.31;
            adapter1.Object.Length = 11;
            adapter1.Object.Weight = 34.2;
            adapter1.Object.WeightUnitOfMeasure = WeightUnitOfMeasure.Kilograms;
            adapter1.Object.Width = 4;
            
            
            
            adapter2 = new Mock<IPackageAdapter>();
            adapter2.SetupAllProperties();

            adapter2.Object.AdditionalWeight = 5.4;
            adapter2.Object.Height = 4.2;
            adapter2.Object.Length = 1;
            adapter2.Object.Weight = 12.9;
            adapter2.Object.WeightUnitOfMeasure = WeightUnitOfMeasure.Pounds;
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
                    Width = 6,
                    WeightUnitOfMeasure = WeightUnitOfMeasure.Ounces
                },
                new KnowledgebasePackage
                {
                    AdditionalWeight = 2.5,
                    Height = 4,
                    Length = 5,
                    Weight = 6,
                    Width = 7,
                    WeightUnitOfMeasure = WeightUnitOfMeasure.Grams
                }
            };
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
        public void ApplyTo_AssignsWeightUnitOfMeasureOfEachAdapter_Test()
        {
            testObject.ApplyTo(adapters);

            // Don't iterate over each element because we want to verify that set 
            // was called via Moq
            adapter1.VerifySet(a => a.WeightUnitOfMeasure = testObject.Packages.ElementAt(0).WeightUnitOfMeasure, Times.Once());
            adapter2.VerifySet(a => a.WeightUnitOfMeasure = testObject.Packages.ElementAt(1).WeightUnitOfMeasure, Times.Once());
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void ApplyFrom_ThrowsInvalidOperationException_WhenPackageCountDoesNotMatchAdapterCount_Test()
        {
            adapters = new List<IPackageAdapter>
            {
                adapter1.Object
            };

            testObject.ApplyFrom(adapters);
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
        public void ApplyFrom_AssignsWeightUnitOfMeasureOfPackage_Test()
        {
            testObject.ApplyFrom(adapters);

            // Check that the Get property was called to confirm that the values are not equal 
            // because the adapter value was assigned to
            adapter1.VerifyGet(a => a.WeightUnitOfMeasure, Times.Once());
            adapter2.VerifyGet(a => a.WeightUnitOfMeasure, Times.Once());

            for (int i = 0; i < testObject.Packages.Count(); i++)
            {
                Assert.AreEqual(testObject.Packages.ElementAt(i).WeightUnitOfMeasure, adapters[i].WeightUnitOfMeasure);
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
    }
}


