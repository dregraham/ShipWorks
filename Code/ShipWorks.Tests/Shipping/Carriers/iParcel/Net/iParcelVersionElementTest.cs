using System.Xml.Linq;
using Xunit;
using ShipWorks.Shipping.Carriers.iParcel.Net;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net
{
    public class iParcelVersionElementTest
    {
        private iParcelVersionElement testObject;

        public iParcelVersionElementTest()
        {
            testObject = new iParcelVersionElement();
        }

        [Fact]
        public void Build_AddsVersionElement_Test()
        {
            XElement element = testObject.Build();

            Assert.Equal("Version", element.Name);
        }

        [Fact]
        public void Build_VersionNumber_Test()
        {
            XElement element = testObject.Build();
            Assert.Equal("3.3", element.Value);
        }
    }
}
