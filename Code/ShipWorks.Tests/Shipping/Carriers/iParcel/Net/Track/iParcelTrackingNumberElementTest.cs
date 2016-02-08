using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Net.Track;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net.Track
{
    public class iParcelTrackingNumberElementTest
    {
        private IParcelPackageEntity package;

        private iParcelTrackingNumberElement testObject;

        public iParcelTrackingNumberElementTest()
        {
            package=new IParcelPackageEntity()
            {
                TrackingNumber = "42"
            };

            testObject = new iParcelTrackingNumberElement(package);
        }

        [Fact]
        public void Build_AddsTrackingNumberElement()
        {
            XElement element = testObject.Build();

            Assert.Equal("TrackingNumber", element.Name.LocalName);
        }

        [Fact]
        public void Build_AddsTrackingNumber()
        {
            XElement element = testObject.Build();

            Assert.Equal("42", element.Value);
        }
    }
}
