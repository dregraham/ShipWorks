using System.Data;
using ShipWorks.Startup;
using ShipWorks.Tests.Integration.MSTest;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.UK;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.EMEA
{
    [Collection("Fedex Tests")]
    public class FedExEMEAIntegrationTests : DataDrivenIntegrationTestBase
    {
        private string FinlandAccountNumber = "604818907";
        private string ItalyAccountNumber = "602810100";
        private string PolandAccountNumber = "604808006";
        private string SpainAccountNumber = "604824729";
        private string SwedenAccountNumber = "604849268";
        private string FranceAccountNumber = "602550940";
        private bool justLabels = true;
        private readonly ITestOutputHelper output;

        private DataContext context;

        public FedExEMEAIntegrationTests(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;

            context = db.GetFedExDataContext(x => ContainerInitializer.Initialize(x),
                ShipWorksInitializer.GetShipWorksInstance());
        }

        [ExcelData(@"DataSources\FedExAll\EMEA.xlsx", "EMEA")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExEMEA(DataRow row)
        {
            var testObject = new FedExEMEAFixture();

            if (PopulateTestObject(row, testObject, FedExEMEAFixture.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[4]}");

                testObject.FedExAccountNumber = GetAccountForCountry(testObject.ShipperCountryCode);

                testObject.CommercialInvoiceFileElectronically = true;

                testObject.Ship(context.Order);
            }
        }

        private string GetAccountForCountry(string countryCode)
        {
            switch (countryCode)
            {
                case "FI":
                    return FinlandAccountNumber;
                case "IT":
                    return ItalyAccountNumber;
                case "PL":
                    return PolandAccountNumber;
                case "ES":
                    return SpainAccountNumber;
                case "SE":
                    return SwedenAccountNumber;
                case "FR":
                    return FranceAccountNumber;
                default:
                    return "";
            }
        }
    }
}