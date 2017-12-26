using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsTermsAndConditionsExceptionTest : IDisposable
    {
        private readonly AutoMock mock;

        public UspsTermsAndConditionsExceptionTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_TermsAndCondionsFieldSet()
        {
            var termsAndConditionsMock = mock.CreateMock<IUspsTermsAndConditions>();
            var testObject = new UspsTermsAndConditionsException("message", termsAndConditionsMock.Object);

            Assert.Equal(termsAndConditionsMock.Object, testObject.TermsAndConditions);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}