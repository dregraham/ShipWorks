using System;
using ShipWorks.Shipping.Insurance.InsureShip;
using Xunit;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    public class InsureShipSettingsTest : IDisposable
    {
        private InsureShipSettings testObject = new InsureShipSettings();
        private readonly bool initialUseTestServer;

        public InsureShipSettingsTest()
        {
            initialUseTestServer = testObject.UseTestServer;
        }

        public void Dispose()
        {
            testObject.UseTestServer = initialUseTestServer;
        }

        [Fact]
        public void UseTestServer_SavesAsTrue()
        {
            testObject.UseTestServer = false;
            testObject.UseTestServer = true;

            Assert.True(testObject.UseTestServer);
        }

        [Fact]
        public void UseTestServer_SavesAsFalse()
        {
            testObject.UseTestServer = true;
            testObject.UseTestServer = false;

            Assert.False(testObject.UseTestServer);
        }

        [Fact]
        public void Url_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.Equal("https://api.insureship.com/", testObject.ApiUrl.AbsoluteUri);
        }

        [Fact]
        public void Url_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal("https://api.insureship.com/", testObject.ApiUrl.AbsoluteUri);
        }

        [Fact]
        public void SubmitClaimDelayTimespan_IsSevenDays()
        {
            Assert.Equal(TimeSpan.FromDays(8).Ticks, testObject.ClaimSubmissionWaitingPeriod.Ticks);
        }

        [Fact]
        public void VoidPolicyMaximumAge_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal(TimeSpan.FromHours(24), InsureShipSettings.VoidPolicyMaximumAge);
        }

        [Fact]
        public void VoidPolicyMaximumAge_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal(TimeSpan.FromHours(24), InsureShipSettings.VoidPolicyMaximumAge);
        }
    }
}
