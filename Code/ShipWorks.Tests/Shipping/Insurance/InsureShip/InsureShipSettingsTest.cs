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
        public void Username_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.Equal("77351", testObject.ClientID);
        }

        [Fact]
        public void Username_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal("77351", testObject.ClientID);
        }

        [Fact]
        public void Password_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.Equal("879ed0612b6e1a5084effb6fa6769ba72cf6ef19f4413536a694d0f06ffed09c615d95cc65dcb22a865ddbb4b942460fea481dd1384e19dc6886cca67fac0229", testObject.ApiKey);
        }

        [Fact]
        public void Password_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal("879ed0612b6e1a5084effb6fa6769ba72cf6ef19f4413536a694d0f06ffed09c615d95cc65dcb22a865ddbb4b942460fea481dd1384e19dc6886cca67fac0229", testObject.ApiKey);
        }

        [Fact]
        public void DistributorID_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.Equal("D00050", testObject.DistributorID);
        }

        [Fact]
        public void DistributorID_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal("D00050", testObject.DistributorID);
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
