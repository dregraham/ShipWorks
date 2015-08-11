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

        [Fact]
        public void EnsureArgumentIsNotNull_WithExpressionThrowsArgumentNullException_WhenObjectIsNull()
        {
            string value = "";
            Assert.Throws< ArgumentNullException>(() => MethodConditions.EnsureArgumentIsNotNull((object)null, () => value));
        }

        [Fact]
        public void EnsureArgumentIsNotNull_WithExpressionSpecifiesNameOfArgument_WhenObjectIsNull()
        {
            try
            {
                string value = "";
                MethodConditions.EnsureArgumentIsNotNull((object)null, () => value);
            }
            catch (ArgumentNullException ex)
            {
                Assert.Equal("value", ex.ParamName);
            }
        }

        [Fact]
        public void EnsureArgumentIsNotNull_WithExpressionReturnsObject_WhenObjectIsNotNull()
        {
            object test = new object();
            object result = MethodConditions.EnsureArgumentIsNotNull(test, () => test);
            Assert.Equal(test, result);
        }
    }
}
