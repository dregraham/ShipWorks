using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipWorks.Tests.Interapptive.Shared.Utility
{
    [TestClass]
    public class ObjectUtilityTest
    {
        [TestMethod]
        public void Nameof_ReturnsName_WithProperty()
        {
            string testObject = "foo";
            string result = ObjectUtility.Nameof(() => testObject.Length);
            Assert.AreEqual("Length", result);
        }

        [TestMethod]
        public void Nameof_ReturnsName_WithMethodInvocation()
        {
            string testObject = "foo";
            string result = ObjectUtility.Nameof(() => testObject.ToLower());
            Assert.AreEqual("ToLower", result);
        }

        [TestMethod]
        public void Nameof_ReturnsName_WithPropertyOfNullObject()
        {
            string result = ObjectUtility.Nameof(() => ((string)null).Length);
            Assert.AreEqual("Length", result);
        }
    }
}
