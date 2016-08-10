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
            Assert.True((new FirstFakeException()).IsExceptionType(typeof(FirstFakeException), typeof(SecondFakeException)));
        }

        [Fact]
        public void IsExceptionType_ReturnsFalse_WhenExceptionIsNotInListOfExceptions()
        {
            Assert.False((new FirstFakeException()).IsExceptionType(typeof(DerivesFromFirstFakeException), typeof(SecondFakeException)));
        }

        public class FirstFakeException : Exception
        { }

        public class SecondFakeException : Exception
        { }

        public class DerivesFromFirstFakeException : FirstFakeException
        { }
    }
}
