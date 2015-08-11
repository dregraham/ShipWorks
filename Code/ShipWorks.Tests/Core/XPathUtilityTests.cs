using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.Utility;

namespace ShipWorks.Tests.Core
{
    public class XPathUtilityTests
    {
        static XmlDocument xmlDocument;
        static XPathNavigator xpath;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            string xml = @"
                <Root>
                    <Car>
                        <Color>Blue</Color>
                        <Wheels>4</Wheels>
                        <Diesel>false</Diesel>
                        <MPG>31.34</MPG>
                    </Car>
                     <Truck>
                        <Color>White</Color>
                        <Wheels>18</Wheels>
                        <Diesel>true</Diesel>
                        <MPG>12.25</MPG>
                    </Truck>
                    <EmptyString></EmptyString>
                </Root>";

            xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            xpath = xmlDocument.CreateNavigator();
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EvaluateNullXpath()
        {
            XPathUtility.Evaluate(null, "Whatever", "Nothing");
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EvaluateNullNodePath()
        {
            XPathUtility.Evaluate(xpath, null, "Nothing");
        }

        [Fact]
        public void EvaluateString()
        {
            string result = XPathUtility.Evaluate(xpath, "//Car/Color", "Black");

            Assert.AreEqual("Blue", result);
        }

        [Fact]
        public void EvaluateMissingString()
        {
            string result = XPathUtility.Evaluate(xpath, "//EmptyString", "NonEmtpy");

            Assert.AreEqual(string.Empty, result);
        }

        [Fact]
        public void EvaluateDefaultString()
        {
            string result = XPathUtility.Evaluate(xpath, "//Does/Not/Exist", "MyDefault");

            Assert.AreEqual("MyDefault", result);
        }

        [Fact]
        public void EvaluateNodeSetString()
        {
            string result = XPathUtility.Evaluate(xpath, "//Color", "Multiple");

            // Just returns the first
            Assert.AreEqual("Blue", result);
        }

        [Fact]
        public void EvaluateInt()
        {
            int result = XPathUtility.Evaluate(xpath, "//Truck/Wheels", 2);

            Assert.AreEqual(18, result);
        }

        [Fact]
        public void EvaluateMissingInt()
        {
            int result = XPathUtility.Evaluate(xpath, "//Van/Nothing", 4);

            Assert.AreEqual(4, result);
        }

        [Fact]
        [ExpectedException(typeof(FormatException))]
        public void EvaluateNonInt()
        {
            XPathUtility.Evaluate(xpath, "//Car/Color", 12);
        }

        [Fact]
        public void EvaluateDecimal()
        {
            decimal result = XPathUtility.Evaluate(xpath, "//Car/MPG", 1.25m);

            Assert.AreEqual(31.34m, result);
        }

        [Fact]
        [ExpectedException(typeof(FormatException))]
        public void EvaluateNonDecimal()
        {
            XPathUtility.Evaluate(xpath, "//Car/Color", 12.5m);
        }

        [Fact]
        public void EvaluateMissingDecimal()
        {
            decimal result = XPathUtility.Evaluate(xpath, "//Van/Nothing", 4.5m);

            Assert.AreEqual(4.5m, result);
        }

        [Fact]
        [ExpectedException(typeof(FormatException))]
        public void EvaluateDecimalAsInt()
        {
            int result = XPathUtility.Evaluate(xpath, "//Car/MPG", 1);
        }

        [Fact]
        public void EvaluateDouble()
        {
            double result = XPathUtility.Evaluate(xpath, "//Truck/MPG", 1.25);

            Assert.AreEqual(12.25, result);
        }

        [Fact]
        [ExpectedException(typeof(FormatException))]
        public void EvaluateNonDouble()
        {
            XPathUtility.Evaluate(xpath, "//Car/Color", 12.5);
        }

        [Fact]
        public void EvaluateMissingDouble()
        {
            double result = XPathUtility.Evaluate(xpath, "//Van/Nothing", 8.98);

            Assert.AreEqual(8.98, result);
        }

        [Fact]
        public void EvaluateBool()
        {
            bool result = XPathUtility.Evaluate(xpath, "//Truck/Diesel", false);

            Assert.AreEqual(true, result);
        }

        [Fact]
        [ExpectedException(typeof(FormatException))]
        public void EvaluateNonBool()
        {
            XPathUtility.Evaluate(xpath, "//Car/Color", true);
        }

        [Fact]
        public void EvaluateMissingBool()
        {
            bool result = XPathUtility.Evaluate(xpath, "//Van/Nothing", true);

            Assert.AreEqual(true, result);
        }
    }
}
