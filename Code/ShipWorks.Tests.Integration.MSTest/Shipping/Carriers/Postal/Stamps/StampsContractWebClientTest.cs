using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal.Stamps
{
    [TestClass]
    public class StampsContractWebClientTest
    {
        private readonly StampsContractWebClient testObject;

        private readonly StampsAccountEntity resellerAccount;

        public StampsContractWebClientTest()
        {
            // This will initialize all of the various static classes
            new StampsPrototypeFixture();

            resellerAccount = new StampsAccountEntity
            {
                Username = "interapptive",
                Password = "AYSaiZOMP3UcalGuDB+4aA=="
            };

            testObject = new StampsContractWebClient(true, new TrustingCertificateInspector());
        }

        [TestCategory("Stamps")]
        [TestCategory("ContinuousIntegration")]
        [TestMethod]
        public void GetContract_Connectivity_Test()
        {
            StampsAccountContractType contractType = testObject.GetContractType(resellerAccount);

            Assert.AreEqual(StampsAccountContractType.Reseller, contractType);
        }

        [TestCategory("Stamps")]
        [TestCategory("ContinuousIntegration")]
        [TestMethod]
        public void ChangeToExpeditedPlan_Connectivity_Test()
        {
            // We just need to make sure we can connect and submit the request without
            // an exception. This will throw an exception since we're trying to convert
            // an existing reseller account.
            try
            {
                testObject.ChangeToExpeditedPlan(resellerAccount, "ShipWorks3");
            }
            catch (StampsApiException exception)
            {
                Assert.AreEqual(0x005f0302, exception.Code);
            }
        }
    }
}
