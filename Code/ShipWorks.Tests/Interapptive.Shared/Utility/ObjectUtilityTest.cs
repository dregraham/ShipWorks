using Interapptive.Shared.Utility;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Utility
{
    public class ObjectUtilityTest
    {
        [Fact]
        public void Nameof_ReturnsName_WithProperty()
        {
            string testObject = "foo";
            string result = ObjectUtility.Nameof(() => testObject.Length);
            Assert.Equal("Length", result);
        }

        [Fact]
        public void Nameof_ReturnsName_WithMethodInvocation()
        {
            string testObject = "foo";
            string result = ObjectUtility.Nameof(() => testObject.ToLower());
            Assert.Equal("ToLower", result);
        }

        [Fact]
        public void Nameof_ReturnsName_WithPropertyOfNullObject()
        {
            string result = ObjectUtility.Nameof(() => ((string)null).Length);
            Assert.Equal("Length", result);
        }
    }
}
