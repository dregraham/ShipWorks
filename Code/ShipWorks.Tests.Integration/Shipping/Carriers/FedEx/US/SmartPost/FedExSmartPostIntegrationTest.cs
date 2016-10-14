using System;
using System.Data;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.SmartPost
{
    public class FedExSmartPostIntegrationTest : DataDrivenIntegrationTestBase
    {
        //note: smartpost gets it account number from the spreadsheet.
        private const bool justLabels = false;
        private readonly ITestOutputHelper output;

        public FedExSmartPostIntegrationTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [ExcelData(@"DataSources\FedExAll\IMpB Smartpost.xlsx", "IMpB Smartpost")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExSmartPost(DataRow row)
        {
            if (row["SaveLabel"] is DBNull || (string)row["SaveLabel"] != "TRUE")
            {
                return;
            }

            FedExSmartPostFixture testObject = new FedExSmartPostFixture();

            if (PopulateTestObject(row, testObject, FedExSmartPostFixture.SmartPostMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row["ProcessShipmentRequest#TransactionDetail"]}");

                testObject.Ship();
            }
        }
    }
}