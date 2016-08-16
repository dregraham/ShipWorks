using System;
using ShipWorks.Stores.Platforms.Sears;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Sears
{
    public class SearsCipherKeyTest
    {
        [Fact]
        public void IV_ReturnsCorrectValue()
        {
            byte[] expectedIV = { 84, 104, 101, 68, 111, 111, 115, 107, 101, 114, 110, 111, 111, 100, 108, 101 };
            SearsCipherKey testObject = new SearsCipherKey();
            Assert.Equal(expectedIV, testObject.InitializationVector);
        }

        [Fact]
        public void Key_ReturnsCorrectValue()
        {
            byte[] expectedKey = new Guid("{A2FC95D9-F255-4D23-B86C-756889A51C6A}").ToByteArray();
            SearsCipherKey testObject = new SearsCipherKey();
            Assert.Equal(expectedKey, testObject.Key);
        }
    }
}
