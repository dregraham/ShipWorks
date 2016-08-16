using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xunit;
using ShipWorks.Shipping.ShipSense.Customs;
using ShipWorks.Shipping.ShipSense.Packaging;

namespace ShipWorks.Tests.Shipping.ShipSense.Customs
{
    public class KnowledgebaseCustomsItemXmlWriterTest
    {
        private KnowledgebaseCustomsItemXmlWriter testObject;

        private List<KnowledgebaseCustomsItem> beforeCustomsItems;
        private List<KnowledgebaseCustomsItem> afterCustomsItems;
        private XElement changeSetElement;

        public KnowledgebaseCustomsItemXmlWriterTest()
        {
            beforeCustomsItems = new List<KnowledgebaseCustomsItem>
            {
                new KnowledgebaseCustomsItem() { CountryOfOrigin = "US", Description = "Item1 Desc", HarmonizedCode = "hc1", NumberOfPieces = 1, Weight = 1, Quantity = 1, UnitPriceAmount = 1, UnitValue = 1},
                new KnowledgebaseCustomsItem() { CountryOfOrigin = "US", Description = "Item2 Desc", HarmonizedCode = "hc2", NumberOfPieces = 2, Weight = 2, Quantity = 2, UnitPriceAmount = 2, UnitValue = 2},
                new KnowledgebaseCustomsItem() { CountryOfOrigin = "US", Description = "Item3 Desc", HarmonizedCode = "hc3", NumberOfPieces = 3, Weight = 3, Quantity = 3, UnitPriceAmount = 3, UnitValue = 3}
            };

            afterCustomsItems = new List<KnowledgebaseCustomsItem>
            {
                new KnowledgebaseCustomsItem() { CountryOfOrigin = "US", Description = "Item1 Desc", HarmonizedCode = "hc1", NumberOfPieces = 1, Weight = 1, Quantity = 1, UnitPriceAmount = 1, UnitValue = 1},
                new KnowledgebaseCustomsItem() { CountryOfOrigin = "US", Description = "Item2 Desc", HarmonizedCode = "hc2", NumberOfPieces = 2, Weight = 2.2, Quantity = 2, UnitPriceAmount = 2, UnitValue = 2},
                new KnowledgebaseCustomsItem() { CountryOfOrigin = "US", Description = "Item3 Desc", HarmonizedCode = "hc3", NumberOfPieces = 3, Weight = 3.3, Quantity = 3, UnitPriceAmount = 3, UnitValue = 3},
                new KnowledgebaseCustomsItem() { CountryOfOrigin = "US", Description = "Item4 Desc", HarmonizedCode = "hc4", NumberOfPieces = 4, Weight = 4.4, Quantity = 4, UnitPriceAmount = 4, UnitValue = 4}
            };

            changeSetElement = new XElement("ChangeSet");
        }

        [Fact]
        public void Write_AddsCustomsItemsNodeToChangeSet()
        {
            testObject = new KnowledgebaseCustomsItemXmlWriter(beforeCustomsItems, afterCustomsItems);
            testObject.WriteTo(changeSetElement);

            Assert.Equal(1, changeSetElement.Descendants("CustomsItems").Count());
        }

        [Fact]
        public void Write_AddsBeforeAfterNodesToCustomsItems()
        {
            testObject = new KnowledgebaseCustomsItemXmlWriter(beforeCustomsItems, afterCustomsItems);
            testObject.WriteTo(changeSetElement);

            Assert.Equal(1, changeSetElement.Descendants("Before").Count());
            Assert.Equal(1, changeSetElement.Descendants("After").Count());
        }

        [Fact]
        public void Write_BeforeItemsMatchXml()
        {
            testObject = new KnowledgebaseCustomsItemXmlWriter(beforeCustomsItems, afterCustomsItems);
            testObject.WriteTo(changeSetElement);

            for (int i = 0; i < beforeCustomsItems.Count; i++)
            {
                XElement beforeXElement = changeSetElement.Descendants("Before").Descendants("CustomsItem").ElementAt(i);

                Assert.Equal(beforeCustomsItems[i].CountryOfOrigin, beforeXElement.Descendants("CountryOfOrigin").First().Value);
                Assert.Equal(beforeCustomsItems[i].Description, beforeXElement.Descendants("Description").First().Value);
                Assert.Equal(beforeCustomsItems[i].HarmonizedCode, beforeXElement.Descendants("HarmonizedCode").First().Value);
                Assert.Equal(beforeCustomsItems[i].NumberOfPieces.ToString(), beforeXElement.Descendants("NumberOfPieces").First().Value);
                Assert.Equal(beforeCustomsItems[i].Quantity.ToString(), beforeXElement.Descendants("Quantity").First().Value);
                Assert.Equal(beforeCustomsItems[i].UnitPriceAmount.ToString(), beforeXElement.Descendants("UnitPriceAmount").First().Value);
                Assert.Equal(beforeCustomsItems[i].UnitValue.ToString(), beforeXElement.Descendants("UnitValue").First().Value);
                Assert.Equal(beforeCustomsItems[i].Weight.ToString(), beforeXElement.Descendants("Weight").First().Value);    
            }
        }

        [Fact]
        public void Write_AfterItemsMatchXml()
        {
            testObject = new KnowledgebaseCustomsItemXmlWriter(beforeCustomsItems, afterCustomsItems);
            testObject.WriteTo(changeSetElement);

            for (int i = 0; i < afterCustomsItems.Count; i++)
            {
                XElement afterXElement = changeSetElement.Descendants("After").Descendants("CustomsItem").ElementAt(i);

                Assert.Equal(afterCustomsItems[i].CountryOfOrigin, afterXElement.Descendants("CountryOfOrigin").First().Value);
                Assert.Equal(afterCustomsItems[i].Description, afterXElement.Descendants("Description").First().Value);
                Assert.Equal(afterCustomsItems[i].HarmonizedCode, afterXElement.Descendants("HarmonizedCode").First().Value);
                Assert.Equal(afterCustomsItems[i].NumberOfPieces.ToString(), afterXElement.Descendants("NumberOfPieces").First().Value);
                Assert.Equal(afterCustomsItems[i].Quantity.ToString(), afterXElement.Descendants("Quantity").First().Value);
                Assert.Equal(afterCustomsItems[i].UnitPriceAmount.ToString(), afterXElement.Descendants("UnitPriceAmount").First().Value);
                Assert.Equal(afterCustomsItems[i].UnitValue.ToString(), afterXElement.Descendants("UnitValue").First().Value);
                Assert.Equal(afterCustomsItems[i].Weight.ToString(), afterXElement.Descendants("Weight").First().Value);
            }
        }

        [Fact]
        public void Write_DoesNotWriteAnyHash()
        {
            testObject = new KnowledgebaseCustomsItemXmlWriter(beforeCustomsItems, afterCustomsItems);
            testObject.WriteTo(changeSetElement);

            Assert.Equal(0, changeSetElement.Descendants("CustomsItems").Descendants("Hash").Count());
        }
    }
}
