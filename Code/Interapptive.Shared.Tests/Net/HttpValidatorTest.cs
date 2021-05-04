using Interapptive.Shared.Net;
using Xunit;

namespace Interapptive.Shared.Tests.Net
{
    public class HttpValidatorTest
    {
        readonly HttpValidator testObject;
        
        public HttpValidatorTest()
        {
            testObject = new HttpValidator();
        }
        
        [Theory]
        [InlineData("127.0.0.1", true)]
        [InlineData("192.168.0.1", true)]
        [InlineData("1.1.1.1", true)]
        [InlineData("255.255.255.255", true)]
        [InlineData(" 1.1.1.1 ", true)]
        [InlineData("300.300.300.300", false)]
        [InlineData("127.1.1", false)]
        [InlineData("a.a.a.a", false)]
        [InlineData("12345", false)]
        [InlineData("1.1.1.1.1", false)]
        [InlineData("null", false)]
        [InlineData("", false)]
        public void ValidateIpAddress(string ipAddress, bool expectedResult)
        {
            var result = testObject.ValidateIPAddress(ipAddress);
            Assert.Equal(expectedResult, result.Success);
            if (expectedResult)
            {
                Assert.Equal(result.Value, ipAddress.Trim());
            }
        }
    }
}