using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xunit;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Tests.Shipping.ShipSense.Packaging
{
    public class KnowledgebasePackageChangeSetXmlWriterTest
    {
        private KnowledgebasePackageChangeSetXmlWriter testObject;

        private List<KnowledgebasePackage> beforePackages;
        private List<KnowledgebasePackage> afterPackages;
        private XElement changeSetElement;

        [TestInitialize]
        public void Initialize()
        {
            beforePackages = new List<KnowledgebasePackage>
            {
                new KnowledgebasePackage() { AdditionalWeight = 1.0, ApplyAdditionalWeight = true, Height = 1, Length = 1, Width = 1, Weight = 1},
                new KnowledgebasePackage() { AdditionalWeight = 2.0, ApplyAdditionalWeight = true, Height = 2, Length = 2, Width = 2, Weight = 2},
                new KnowledgebasePackage() { AdditionalWeight = 3.0, ApplyAdditionalWeight = true, Height = 3, Length = 3, Width = 3, Weight = 3}
            };

            afterPackages = new List<KnowledgebasePackage>
            {
                new KnowledgebasePackage() { AdditionalWeight = 4.0, ApplyAdditionalWeight = true, Height = 4, Length = 4, Width = 4, Weight = 4},
                new KnowledgebasePackage() { AdditionalWeight = 5.0, ApplyAdditionalWeight = true, Height = 5, Length = 5, Width = 5, Weight = 5},
                new KnowledgebasePackage() { AdditionalWeight = 6.0, ApplyAdditionalWeight = true, Height = 6, Length = 6, Width = 6, Weight = 6},
                new KnowledgebasePackage() { AdditionalWeight = 7.0, ApplyAdditionalWeight = true, Height = 7, Length = 7, Width = 7, Weight = 7}
            };

            changeSetElement = new XElement("ChangeSet");
        }

        [Fact]
        public void Write_AddsPackagesNodeToChangeSet_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.AreEqual(1, changeSetElement.Descendants("Packages").Count());
        }

        [Fact]
        public void Write_AddsBeforeNodeToPackages_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.AreEqual(1, changeSetElement.Descendants("Packages").Descendants("Before").Count());
        }
        
        [Fact]
        public void Write_PackageNodesInBeforeNode_MatchNumberOfElementsInBeforeList_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.AreEqual(beforePackages.Count, changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").Count());
        }

        [Fact]
        public void Write_WritesPackageWeight_OfEachBeforePackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.AreEqual(beforePackages[i].Weight.ToString(), packageElement.Descendants("Weight").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageHeight_OfEachBeforePackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.AreEqual(beforePackages[i].Height.ToString(), packageElement.Descendants("Height").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageWidth_OfEachBeforePackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.AreEqual(beforePackages[i].Width.ToString(), packageElement.Descendants("Width").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageLength_OfEachBeforePackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.AreEqual(beforePackages[i].Length.ToString(), packageElement.Descendants("Length").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageAdditionalWeight_OfEachBeforePackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.AreEqual(beforePackages[i].AdditionalWeight.ToString(), packageElement.Descendants("AdditionalWeight").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageApplyAdditionalWeight_OfEachBeforePackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.AreEqual(beforePackages[i].ApplyAdditionalWeight.ToString().ToLower(), packageElement.Descendants("ApplyAdditionalWeight").First().Value);
            }
        }

        [Fact]
        public void Write_DoesNotWritePackageHash_OfEachBeforePackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.AreEqual(0, packageElement.Descendants("Hash").Count());
            }
        }

        [Fact]
        public void Write_AddsAfterNodeToPackages_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.AreEqual(1, changeSetElement.Descendants("Packages").Descendants("After").Count());
        }

        [Fact]
        public void Write_PackageNodesIAfterNode_MatchNumberOfElementsInAfterList_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.AreEqual(afterPackages.Count, changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").Count());
        }

        [Fact]
        public void Write_WritesPackageWeight_OfEachAfterPackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.AreEqual(afterPackages[i].Weight.ToString(), packageElement.Descendants("Weight").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageHeight_OfEachAfterPackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.AreEqual(afterPackages[i].Height.ToString(), packageElement.Descendants("Height").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageWidth_OfEachAfterPackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.AreEqual(afterPackages[i].Width.ToString(), packageElement.Descendants("Width").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageLength_OfEachAfterPackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.AreEqual(afterPackages[i].Length.ToString(), packageElement.Descendants("Length").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageAdditionalWeight_OfEachAfterPackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.AreEqual(afterPackages[i].AdditionalWeight.ToString(), packageElement.Descendants("AdditionalWeight").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageApplyAdditionalWeight_OfEachAfterPackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.AreEqual(afterPackages[i].ApplyAdditionalWeight.ToString().ToLower(), packageElement.Descendants("ApplyAdditionalWeight").First().Value);
            }
        }

        [Fact]
        public void Write_DoesNotWritePackageHash_OfEachAfterPackage_Test()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.AreEqual(0, packageElement.Descendants("Hash").Count());
            }
        }
    }
}
