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

        static XPathUtilityTests()
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
        public void EvaluateNullXpath()
        {
            Assert.Throws<ArgumentNullException>(() => XPathUtility.Evaluate(null, "Whatever", "Nothing"));
        }

        [Fact]
        public void EvaluateNullNodePath()
        {
            Assert.Throws<ArgumentNullException>(() => XPathUtility.Evaluate(xpath, null, "Nothing"));
        }

        [Fact]
        public void EvaluateString()
        {
            string result = XPathUtility.Evaluate(xpath, "//Car/Color", "Black");

            Assert.Equal("Blue", result);
        }

        [Fact]
        public void EvaluateMissingString()
        {
            string result = XPathUtility.Evaluate(xpath, "//EmptyString", "NonEmtpy");

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EvaluateDefaultString()
        {
            string result = XPathUtility.Evaluate(xpath, "//Does/Not/Exist", "MyDefault");

            Assert.Equal("MyDefault", result);
        }

        [Fact]
        public void EvaluateNodeSetString()
        {
            string result = XPathUtility.Evaluate(xpath, "//Color", "Multiple");

            // Just returns the first
            Assert.Equal("Blue", result);
        }

        [Fact]
        public void EvaluateInt()
        {
            int result = XPathUtility.Evaluate(xpath, "//Truck/Wheels", 2);

            Assert.Equal(18, result);
        }

        [Fact]
        public void EvaluateMissingInt()
        {
            int result = XPathUtility.Evaluate(xpath, "//Van/Nothing", 4);

            Assert.Equal(4, result);
        }

        [Fact]
        public void EvaluateNonInt()
        {
            Assert.Throws<FormatException>(() => XPathUtility.Evaluate(xpath, "//Car/Color", 12));
        }

        [Fact]
        public void EvaluateDecimal()
        {
            decimal result = XPathUtility.Evaluate(xpath, "//Car/MPG", 1.25m);

            Assert.Equal(31.34m, result);
        }

        [Fact]
        public void EvaluateNonDecimal()
        {
            Assert.Throws<FormatException>(() => XPathUtility.Evaluate(xpath, "//Car/Color", 12.5m));
        }

        [Fact]
        public void EvaluateMissingDecimal()
        {
            decimal result = XPathUtility.Evaluate(xpath, "//Van/Nothing", 4.5m);

            Assert.Equal(4.5m, result);
        }

        [Fact]
        public void EvaluateDecimalAsInt()
        {
            Assert.Throws<FormatException>(() => XPathUtility.Evaluate(xpath, "//Car/MPG", 1));
        }

        [Fact]
        public void EvaluateDouble()
        {
            double result = XPathUtility.Evaluate(xpath, "//Truck/MPG", 1.25);

            Assert.Equal(12.25, result);
        }

        [Fact]
        public void EvaluateNonDouble()
        {
            Assert.Throws<FormatException>(() => XPathUtility.Evaluate(xpath, "//Car/Color", 12.5));
        }

        [Fact]
        public void EvaluateMissingDouble()
        {
            double result = XPathUtility.Evaluate(xpath, "//Van/Nothing", 8.98);

            Assert.Equal(8.98, result);
        }

        [Fact]
        public void EvaluateBool()
        {
            bool result = XPathUtility.Evaluate(xpath, "//Truck/Diesel", false);

            Assert.Equal(true, result);
        }

        [Fact]
        public void EvaluateNonBool()
        {
            Assert.Throws<FormatException>(() => XPathUtility.Evaluate(xpath, "//Car/Color", true));
        }

        [Fact]
        public void EvaluateMissingBool()
        {
            bool result = XPathUtility.Evaluate(xpath, "//Van/Nothing", true);

            Assert.Equal(true, result);
        }
    }
}
