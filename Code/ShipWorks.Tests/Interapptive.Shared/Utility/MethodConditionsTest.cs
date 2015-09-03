using System;
using Interapptive.Shared.Utility;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Utility
{
    public class MethodConditionsTest
    {
        [Fact]
        public void EnsureArgumentIsNotNull_ThrowsArgumentNullException_WhenObjectIsNull()
        {
            Assert.Throws< ArgumentNullException>(() => MethodConditions.EnsureArgumentIsNotNull((object)null, "null"));
        }

        [Fact]
        public void EnsureArgumentIsNotNull_SpecifiesNameOfArgument_WhenObjectIsNull()
        {
            try
            {
                MethodConditions.EnsureArgumentIsNotNull((object)null, "value");
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal("value", ex.ParamName);
            }
        }

        [Fact]
        public void EnsureArgumentIsNotNull_ReturnsObject_WhenObjectIsNotNull()
        {
            object test = new object();
            object result = MethodConditions.EnsureArgumentIsNotNull(test, "null");
            Assert.Equal(test, result);
        }
    }
}
