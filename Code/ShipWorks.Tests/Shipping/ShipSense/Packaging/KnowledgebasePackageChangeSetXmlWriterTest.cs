﻿using System;
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

        public KnowledgebasePackageChangeSetXmlWriterTest()
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
        public void Write_AddsPackagesNodeToChangeSet()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.Equal(1, changeSetElement.Descendants("Packages").Count());
        }

        [Fact]
        public void Write_AddsBeforeNodeToPackages()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.Equal(1, changeSetElement.Descendants("Packages").Descendants("Before").Count());
        }
        
        [Fact]
        public void Write_PackageNodesInBeforeNode_MatchNumberOfElementsInBeforeList()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.Equal(beforePackages.Count, changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").Count());
        }

        [Fact]
        public void Write_WritesPackageWeight_OfEachBeforePackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.Equal(beforePackages[i].Weight.ToString(), packageElement.Descendants("Weight").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageHeight_OfEachBeforePackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.Equal(beforePackages[i].Height.ToString(), packageElement.Descendants("Height").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageWidth_OfEachBeforePackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.Equal(beforePackages[i].Width.ToString(), packageElement.Descendants("Width").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageLength_OfEachBeforePackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.Equal(beforePackages[i].Length.ToString(), packageElement.Descendants("Length").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageAdditionalWeight_OfEachBeforePackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.Equal(beforePackages[i].AdditionalWeight.ToString(), packageElement.Descendants("AdditionalWeight").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageApplyAdditionalWeight_OfEachBeforePackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.Equal(beforePackages[i].ApplyAdditionalWeight.ToString().ToLower(), packageElement.Descendants("ApplyAdditionalWeight").First().Value);
            }
        }

        [Fact]
        public void Write_DoesNotWritePackageHash_OfEachBeforePackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> beforePackageElements = changeSetElement.Descendants("Packages").Descendants("Before").Descendants("Package").ToList();
            for (int i = 0; i < beforePackages.Count; i++)
            {
                XElement packageElement = beforePackageElements[i];
                Assert.Equal(0, packageElement.Descendants("Hash").Count());
            }
        }

        [Fact]
        public void Write_AddsAfterNodeToPackages()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.Equal(1, changeSetElement.Descendants("Packages").Descendants("After").Count());
        }

        [Fact]
        public void Write_PackageNodesIAfterNode_MatchNumberOfElementsInAfterList()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            Assert.Equal(afterPackages.Count, changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").Count());
        }

        [Fact]
        public void Write_WritesPackageWeight_OfEachAfterPackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.Equal(afterPackages[i].Weight.ToString(), packageElement.Descendants("Weight").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageHeight_OfEachAfterPackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.Equal(afterPackages[i].Height.ToString(), packageElement.Descendants("Height").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageWidth_OfEachAfterPackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.Equal(afterPackages[i].Width.ToString(), packageElement.Descendants("Width").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageLength_OfEachAfterPackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.Equal(afterPackages[i].Length.ToString(), packageElement.Descendants("Length").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageAdditionalWeight_OfEachAfterPackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.Equal(afterPackages[i].AdditionalWeight.ToString(), packageElement.Descendants("AdditionalWeight").First().Value);
            }
        }

        [Fact]
        public void Write_WritesPackageApplyAdditionalWeight_OfEachAfterPackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.Equal(afterPackages[i].ApplyAdditionalWeight.ToString().ToLower(), packageElement.Descendants("ApplyAdditionalWeight").First().Value);
            }
        }

        [Fact]
        public void Write_DoesNotWritePackageHash_OfEachAfterPackage()
        {
            testObject = new KnowledgebasePackageChangeSetXmlWriter(beforePackages, afterPackages);
            testObject.WriteTo(changeSetElement);

            List<XElement> afterPackageElements = changeSetElement.Descendants("Packages").Descendants("After").Descendants("Package").ToList();
            for (int i = 0; i < afterPackages.Count; i++)
            {
                XElement packageElement = afterPackageElements[i];
                Assert.Equal(0, packageElement.Descendants("Hash").Count());
            }
        }
    }
}
