﻿using System;
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
        [ExpectedException(typeof(NudgeException))]
        public void Deserialize_ThrowsNudgeOptionException_WhenMissingIndex_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Index").Remove();

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void Deserialize_ThrowsNudgeOptionException_WhenIndexNotANumber_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Index").First().SetValue("hi");

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void Deserialize_ThrowsNudgeOptionException_WhenMissingText_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Text").Remove();

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void Deserialize_ThrowsNudgeOptionException_WhenMissingAction_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Action").Remove();

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
        }        

        [TestMethod]
        public void Deserialize_NudgeOptionHasCorrectIndex_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.AreEqual(int.Parse(GetValue(nudgeOptionElement, "Index")), nudgeOption.Index);
        }

        [TestMethod]
        public void Deserialize_NudgeOptionHasCorrectAction_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            string value = GetValue(nudgeOptionElement, "Action");
            NudgeOptionActionType action = (NudgeOptionActionType) int.Parse(value);

            Assert.AreEqual(action, nudgeOption.Action);
        }

        [TestMethod]
        public void Deserialize_NudgeOptionHasCorrectAction_ForRegisterStampsAccount_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("Action").First().SetValue(((int)NudgeOptionActionType.RegisterStampsAccount).ToString());

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            string value = GetValue(nudgeOptionElement, "Action");
            NudgeOptionActionType action = (NudgeOptionActionType)int.Parse(value);

            Assert.AreEqual(action, nudgeOption.Action);
        }

        [TestMethod]
        public void Deserialize_NudgeOptionHasCorrectText_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.AreEqual(GetValue(nudgeOptionElement, "Text"), nudgeOption.Text);
        }

        [TestMethod]
        public void Deserialize_SetsNudgeOptionID_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);

            NudgeOption nudgeOption = NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);

            Assert.AreEqual(int.Parse(GetValue(nudgeOptionElement, "NudgeOptionID")), nudgeOption.NudgeOptionID);
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void Deserialize_ThrowsNudgeException_WhenNudgeIDIsMissing_Test()
        {
            XElement nudgeOptionElement = XElement.Parse(GoodNudgeOptionXml);
            nudgeOptionElement.Descendants("NudgeOptionID").Remove();

            NudgeOptionDeserializer.Deserialize(nudge, nudgeOptionElement);
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
                <NudgeOptionID>1</NudgeOptionID>
                <Index>2</Index>
                <Text>OK</Text>
                <Action>0</Action>
                <Result>OKClicked</Result>
            </Option>"
            ;        
    }

}
