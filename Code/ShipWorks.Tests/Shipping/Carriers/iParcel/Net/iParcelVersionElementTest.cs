using System.Xml.Linq;
using Xunit;
using ShipWorks.Shipping.Carriers.iParcel.Net;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.Net
{
    public class iParcelVersionElementTest
    {
        private iParcelVersionElement testObject;

        [TestInitialize]
        public void Initialize()
        {
            testObject = new iParcelVersionElement();
        }

        [Fact]
        public void Build_AddsVersionElement_Test()
        {
            XElement element = testObject.Build();

            Assert.AreEqual("Version", element.Name);
        }

        [Fact]
        public void Build_VersionNumber_Test()
        {
            XElement element = testObject.Build();
            Assert.AreEqual("3.3", element.Value);
        }
    }
}
