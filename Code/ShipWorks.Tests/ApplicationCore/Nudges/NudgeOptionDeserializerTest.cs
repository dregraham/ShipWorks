using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz.Util;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Nudges.NudgeActions;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{

    [TestClass]
    public class NudgeOptionDeserializerTest
    {
        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionAction_ReturnsCorrectResult()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudgeOptionElement);

            string result = nudgeOption.Action.Execute();

            Assert.AreEqual(GetValue(nudgeOptionElement, "Result"), result);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenMissingIndex()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Index").Remove();

            NudgeOptionDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenIndexNotANumber()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Index").First().SetValue("hi");

            NudgeOptionDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenMissingText()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Text").Remove();

            NudgeOptionDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenMissingAction()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Action").Remove();

            NudgeOptionDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenActionInvalid()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Action").First().SetValue("bad type name");

            NudgeOptionDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeOptionDeserializer_ThrowsNudgeOptionException_WhenMissingResult()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Result").Remove();

            NudgeOptionDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionHasCorrectIndex()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudgeOptionElement);

            Assert.AreEqual(nudgeOption.Index, int.Parse(GetValue(nudgeOptionElement, "Index")));
        }

        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionHasCorrectAction()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudgeOptionElement);

            string fullActionPath = string.Format("ShipWorks.ApplicationCore.Nudges.NudgeActions.{0}", GetValue(nudgeOptionElement, "Action"));
            Assert.AreEqual(nudgeOption.Action.ToString(), fullActionPath);
        }

        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionHasCorrectText()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudgeOptionElement);

            Assert.AreEqual(nudgeOption.Text, GetValue(nudgeOptionElement, "Text"));
        }

        [TestMethod]
        public void NudgeOptionDeserializer_NudgeOptionHasCorrectResult()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudgeOptionElement);

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
                <Action>AcknowledgeNudgeAction</Action>
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
                             <Action>AcknowledgeNudgeAction</Action>
                             <Result>OKClicked</Result>
                         </Option>
                     </Options>
                 </Nudge>
             </Nudges>";

    }

}
