using System;
using Xunit;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Express.Domestic;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Express.International;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Ground;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;
using System.Data;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada
{
    public class FedExCanadaIntegrationTest : DataDrivenIntegrationTestBase
    {
        private const string fedExTestAccountNumber = "604601185";

        private const bool justLabels = true;

        //// [DataSource("DataSource_Ship_FedExCanadaExpressDomestic")]
        //// [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Trait("Category", "FedEx")]
        [Theory]
        [ExcelData(@"DataSources\FedExAll.xlsx", "CA Exp Dom")]
        public void Ship_FedExCanadaExpressDomestic(DataRow row)
        {
            //try
            //{
                FedExCanadaExpressDomesticMapping testObject = new FedExCanadaExpressDomesticMapping();
                
                if (PopulateTestObject(row, testObject, FedExCanadaExpressDomesticMapping.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", row["ProcessShipmentRequest#TransactionDetail"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    testObject.FedExAccountNumber = fedExTestAccountNumber;

                    testObject.Ship();
                }
            //}
            //catch (Exception)
            //{
            //    if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
            //    {
            //        // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
            //        return;
            //    }

            //    // We have a legitimate exception
            //    throw;
            //}
        }

        // [DataSource("DataSource_Ship_FedExCanadaGroundDomIntl")]
        // [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExCanadaGroundDomIntl(DataRow row)
        {
            FedExCAGroundDomesticInternationalFixture testObject = new FedExCAGroundDomesticInternationalFixture();
            try
            {
                if (PopulateTestObject(row, testObject, FedExCanadaGroundDomesticInternationalMapping.Mapping) &&
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

                string msg = string.Format("CustomerTransactionID: {0}, Message: {1}", row["ProcessShipmentRequest#TransactionDetail"], ex.Message);
                throw new Exception(msg, ex);
            }
        }

        // [DataSource("DataSource_Ship_FedExCanadaExpressInternational")]
        // [DeploymentItem("DataSources\\FedExAll.xlsx")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExCanadaExpressInternational(DataRow row)
        {
            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();

            try
            {
                if (PopulateTestObject(row, testObject, FedExCanadaExpressInternationalMapping.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    Console.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing customer transaction ID {0}", row["ProcessShipmentRequest#TransactionDetail"]));
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