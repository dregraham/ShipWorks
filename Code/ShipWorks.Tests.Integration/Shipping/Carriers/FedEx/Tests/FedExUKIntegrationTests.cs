using System.Data;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Startup;
using ShipWorks.Tests.Integration.MSTest;
using ShipWorks.Tests.Integration.Shared;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.UK;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Express.International;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.Tests
{
    [Collection("Fedex Tests")]
    public class FedExUKIntegrationTests : DataDrivenIntegrationTestBase
    {
        private string UKAccountNumber = "605517846";
        private readonly ITestOutputHelper output;
        private DataContext context;

        public FedExUKIntegrationTests(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;

            context = db.GetFedExDataContext((_mock, _builder) => { },
                ShipWorksInitializer.GetShipWorksInstance());

            justLabels = false;
            justForPhysicalPrint = false;
            physicalPrintType = ThermalLanguage.None;

            SetupPhysicalPrints();
        }

        [ExcelData(@"DataSources\FedExAll\UK ETD.xlsx", "UK ETD")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExUKETD(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUKETDMapping.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = UKAccountNumber;
                testObject.CommercialInvoiceFileElectronically = true;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\UK International.xlsx", "UK International")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternational(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUkInternationalMapping.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[4]}");

                testObject.FedExAccountNumber = UKAccountNumber;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\UK Intra.xlsx", "UK Intra")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExUKIntra(DataRow row)
        {
            var testObject = new FedExUKIntraFixture();

            if (PopulateTestObject(row, testObject, FedExUKIntraFixture.Mapping) &&
                JustLabels(testObject.IsSaveLabel) &&
                PhysicalPrint(testObject.CustomerTransactionId, testObject.CustomerReferenceValue))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = UKAccountNumber;
                testObject.CommercialInvoiceFileElectronically = true;

                testObject.Ship(context.Order);
            }
        }
    }
}