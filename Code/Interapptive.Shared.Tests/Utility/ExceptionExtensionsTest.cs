using Interapptive.Shared.Utility;
using System;
using Xunit;

namespace Interapptive.Shared.Tests.Utility
{
    public class ExceptionExtensionsTest
    {
        [Fact]
        public void IsExceptionType_ReturnsTrue_WhenExceptionIsInListOfExceptions()
        {
            Assert.True((new FakeException1()).IsExceptionType(typeof(FakeException1), typeof(FakeException2)));
        }

        [Fact]
        public void IsExceptionType_ReturnsFalse_WhenExceptionIsNotInListOfExceptions()
        {
            Assert.False((new FakeException1()).IsExceptionType(typeof(DerivesFromFakeException1), typeof(FakeException2)));
        }

        public class FakeException1 : Exception
        { }

        public class FakeException2 : Exception
        { }

        public class DerivesFromFakeException1 : FakeException1
        { }
    }
}
