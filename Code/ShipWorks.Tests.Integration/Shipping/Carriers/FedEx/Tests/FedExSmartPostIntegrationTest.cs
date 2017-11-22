using System;
using System.Data;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Startup;
using ShipWorks.Tests.Integration.MSTest;
using ShipWorks.Tests.Integration.Shared;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.SmartPost;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.Tests
{
    [Collection("Fedex Tests")]
    public class FedExSmartPostIntegrationTest : DataDrivenIntegrationTestBase
    {
        //note: smartpost gets it account number from the spreadsheet.
        private readonly ITestOutputHelper output;
        private DataContext context;

        public FedExSmartPostIntegrationTest(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;
            context = db.GetFedExDataContext((_mock, _builder) => { },
                ShipWorksInitializer.GetShipWorksInstance());

            justLabels = false;
            justForPhysicalPrint = false;
            physicalPrintType = ThermalLanguage.None;

            SetupPhysicalPrints();
        }

        [ExcelData(@"DataSources\FedExAll\IMpB Smartpost.xlsx", "IMPB SmartPost")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExSmartPost(DataRow row)
        {
            if (row[0] is DBNull || row[0].ToString().ToUpperInvariant() != "TRUE")
            {
                return;
            }

            FedExSmartPostFixture testObject = new FedExSmartPostFixture();

            if (PopulateTestObject(row, testObject, FedExSmartPostFixture.SmartPostMapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row["ProcessShipmentRequest#TransactionDetail"]}");

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }

        [ExcelData(@"DataSources\FedExAll\IMPB SmartPost Returns.xlsx", "IMPB SmartPost")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExSmartPostReturns(DataRow row)
        {
            if (row[0] is DBNull || row[0].ToString().ToUpperInvariant() != "TRUE")
            {
                return;
            }

            FedExSmartPostFixture testObject = new FedExSmartPostFixture();

            if (PopulateTestObject(row, testObject, FedExSmartPostFixture.SmartPostMapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row["ProcessShipmentRequest#TransactionDetail"]}");

                testObject.Ship(context.Order, justForPhysicalPrint);
            }
        }
    }
}