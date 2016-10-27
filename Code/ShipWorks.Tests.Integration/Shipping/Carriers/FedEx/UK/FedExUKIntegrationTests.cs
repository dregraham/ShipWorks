using System.Data;
using ShipWorks.Startup;
using ShipWorks.Tests.Integration.MSTest;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.UK
{
    [Collection("Fedex Tests")]
    public class FedExUKIntegrationTests : DataDrivenIntegrationTestBase
    {
        private string UKAccountNumber = "605517846";
        private bool justLabels = true;
        private readonly ITestOutputHelper output;

        private DataContext context;


        public FedExUKIntegrationTests(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;

            context = db.GetFedExDataContext(x => ContainerInitializer.Initialize(x),
                ShipWorksInitializer.GetShipWorksInstance());
        }

        [ExcelData(@"DataSources\FedExAll\UK ETD.xlsx", "UK ETD")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExUKETD(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUKETDMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
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
                (testObject.IsSaveLabel || !justLabels)) // && (string) row[4] == "UK-404")
            {
                output.WriteLine($"Executing customer transaction ID {row[4]}");

                testObject.FedExAccountNumber = UKAccountNumber;

                testObject.Ship(context.Order);
            }
        }
    }
}