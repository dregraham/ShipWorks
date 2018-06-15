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
            Assert.True(new FirstFakeException().IsExceptionType(typeof(FirstFakeException), typeof(SecondFakeException)));
        }

        [Fact]
        public void IsExceptionType_ReturnsFalse_WhenExceptionIsNotInListOfExceptions()
        {
            Assert.False(new FirstFakeException().IsExceptionType(typeof(DerivesFromFirstFakeException), typeof(SecondFakeException)));
        }

        [Fact]
        public void HasExceptionType_ReturnsTrue_WhenExceptionIsOfRequestedType()
        {
            Assert.True(new FirstFakeException().HasExceptionType<FirstFakeException>());
        }

        [Fact]
        public void HasExceptionType_ReturnsTrue_WhenInnerExceptionIsOfRequestedType()
        {
            InvalidOperationException testObject = new InvalidOperationException("test", new FirstFakeException());
            Assert.True(testObject.HasExceptionType<FirstFakeException>());
        }

        [Fact]
        public void HasExceptionType_ReturnsFalse_WhenExceptionIsNotOfRequestedType()
        {
            Assert.False(new FirstFakeException().HasExceptionType<InvalidOperationException>());
        }

        [Fact]
        public void HasExceptionType_ReturnsFalse_WhenInnerExceptionIsNotOfRequestedType()
        {
            InvalidOperationException testObject = new InvalidOperationException("test", new InvalidOperationException());
            Assert.False(testObject.HasExceptionType<FirstFakeException>());
        }

        [Fact]
        public void HasExceptionType_ReturnsFalse_WhenExceptionIsNull()
        {
            FirstFakeException testObject = null;
            Assert.False(testObject.HasExceptionType<InvalidOperationException>());
        }

        public class FirstFakeException : Exception
        { }

        public class SecondFakeException : Exception
        { }

        public class DerivesFromFirstFakeException : FirstFakeException
        { }
    }
}
