using System;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.ApplicationCore.Nudges;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{
    [TestClass]
    public class NudgeOptionDeserializerTest
    {
        private readonly Nudge nudge;

        public NudgeOptionDeserializerTest()
        {
            nudge = new Nudge(1, NudgeType.ShipWorksUpgrade, new Uri("http://www.google.com"), new Size(500, 400));
        }

        [TestMethod]
        public void NudgeOptionDeserializer_ReturnsCorrectResult_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.AreEqual(GetValue(nudgeOptionElement, "Result"), nudgeOption.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenMissingIndex_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Index").Remove();

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenIndexNotANumber_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Index").First().SetValue("hi");

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenMissingText_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Text").Remove();

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenMissingAction_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Action").Remove();

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }        

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenMissingResult_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Result").Remove();

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }

        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionHasCorrectIndex_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.AreEqual(nudgeOption.Index, int.Parse(GetValue(nudgeOptionElement, "Index")));
        }

        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionHasCorrectAction_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            string value = GetValue(nudgeOptionElement, "Action");
            NudgeOptionActionType action = (NudgeOptionActionType) int.Parse(value);

            Assert.AreEqual(nudgeOption.Action, action);
        }

        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionHasCorrectText_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.AreEqual(nudgeOption.Text, GetValue(nudgeOptionElement, "Text"));
        }

        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionHasCorrectResult_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.AreEqual(nudgeOption.Result, GetValue(nudgeOptionElement, "Result"));
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
                <Index>2</Index>
                <Text>OK</Text>
                <Action>0</Action>
                <Result>OKClicked</Result>
            </Option>"
            ;

        private const string GoodNudgeXml = @"
            <Nudges>
                 <Nudge>
                     <NudgeID>12345</NudgeID>
                     <NudgeType>ShipWorksUpgrade</NudgeType>
                     <ContentUri>https://www.shipworks.com/blah</ContentUri>
                     <ContentDimensions>
                         <Width>1024</Width>
                         <Height>768</Height>
                     </ContentDimensions>
                     <Options>
                         <Option>
                             <Index>0</Index>
                             <Text>OK</Text>
                             <Action>AcknowledgeNudgeAction</Action>
                             <Result>OKClicked</Result>
                         </Option>
                         <Option>
                             <Index>1</Index>
                             <Text>Close</Text>
                             <Action>0</Action>
                             <Result>OKClicked</Result>
                         </Option>
                     </Options>
                 </Nudge>
             </Nudges>";

    }

}
