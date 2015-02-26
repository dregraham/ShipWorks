using System.Linq;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.ApplicationCore.Nudges;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{
    [TestClass]
    public class NudgeDeserializerTest
    {
        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingNudgeID_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeID").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenNudgeIDNotANumber_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeID").First().SetValue("hi");

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingNudgeType_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeType").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentUri_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentUri").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentUriIsInvalid_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentUri").First().SetValue("asdf");

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensions_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentDimensions").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensionsWidth_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Width").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensionsHeight_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Height").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentDimensionsWidthIsInvalid_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Width").First().SetValue("hi");

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentDimensionsHeightIsInvalid_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Height").First().SetValue("hi");

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingOptions_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Options").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectNudgeID_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(nudge.NudgeID, int.Parse(GetValue(nudgeElement, "NudgeID")));
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectNudgeType_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual((int)nudge.NudgeType, int.Parse(GetValue(nudgeElement, "NudgeType")));
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectContentUri_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(nudge.ContentUri.AbsoluteUri, GetValue(nudgeElement, "ContentUri"));
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectContentDimensions_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(nudge.ContentDimensions.Width, int.Parse(GetValue(nudgeElement.Descendants("ContentDimensions").First(), "Width")));
            Assert.AreEqual(nudge.ContentDimensions.Height, int.Parse(GetValue(nudgeElement.Descendants("ContentDimensions").First(), "Height")));
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectName_WhenNameIsProvided_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(GetValue(nudgeElement, "Name"), nudge.Name);            
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeNameIsEmptyString_WhenNameNodeIsOmittedInXml_Test()
        {
            XElement nudgeElement = XElement.Parse(NudgeXmlWithoutNameNode);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(string.Empty, nudge.Name);
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeNameIsEmptyString_WhenNameNodeIsEmpty_Test()
        {
            XElement nudgeElement = XElement.Parse(NudgeXmlWithEmptyNameNode);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(string.Empty, nudge.Name);
        }
        
        /// <summary>
        /// Gets the string value of an element
        /// </summary>
        private static string GetValue(XElement nudgeOptionElement, string elementName)
        {
            return nudgeOptionElement.Descendants(elementName).First().Value.Trim();
        }

        private const string GoodNudgeXml = @"
            <Nudges>
                 <Nudge>
                     <NudgeID>12345</NudgeID>
                     <Name>The nudge name</Name>
                     <NudgeType>0</NudgeType>
                     <ContentUri>https://www.shipworks.com/blah</ContentUri>
                     <ContentDimensions>
                         <Width>1024</Width>
                         <Height>768</Height>
                     </ContentDimensions>
                     <Options>
                         <Option>
                             <OptionId>1</OptionId>
                             <Index>0</Index>
                             <Text>OK</Text>
                             <Action>0</Action>
                         </Option>
                         <Option>
                             <OptionId>2</OptionId>
                             <Index>1</Index>
                             <Text>Close</Text>
                             <Action>1</Action>
                         </Option>
                     </Options>
                 </Nudge>
             </Nudges>";


        private const string NudgeXmlWithoutNameNode = @"
            <Nudges>
                 <Nudge>
                     <NudgeID>12345</NudgeID>
                     <NudgeType>0</NudgeType>
                     <ContentUri>https://www.shipworks.com/blah</ContentUri>
                     <ContentDimensions>
                         <Width>1024</Width>
                         <Height>768</Height>
                     </ContentDimensions>
                     <Options>
                         <Option>
                             <OptionId>1</OptionId>
                             <Index>0</Index>
                             <Text>OK</Text>
                             <Action>0</Action>
                         </Option>
                         <Option>
                             <OptionId>2</OptionId>
                             <Index>1</Index>
                             <Text>Close</Text>
                             <Action>1</Action>
                         </Option>
                     </Options>
                 </Nudge>
             </Nudges>";

        private const string NudgeXmlWithEmptyNameNode = @"
            <Nudges>
                 <Nudge>
                     <NudgeID>12345</NudgeID>
                     <Name></Name>
                     <NudgeType>0</NudgeType>
                     <ContentUri>https://www.shipworks.com/blah</ContentUri>
                     <ContentDimensions>
                         <Width>1024</Width>
                         <Height>768</Height>
                     </ContentDimensions>
                     <Options>
                         <Option>
                             <OptionId>1</OptionId>
                             <Index>0</Index>
                             <Text>OK</Text>
                             <Action>0</Action>
                         </Option>
                         <Option>
                             <OptionId>2</OptionId>
                             <Index>1</Index>
                             <Text>Close</Text>
                             <Action>1</Action>
                         </Option>
                     </Options>
                 </Nudge>
             </Nudges>";
    }

}
