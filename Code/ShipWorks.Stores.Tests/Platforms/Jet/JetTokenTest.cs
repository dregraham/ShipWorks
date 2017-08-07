using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetTokenTest
    {
        private readonly AutoMock mock;

        public JetTokenTest()
        {
           mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void IsValid_ReturnsTrue_WhenTokenIsNotEmpty()
        {
            Assert.True(new JetToken("valid token").IsValid);
        }

        [Fact]
        public void IsValid_ReturnsFalse_WhenTokenIsEmpty()
        {
            Assert.False(new JetToken(string.Empty).IsValid);
        }

        [Fact]
        public void InvalidToken_ReturnsTokenThatIsNotValid()
        {
            Assert.False(JetToken.InvalidToken.IsValid);
        }

        [Fact]
        public void AttachTo_AddsAuthHeaderToRequest()
        {
            Mock<IHttpRequestSubmitter> request = mock.Mock<IHttpRequestSubmitter>();
            new JetToken("valid token").AttachTo(request.Object);

            request.Verify(r => r.Headers.Set("Authorization", "bearer valid token"));
        }
    }
}