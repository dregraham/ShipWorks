using System;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using ShipWorks.ApplicationCore.Nudges;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{
    public class NudgeOptionDeserializerTest
    {
        private readonly Nudge nudge;

        public NudgeOptionDeserializerTest()
        {
            nudge = new Nudge(1, "Nudge 1", NudgeType.ShipWorksUpgrade, new Uri("http://www.google.com"), new Size(500, 400));
        }
        
        [Fact]
        public void Deserialize_ThrowsNudgeOptionException_WhenMissingIndex_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Index").Remove();

            Assert.Throws<NudgeException>(() => NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement));
        }

        [Fact]
        public void Deserialize_ThrowsNudgeOptionException_WhenIndexNotANumber_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Index").First().SetValue("hi");

            Assert.Throws<NudgeException>(() => NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement));
        }

        [Fact]
        public void Deserialize_ThrowsNudgeOptionException_WhenMissingText_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Text").Remove();

            Assert.Throws<NudgeException>(() => NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement));
        }

        [Fact]
        public void Deserialize_ThrowsNudgeOptionException_WhenMissingAction_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Action").Remove();

            Assert.Throws<NudgeException>(() => NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement));
        }                

        [Fact]
        public void Deserialize_NudgeOptionHasCorrectIndex_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.Equal(int.Parse(GetValue(nudgeOptionElement, "Index")), nudgeOption.Index);
        }

        [Fact]
        public void Deserialize_NudgeOptionHasCorrectAction_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            string value = GetValue(nudgeOptionElement, "Action");
            NudgeOptionActionType action = (NudgeOptionActionType) int.Parse(value);

            Assert.Equal(action, nudgeOption.Action);
        }

        [Fact]
        public void Deserialize_NudgeOptionHasCorrectText_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.Equal(GetValue(nudgeOptionElement, "Text"), nudgeOption.Text);
        }
        
        [Fact]
        public void Deserialize_SetsNudgeOptionID_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.Equal(int.Parse(GetValue(nudgeOptionElement, "OptionId")), nudgeOption.NudgeOptionID);
        }

        [Fact]
        public void Deserialize_ThrowsNudgeException_WhenNudgeIDIsMissing_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("OptionId").Remove();

            Assert.Throws<NudgeException>(() => NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement));
        }


        /// <summary>
        /// Gets the string value of an element
        /// </summary>
        private static string GetValue(XElement nudgeOptionElement, string elementName)
        {
            return nudgeOptionElement.Descendants(elementName).First().Value.Trim();
        }

        private const string GoodNudgeOptionXml = @"
            <Option>
                <OptionId>1</OptionId>
                <Index>2</Index>
                <Text>OK</Text>
                <Action>0</Action>
            </Option>"
            ;        
    }

}
