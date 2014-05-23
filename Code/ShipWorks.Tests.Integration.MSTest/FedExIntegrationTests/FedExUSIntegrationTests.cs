using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx;

namespace ShipWorks.Tests.Integration.MSTest.FedExIntegrationTests
{
    [TestClass]
    public class FedExUSIntegrationTests : IntegrationTestBase
    {
        private string fedExTestAccountNumber = "603103343";
        private const string ecodAccountNumber = "222326460";

        private bool justLabels = true;

        [DataSource("DataSource_Ship_FedExUSGroundDomestic")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_FedExUSGroundDomestic()
        {
            var testObject = new FedExUSGroundFixture();

            try
            {
                if (PopulateTestObject(testObject, FedExUSGroundFixture.UsGroundDomesticMapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    // TransactionID 605733 should use ECOD account.
                    testObject.FedExAccountNumber = testObject.CustomerTransactionId == "605733" ? ecodAccountNumber : fedExTestAccountNumber;

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

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", TestContext.DataRow[5], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        [DataSource("DataSource_Ship_FedExGroundDomesticAlcohol")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_FedExGroundDomesticAlcohol()
        {
            fedExTestAccountNumber = "510158040";

            var testObject = new FedExUSGroundAlcoholFixture();
            try
            {
                if (PopulateTestObject(testObject, FedExUSGroundAlcoholFixture.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
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

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", TestContext.DataRow[5], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        [DataSource("DataSource_Ship_FedExExpressInternationalAlcohol")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_FedExExpressInternationalAlcohol()
        {
            fedExTestAccountNumber = "510158040";

            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(testObject, FedExUsExpressInternationalAlcoholMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);
                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_FedExExpressInternational")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_FedExExpressInternational()
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(testObject, FedExUSExpressInternationalFixture.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                Console.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", testObject.CustomerTransactionId));
                Console.WriteLine("--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_FedExExpressDomesticAlcohol")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_FedExExpressDomesticAlcohol()
        {
            fedExTestAccountNumber = "510158040";

            var testObject = new FedExPrototypeFixture();

            if (PopulateTestObject(testObject, FedExUSExpressDomesticAlcoholMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_FedExExpressDomestic")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_FedExExpressDomestic()
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(testObject, FedExUsExpressDomesticMapping.UsExpDomesticMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_FedExOneRate")]
        [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_FedExOneRate_Test()
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(testObject, FedExUsExpressDomesticMapping.UsExpDomesticMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", TestContext.DataRow["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }
    }
}