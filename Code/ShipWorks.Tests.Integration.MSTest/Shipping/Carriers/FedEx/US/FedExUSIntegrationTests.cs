using System;
using Xunit;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.Domestic;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Ground;
using System.Data;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US
{
    public class FedExUSIntegrationTests : DataDrivenIntegrationTestBase
    {
        private string fedExTestAccountNumber = "604589967"; // "603103343";
        private const string ecodAccountNumber = "222326460";

        private bool justLabels = false;

        //[DataSource("DataSource_Ship_FedExUSGroundDomestic")]
        //[DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExUSGroundDomestic(DataRow row)
        {
            var testObject = new FedExUSGroundFixture();
            
            try
            {
                if (PopulateTestObject(row, testObject, FedExUSGroundFixture.UsGroundDomesticMapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    testObject.FedExAccountNumber = fedExTestAccountNumber;

                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                {
                    return;
                }

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", row[5], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        //[DataSource("DataSource_Ship_FedExGroundDomesticAlcohol")]
        //[DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExGroundDomesticAlcohol(DataRow row)
        {
            fedExTestAccountNumber = "510158040";

            var testObject = new FedExUSGroundAlcoholFixture();
            try
            {
                if (PopulateTestObject(row, testObject, FedExUSGroundAlcoholFixture.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    testObject.FedExAccountNumber = fedExTestAccountNumber;

                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                {
                    return;
                }

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", row[5], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        //[DataSource("DataSource_Ship_FedExExpressInternationalAlcohol")]
        //[DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternationalAlcohol(DataRow row)
        {
            fedExTestAccountNumber = "510158040";

            try
            {
                var testObject = new FedExUSExpressInternationalFixture();

                if (PopulateTestObject(row, testObject, FedExUSExpressInternationalAlcoholMapping.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", row["ProcessShipmentRequest#TransactionDetail"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);
                    testObject.FedExAccountNumber = fedExTestAccountNumber;

                    testObject.Ship();
                }
            }
            catch (Exception ex)
            {
                // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                {
                    return;
                }

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", row[5], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        //[DataSource("DataSource_Ship_FedExExpressInternational")]
        //[DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternational(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalFixture.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                try
                {
                    Console.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", testObject.CustomerTransactionId));
                    Console.WriteLine("--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    testObject.FedExAccountNumber = fedExTestAccountNumber;

                    testObject.Ship();
                }
                catch (Exception ex)
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                    {
                        return;
                    }

                    string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", row[5], ex.Message);
                    throw new Exception(msg, ex);
                }
            }
        }

        //[DataSource("DataSource_Ship_FedExExpressDomesticAlcohol")]
        //[DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressDomesticAlcohol(DataRow row)
        {
            fedExTestAccountNumber = "510158040";

            var testObject = new FedExPrototypeFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressDomesticAlcoholMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", row["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        //[DataSource("DataSource_Ship_FedExExpressDomestic")]
        //[DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressDomestic(DataRow row)
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressDomesticMapping.UsExpDomesticMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", row["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        //[DataSource("DataSource_Ship_FedExOneRate")]
        //[DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExOneRate_Test(DataRow row)
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSOneRateMapping.UsOneRateMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                Console.WriteLine(string.Format("Executing customer transaction ID {0}", row["ProcessShipmentRequest#TransactionDetail"]));
                Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship();
            }
        }

        //[DataSource("DataSource_Ship_FedExETD")]
        //[DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExETD(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalEtdMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                try
                {
                    Console.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", testObject.CustomerTransactionId));
                    Console.WriteLine("--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    testObject.FedExAccountNumber = fedExTestAccountNumber;
                    testObject.CommercialInvoiceFileElectronically = true;

                    testObject.Ship();
                }
                catch (Exception ex)
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                    {
                        return;
                    }

                    string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", row[5], ex.Message);
                    throw new Exception(msg, ex);
                }
            }
        }
    }
}