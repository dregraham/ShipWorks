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
            object value = null;
            Assert.Throws<ArgumentNullException>(() => MethodConditions.EnsureArgumentIsNotNull(value, nameof(value)));
        }

        [Fact]
        public void EnsureArgumentIsNotNull_SpecifiesNameOfArgument_WhenObjectIsNull()
        {
            try
            {
                object value = null;
                MethodConditions.EnsureArgumentIsNotNull(value, nameof(value));
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
            object result = MethodConditions.EnsureArgumentIsNotNull(test, nameof(test));
            Assert.Equal(test, result);
        }
    }
}
