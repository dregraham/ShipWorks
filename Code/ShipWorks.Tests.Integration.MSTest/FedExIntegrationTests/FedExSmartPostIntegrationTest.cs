using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx;

namespace ShipWorks.Tests.Integration.MSTest.FedExIntegrationTests
{
    [TestClass]
    public class FedExSmartPostIntegrationTest : IntegrationTestBase
    {

        //note: smartpost gets it account number from the spreadsheet.

        [DataSource("DataSource_Ship_FedExSmartPost")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExSmartPost()
        {
            FedExSmartPostFixture testObject = new FedExSmartPostFixture();
            try
            {
                if (PopulateTestObject(testObject, FedExSmartPostFixture.SmartPostMapping))
                {
                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                if (string.IsNullOrWhiteSpace(TestContext.DataRow[0].ToString().Trim()))
                {
                    return;
                }

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"], ex.Message);
                throw new Exception(msg, ex);
            }
        }
    }
}