﻿using System.Linq;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using Xunit;
using ShipWorks.ApplicationCore.Nudges;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{
    public class NudgeDeserializerTest
    {
        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingNudgeID_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeID").Remove();

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenNudgeIDNotANumber_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeID").First().SetValue("hi");
            
            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingNudgeType_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeType").Remove();

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentUri_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentUri").Remove();

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentUriIsInvalid_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentUri").First().SetValue("asdf");

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensions_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentDimensions").Remove();

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensionsWidth_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Width").Remove();

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensionsHeight_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Height").Remove();

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentDimensionsWidthIsInvalid_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Width").First().SetValue("hi");

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentDimensionsHeightIsInvalid_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Height").First().SetValue("hi");

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingOptions_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Options").Remove();

            Assert.Throws<NudgeException>(() => NudgeDeserializer.Deserialize(nudgeOptionElement));
        }

        [Fact]
        public void NudgeDeserializer_NudgeHasCorrectNudgeID_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.Equal(nudge.NudgeID, int.Parse(GetValue(nudgeElement, "NudgeID")));
        }

        [Fact]
        public void NudgeDeserializer_NudgeHasCorrectNudgeType_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.Equal((int)nudge.NudgeType, int.Parse(GetValue(nudgeElement, "NudgeType")));
        }

        [Fact]
        public void NudgeDeserializer_NudgeHasCorrectContentUri_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.Equal(nudge.ContentUri.AbsoluteUri, GetValue(nudgeElement, "ContentUri"));
        }

        [Fact]
        public void NudgeDeserializer_NudgeHasCorrectContentDimensions_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.Equal(nudge.ContentDimensions.Width, int.Parse(GetValue(nudgeElement.Descendants("ContentDimensions").First(), "Width")));
            Assert.Equal(nudge.ContentDimensions.Height, int.Parse(GetValue(nudgeElement.Descendants("ContentDimensions").First(), "Height")));
        }

        [Fact]
        public void NudgeDeserializer_NudgeHasCorrectName_WhenNameIsProvided_Test()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.Equal(GetValue(nudgeElement, "Name"), nudge.Name);            
        }

        [Fact]
        public void NudgeDeserializer_NudgeNameIsEmptyString_WhenNameNodeIsOmittedInXml_Test()
        {
            XElement nudgeElement = XElement.Parse(NudgeXmlWithoutNameNode);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.Equal(string.Empty, nudge.Name);
        }

        [Fact]
        public void NudgeDeserializer_NudgeNameIsEmptyString_WhenNameNodeIsEmpty_Test()
        {
            XElement nudgeElement = XElement.Parse(NudgeXmlWithEmptyNameNode);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.Equal(string.Empty, nudge.Name);
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
