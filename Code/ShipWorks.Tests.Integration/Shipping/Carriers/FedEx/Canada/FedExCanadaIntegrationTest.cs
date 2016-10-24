using ShipWorks.Startup;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Express.Domestic;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Express.International;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Ground;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;
using ShipWorks.Tests.Shared.Database;
using System;
using System.Data;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada
{
    [Collection("Fedex Tests")]
    public class FedExCanadaIntegrationTest : DataDrivenIntegrationTestBase
    {
        private const string fedExTestAccountNumber = "612365903";
        private const bool justLabels = false;
        private readonly ITestOutputHelper output;

        private DataContext context;

        public FedExCanadaIntegrationTest(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;

            context = db.GetFedExDataContext(x => ContainerInitializer.Initialize(x),
                ShipWorksInitializer.GetShipWorksInstance());
        }

        [ExcelData(@"DataSources\FedExAll\CA Exp Dom.xlsx", "CA Exp Dom")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExCanadaExpressDomestic(DataRow row)
        {
            FedExCanadaExpressDomesticMapping testObject = new FedExCanadaExpressDomesticMapping();

            if (PopulateTestObject(row, testObject, FedExCanadaExpressDomesticMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                output.WriteLine($"Executing customer transaction ID {row["ProcessShipmentRequest#TransactionDetail"]}");
                output.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\CA Grn Dom Intl.xlsx", "CA Grn Dom Intl")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExCanadaGroundDomIntl(DataRow row)
        {
            FedExCAGroundDomesticInternationalFixture testObject = new FedExCAGroundDomesticInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExCanadaGroundDomesticInternationalMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }
        
        [ExcelData(@"DataSources\FedExAll\CA Exp Intl.xlsx", "CA Exp Intl")]
        [Trait("Category", "FedEx")]
        [Theory]
        public void Ship_FedExCanadaExpressInternational(DataRow row)
        {
            if (row[0] is DBNull)
            {
                return;
            }

            FedExUSExpressInternationalFixture testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExCanadaExpressInternationalMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine("{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                output.WriteLine($"Executing customer transaction ID {row["ProcessShipmentRequest#TransactionDetail"]}");
                output.WriteLine("--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }
    }
}