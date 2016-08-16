using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip
{
    public class InsureShipSettingsTest : IDisposable
    {
        InsureShipSettings testObject = new InsureShipSettings();
        private bool initialUseTestServer;

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

            Assert.Equal("shipworks", testObject.Username);
        }

        [Fact]
        public void Username_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal("shipworks", testObject.Username);
        }

        [Fact]
        public void Password_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = true;

            Assert.Equal("shipworks123", testObject.Password);
        }

        [Fact]
        public void Password_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal("624c55cb00f588f0fe1a79", testObject.Password);
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

            Assert.Equal("https://int.insureship.com/api/", testObject.ApiUrl.AbsoluteUri);
        }

        [Fact]
        public void Url_IsCorrect_WhenUsingProductionServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal("https://api2.insureship.com/api/", testObject.ApiUrl.AbsoluteUri);
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

            Assert.Equal(TimeSpan.FromHours(24), testObject.VoidPolicyMaximumAge);
        }

        [Fact]
        public void VoidPolicyMaximumAge_IsCorrect_WhenUsingTestServer()
        {
            testObject.UseTestServer = false;

            Assert.Equal(TimeSpan.FromHours(24), testObject.VoidPolicyMaximumAge);
        }
    }
}
