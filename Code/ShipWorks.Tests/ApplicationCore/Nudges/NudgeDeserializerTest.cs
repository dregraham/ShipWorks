using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz.Util;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Nudges.NudgeActions;
using ShipWorks.Filters.Content.Conditions.Orders;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{

    [TestClass]
    public class NudgeDeserializerTest
    {
        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingNudgeID()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeID").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenNudgeIDNotANumber()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeID").First().SetValue("hi");

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingNudgeType()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("NudgeType").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentUri()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentUri").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentUriIsInvalid()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentUri").First().SetValue("asdf");

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensions()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("ContentDimensions").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensionsWidth()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Width").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingContentDimensionsHeight()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Height").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentDimensionsWidthIsInvalid()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Width").First().SetValue("hi");

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenContentDimensionsHeightIsInvalid()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Height").First().SetValue("hi");

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void NudgeDeserializer_ThrowsNudgeOptionException_WhenMissingOptions()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeXml);
            nudgeOptionElement.Descendants("Options").Remove();

            NudgeDeserializer.Deserialize(nudgeOptionElement);
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectNudgeID()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(nudge.NudgeID, int.Parse(GetValue(nudgeElement, "NudgeID")));
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectNudgeType()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(EnumHelper.GetApiValue(nudge.NudgeType),  GetValue(nudgeElement, "NudgeType"));
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectContentUri()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(nudge.ContentUri.AbsoluteUri, GetValue(nudgeElement, "ContentUri"));
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectContentDimensions()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(nudge.ContentDimensions.Width, int.Parse(GetValue(nudgeElement.Descendants("ContentDimensions").First(), "Width")));
            Assert.AreEqual(nudge.ContentDimensions.Height, int.Parse(GetValue(nudgeElement.Descendants("ContentDimensions").First(), "Height")));
        }

        [TestMethod]
        public void NudgeDeserializer_NudgeHasCorrectOptions()
        {
            XElement nudgeElement = XElement.Parse(GoodNudgeXml);

            Nudge nudge = NudgeDeserializer.Deserialize(nudgeElement);

            Assert.AreEqual(nudgeElement.Descendants("Option").Count(), nudge.NudgeOptions.Count);

            foreach (XElement optionElement in nudgeElement.Descendants("Option"))
            {
                int index = int.Parse(GetValue(optionElement, "Index"));
                NudgeOption nudgeOption = nudge.NudgeOptions.First(no => no.Value.Index == index).Value;

                Assert.AreEqual(nudgeOption.Result, GetValue(optionElement, "Result"));
                Assert.AreEqual(nudgeOption.Action.ToString(), string.Format("ShipWorks.ApplicationCore.Nudges.NudgeActions.{0}", GetValue(optionElement, "Action")));
                Assert.AreEqual(nudgeOption.Text, GetValue(optionElement, "Text"));
            }
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
