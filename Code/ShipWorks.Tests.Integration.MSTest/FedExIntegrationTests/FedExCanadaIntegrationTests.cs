using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx;

namespace ShipWorks.Tests.Integration.MSTest.FedExIntegrationTests
{
    [TestClass]
    public class FedExCanadaIntegrationTests : IntegrationTestBase
    {
        private const string fedExTestAccountNumber = "603153529";

        private bool justLabels = true;


        [DataSource("DataSource_Ship_FedExCanadaExpressDomestic")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExCanadaExpressDomestic()
        {
            try
            {
                FedExCanadaExpressDomesticFixture testObject = new FedExCanadaExpressDomesticFixture();

                if (PopulateTestObject(testObject, FedExCanadaExpressDomesticFixture.Mapping) && testObject.IsSaveLabel)
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", this.TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    testObject.FedExAccountNumber = fedExTestAccountNumber;

                    testObject.Ship();
                }
            }
            catch (Exception)
            {
                if (string.IsNullOrWhiteSpace(TestContext.DataRow[0].ToString().Trim()))
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    return;
                }

                // We have a legitimate exception
                throw;
            }
        }

        [DataSource("DataSource_Ship_FedExCanadaGroundDomIntl")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExCanadaGroundDomIntl()
        {
            FedExCAGroundDomesticInternational testObject = new FedExCAGroundDomesticInternational();
            try
            {
                if (PopulateTestObject(testObject, FedExCAGroundDomesticInternational.FedExCAGroundDomesticInternationalMapping) && testObject.IsSaveLabel)
                {
                    testObject.FedExAccountNumber = fedExTestAccountNumber;

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

        [DataSource("DataSource_Ship_FedExCanadaExpressInternational")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        public void Ship_FedExCanadaExpressInternational()
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();

            try
            {
                if (PopulateTestObject(testObject, FedExCaExpressInternationalMapping.CaExpressInternationalMapping) && testObject.IsSaveLabel)
                {
                    Console.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", this.TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                    Console.WriteLine("--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    testObject.FedExAccountNumber = fedExTestAccountNumber;

                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
        }
    }
}