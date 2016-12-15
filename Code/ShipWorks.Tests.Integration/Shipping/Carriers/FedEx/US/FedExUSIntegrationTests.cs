using System;
using System.Collections.Generic;
using System.Data;
using ShipWorks.Startup;
using ShipWorks.Tests.Integration.MSTest;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Express.Domestic;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Express.International;
using ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Ground;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US
{
    [Collection("Fedex Tests")]
    public class FedExUSIntegrationTests : DataDrivenIntegrationTestBase
    {
        private string fedExTestAccountNumber = "612480567";
        private const string ecodAccountNumber = "222326460";
        private bool justLabels = false;
        private readonly ITestOutputHelper output;

        private DataContext context;


        public FedExUSIntegrationTests(FedExDatabaseFixture db, ITestOutputHelper output)
        {
            this.output = output;

            context = db.GetFedExDataContext(x => ContainerInitializer.Initialize(x),
                ShipWorksInitializer.GetShipWorksInstance());
        }

        [ExcelData(@"DataSources\FedExAll\US Grn Dom Intl And Home Del.xlsx", "US Grn Dom Intl And Home Del")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExUSGround(DataRow row)
        {
            if (row["SaveLabel"] is DBNull)
            {
                return;
            }

            output.WriteLine($"Preparing customer transaction ID {row[5]}");

            var testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSGroundFixture.UsGroundDomesticMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                // TransactionID 605733 should use ECOD account.
                List<string> transactionsForEcodAccount = new List<string>()
                {
                    "605733",
                    "605793",
                    "605797",
                    "605798"
                };

                testObject.FedExAccountNumber = transactionsForEcodAccount.Contains(testObject.CustomerTransactionId)
                    ? ecodAccountNumber : fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }


        [ExcelData(@"DataSources\FedExAll\Grn Alcohol.xlsx", "Grn Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExGroundDomesticAlcohol(DataRow row)
        {
            var testObject = new FedExUSGroundAlcoholFixture();

            if (PopulateTestObject(row, testObject, FedExUSGroundAlcoholFixture.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Exp Intl-Alcohol.xlsx", "US Exp Intl-Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternationalAlcohol(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalAlcoholMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Exp Intl.xlsx", "US Exp Intl")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressInternational(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalFixture.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))// && (string) row[5] == "413230")
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Exp Dom-Alcohol.xlsx", "US Exp Dom-Alcohol")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressDomesticAlcohol(DataRow row)
        {
            fedExTestAccountNumber = "612480567";

            var testObject = new FedExPrototypeFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressDomesticAlcoholMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\US Exp Dom.xlsx", "US Exp Dom")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExExpressDomestic(DataRow row)
        {
            output.WriteLine($"Preparing customer transaction ID {row[5]}");
            if (row["SaveLabel"] is DBNull || (!(bool)row["SaveLabel"] && justLabels)) // || (string) row[5] != "323234")
            {
                output.WriteLine("Skipping");
                return;
            }

            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressDomesticMapping.UsExpDomesticMapping))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\OneRate.xlsx", "OneRate")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExOneRate(DataRow row)
        {
            FedExPrototypeFixture testObject = new FedExUSGroundFixture();

            if (PopulateTestObject(row, testObject, FedExUSOneRateMapping.UsOneRateMapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;

                testObject.Ship(context.Order);
            }
        }

        [ExcelData(@"DataSources\FedExAll\ETD.xlsx", "ETD")]
        [Theory]
        [Trait("Category", "FedEx")]
        public void Ship_FedExETD(DataRow row)
        {
            var testObject = new FedExUSExpressInternationalFixture();

            if (PopulateTestObject(row, testObject, FedExUSExpressInternationalEtdMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine($"Executing customer transaction ID {row[5]}");

                testObject.FedExAccountNumber = fedExTestAccountNumber;
                testObject.CommercialInvoiceFileElectronically = true;

                testObject.Ship(context.Order);
            }
        }
    }
}