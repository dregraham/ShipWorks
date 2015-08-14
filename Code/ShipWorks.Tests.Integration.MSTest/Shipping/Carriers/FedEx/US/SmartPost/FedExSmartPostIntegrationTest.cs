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

        [ExcelData(@"DataSources\FedExAll.xlsx", "IMpB Smartpost test cases")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExSmartPost(DataRow row)
        {
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