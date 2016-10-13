using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using System;
using System.Data;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.SmartPost
{
    [Collection("US Fedex Tests")]
    public class FedExSmartPostIntegrationTest : DataDrivenIntegrationTestBase
    {
        //note: smartpost gets it account number from the spreadsheet.
        private const bool justLabels = false;

        private readonly ITestOutputHelper output;
        private DataContext context;

        public FedExSmartPostIntegrationTest(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.GetFedExDataContext(x => ContainerInitializer.Initialize(x),
                ShipWorksInitializer.GetShipWorksInstance());

        }

        [ExcelData(@"DataSources\FedExAll.xlsx", "IMpB Smartpost test cases")]
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

                testObject.Ship(context.Order);
            }
        }
    }
}