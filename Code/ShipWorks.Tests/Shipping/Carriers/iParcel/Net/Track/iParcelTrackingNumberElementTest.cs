using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Net.Track;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net.Track
{
    [TestClass]
    public class iParcelTrackingNumberElementTest
    {
        private IParcelPackageEntity package;

        private iParcelTrackingNumberElement testObject;

        [TestInitialize]
        public void Initialize()
        {
            package=new IParcelPackageEntity()
            {
                TrackingNumber = "42"
            };

            testObject = new iParcelTrackingNumberElement(package);
        }

        [TestMethod]
        public void Build_AddsTrackingNumberElement_Test()
        {
            XElement element = testObject.Build();

            Assert.AreEqual("TrackingNumber", element.Name.LocalName);
        }

        [TestMethod]
        public void Build_AddsTrackingNumber_Test()
        {
            XElement element = testObject.Build();

            Assert.AreEqual("42", element.Value);
        }
    }
}
